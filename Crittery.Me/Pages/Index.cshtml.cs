// <copyright file="Index.cshtml.cs" company="Jack Kelly">
// Copyright © 2026 Jack Kelly.
//
// The crittery.me website is free software: you can redistribute it and/or modify it
// under the terms of the GNU General Public License as published by the
// Free Software Foundation, either version 3 of the License, or
// (at your option)any later version.
//
// The crittery.me source code is distributed in the hope that it will be useful, but
// WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
// or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for
// more details.
//
// You should have received a copy of the GNU General Public License along with
// The crittery.me source code. If not, see [https://www.gnu.org/licenses/].
// </copyright>

namespace Crittery.Me.Pages
{
    using Crittery.Me.Data;
    using Crittery.Me.Data.Models;
    using Crittery.Me.Services;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Primitives;

    /// <summary>
    /// Page model for the site homepage.
    /// </summary>
    public class IndexModel(CritteryMeContext context, IVisitorService visitorService) : PageModel
    {
        /// <summary>
        /// Gets the next meet and convention I'm attending.
        /// </summary>
        public List<CalendarEvent> ContentCalendar { get; private set; } = [];

        /// <summary>
        /// Gets the bio content.
        /// </summary>
        public List<BioContent> ContentBio { get; private set; } = [];

        /// <summary>
        /// Gets the interests content.
        /// </summary>
        public List<InterestContent> ContentInterests { get; private set; } = [];

        /// <summary>
        /// Gets the relationships content.
        /// </summary>
        public List<RelationshipContent> ContentRelationships { get; private set; } = [];

        /// <summary>
        /// Gets the links content.
        /// </summary>
        public List<Link> Links { get; private set; } = [];

        /// <summary>
        /// Gets a value indicating whether today is an event day.
        /// </summary>
        public bool IsDayOfEvent { get; private set; }

        /// <summary>
        /// Gets a value indicating whether extra content should be displayed.
        /// </summary>
        /// <remarks>
        /// Value is true <see langword="true"/> when <see cref="IsDayOfEvent"/> is true, or when a specific query parameter is present.
        /// </remarks>
        public bool IsExtraContentEnabled { get; private set; }

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
                else
                {
                    return year - 1994 - 1;
                }
            }
        }

        /// <summary>
        /// Loads the homepage.
        /// </summary>
        /// <returns>
        /// A <see cref="Task"/> representing the duration of the page request.
        /// </returns>
        public async Task OnGetAsync()
        {
            // Record the visit
            await visitorService.RecordVisitAsync(this.Request, "/");

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
            foreach (BioContent item in this.ContentBio)
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

            this.IsExtraContentEnabled = this.IsDayOfEvent || this.DetermineExtraContentParameters();
        }

        /// <summary>
        /// Determines if a valid query parameter is present viewing the extra homepage content.
        /// </summary>
        /// <returns>A <see langword="bool"/> indicating whether extra content should be displayed.</returns>
        private bool DetermineExtraContentParameters()
        {
            StringValues sourceParameter = this.Request.Query["from"];

            return sourceParameter.Contains("CMS", StringComparer.OrdinalIgnoreCase)
                || sourceParameter.Contains("NFC", StringComparer.OrdinalIgnoreCase)
                || sourceParameter.Contains("QR", StringComparer.OrdinalIgnoreCase);
        }
    }
}
