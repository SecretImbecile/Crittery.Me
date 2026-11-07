using Crittery.Me.Data;
using Crittery.Me.Data.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;

namespace Crittery.Me.Pages
{
    public class IndexModel(CritteryMeContext context) : PageModel
    {
        /// <summary>
        /// Gets or sets the next meet and convention I'm attending.
        /// </summary>
        public List<CalendarEvent> ContentCalendar { get; private set; } = [];

        /// <summary>
        /// Gets or sets the bio content.
        /// </summary>
        public List<BioContent> ContentBio { get; set; } = [];

        /// <summary>
        /// Gets or sets the interests content.
        /// </summary>
        public List<InterestContent> ContentInterests { get; set; } = [];

        /// <summary>
        /// Gets or sets the relationships content.
        /// </summary>
        public List<RelationshipContent> ContentRelationships { get; set; } = [];

        public List<Link> Links { get; set; } = [];

        /// <summary>
        /// Gets or sets a value indicating whether today is an event day.
        /// </summary>
        public bool IsDayOfEvent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether extra content should be displayed.
        /// </summary>
        /// <remarks>
        /// Value is true <see langword="true"/> when <see cref="IsDayOfEvent"/> is true, or when a specific query parameter is present.
        /// </remarks>
        public bool IsExtraContentEnabled { get; set; }

        /// <summary>
        /// Gets my current age.
        /// </summary>
        public int Age
        {
            get
            {
                DateTime today = DateTime.Today;
                int year = today.Year;
                int month = today.Month;
                int day = today.Day;
                
                if (month > 6 || (month == 6 && day >= 15))
                {
                    return year - 1994;
                }
                {
                    return year - 1994 - 1;
                }
            }
        }

        public async Task OnGetAsync()
        {
            CalendarEvent? nextMeet = await context.CalendarEvents
                .Where(item => item.Type == Data.Enums.CalendarEventType.Meet)
                .Where(item => item.DateEnd >= DateOnly.FromDateTime(DateTime.Today))
                .OrderBy(item => item.DateStart)
                .FirstOrDefaultAsync();

            CalendarEvent? nextConvention = await context.CalendarEvents
                .Where(item => item.Type == Data.Enums.CalendarEventType.Convention)
                .Where(item => item.DateEnd >= DateOnly.FromDateTime(DateTime.Today))
                .OrderBy(item => item.DateStart)
                .FirstOrDefaultAsync();

            // Next meet and convention, sorted by date
            this.ContentCalendar = new List<CalendarEvent?>() { nextMeet, nextConvention }
                .OfType<CalendarEvent>()
                .OrderBy(calendarEvent => calendarEvent.DateStart)
                .ToList();

            this.ContentBio = await context.BioItems
                .OrderBy(item => item.Order)
                .ToListAsync();

            this.ContentInterests = await context.Interests
                .OrderBy(item => item.Order)
                .ToListAsync();

            this.ContentRelationships = await context.RelationshipItems
                .OrderBy(item => item.Order)
                .ToListAsync();

            this.Links = await context.Links
                .OrderBy(item => item.Order)
                .ToListAsync();

            // Substitute my actual age
            foreach(BioContent item in this.ContentBio)
            {
                if (item.Text.Equals("{Age}", StringComparison.OrdinalIgnoreCase))
                {
                    item.Text = this.Age.ToString();
                }
            }

            // Determine the extra content flags
            DateOnly today = DateOnly
                .FromDateTime(DateTime.Today);
            
            this.IsDayOfEvent = this.ContentCalendar
                .Where(calendarEvent => calendarEvent.DateStart <= today && calendarEvent.DateEnd >= today)
                .Any();

            this.IsExtraContentEnabled = this.IsDayOfEvent || DetermineExtraContentParameters();
        }

        /// <summary>
        /// Determines if a valid query parameter is present viewing the extra homepage content.
        /// </summary>
        /// <returns>A <see langword="bool"/> indicating whether extra content should be displayed.</returns>
        private bool DetermineExtraContentParameters()
        {
            StringValues sourceParameter = Request.Query["from"];
            
            return sourceParameter.Contains("CMS", StringComparer.OrdinalIgnoreCase)
                || sourceParameter.Contains("NFC", StringComparer.OrdinalIgnoreCase)
                || sourceParameter.Contains("QR", StringComparer.OrdinalIgnoreCase);
        }
    }
}
