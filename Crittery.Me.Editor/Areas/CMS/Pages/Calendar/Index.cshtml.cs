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

namespace Crittery.Me.Editor.Areas.CMS.Pages.Calendar
{
    using Crittery.Me.Data;
    using Crittery.Me.Data.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Page model for the event listing.
    /// </summary>
    public class IndexModel(CritteryMeContext context) : PageModel
    {
        /// <summary>
        /// Gets or sets the events to list.
        /// </summary>
        public List<CalendarEvent> Events { get; set; } = [];

        /// <summary>
        /// Gets the upcoming events to list.
        /// </summary>
        public IEnumerable<CalendarEvent> UpcomingEvents => this.Events
            .Where(calendarEvent => calendarEvent.DateEnd >= DateOnly.FromDateTime(DateTime.Today));

        /// <summary>
        /// Gets the past events to list.
        /// </summary>
        public IEnumerable<CalendarEvent> PastEvents => this.Events
            .Where(calendarEvent => calendarEvent.DateEnd < DateOnly.FromDateTime(DateTime.Today));

        /// <summary>
        /// Load the calendar events.
        /// </summary>
        /// <returns>
        /// A <see cref="Task"/> representing the duration of the page request.
        /// </returns>
        public async Task OnGetAsync()
        {
            this.Events = await context.CalendarEvents
                .OrderByDescending(calendarEvent => calendarEvent.DateStart)
                .ToListAsync();
        }

        /// <summary>
        /// Delete all past calendar events.
        /// </summary>
        /// <returns>
        /// A <see cref="Task"/> representing the duration of the request.
        /// Results in a page redirect.
        /// </returns>
        public async Task<IActionResult> OnGetClearAsync()
        {
            _ = await context.CalendarEvents
                .Where(calendarEvent => calendarEvent.DateEnd < DateOnly.FromDateTime(DateTime.Today))
                .ExecuteDeleteAsync();

            return this
                .RedirectToPage("/Calendar/Index", new { Area = "CMS" });
        }

        /// <summary>
        /// Delete a calendar event.
        /// </summary>
        /// <param name="id">ID of the calendar event to delete.</param>
        /// <returns>
        /// A <see cref="Task"/> representing the duration of the request.
        /// Results in a page redirect.
        /// </returns>
        public async Task<IActionResult> OnGetDeleteAsync(Guid id)
        {
            int count = await context.CalendarEvents
                .Where(calendarEvent => calendarEvent.Id == id)
                .ExecuteDeleteAsync();

            if (count > 0)
            {
                return this
                    .RedirectToPage("/Calendar/Index", new { Area = "CMS" });
            }
            else
            {
                return this
                    .NotFound();
            }
        }
    }
}
