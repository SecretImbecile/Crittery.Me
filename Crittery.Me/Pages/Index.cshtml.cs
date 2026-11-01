using Crittery.Me.Data;
using Crittery.Me.Data.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Crittery.Me.Pages
{
    public class IndexModel(CritteryMeContext context) : PageModel
    {
        /// <summary>
        /// Gets or sets the next convention I'm attending.
        /// </summary>
        public CalendarEvent? NextConvention { get; private set; }

        /// <summary>
        /// Gets the next meet I'm attending.
        /// </summary>
        public CalendarEvent? NextMeet { get; private set; }

        /// <summary>
        /// Gets or sets the bio items to display.
        /// </summary>
        public List<BioItem> BioItems { get; set; } = [];

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
            this.NextConvention = await context.CalendarEvents
                .Where(item => item.Type == Data.Enums.CalendarEventType.Convention)
                .Where(item => item.DateEnd >= DateOnly.FromDateTime(DateTime.Today))
                .OrderBy(item => item.DateStart)
                .FirstOrDefaultAsync();

            this.NextMeet = await context.CalendarEvents
                .Where(item => item.Type == Data.Enums.CalendarEventType.Meet)
                .Where(item => item.DateEnd >= DateOnly.FromDateTime(DateTime.Today))
                .OrderBy(item => item.DateStart)
                .FirstOrDefaultAsync();


            this.BioItems = await context.BioItems
                .OrderBy(item => item.Order)
                .ToListAsync();

            // Substitute my actual age
            foreach(BioItem item in this.BioItems)
            {
                if (item.Text.Equals("{Age}", StringComparison.OrdinalIgnoreCase))
                {
                    item.Text = this.Age.ToString();
                }
            }
        }
    }
}
