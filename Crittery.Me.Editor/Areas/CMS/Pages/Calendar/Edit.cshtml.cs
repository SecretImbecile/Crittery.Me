// <copyright file="Edit.cshtml.cs" company="Jack Kelly">
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
    using Crittery.Me.Data.Enums;
    using Crittery.Me.Data.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Page model for the edit calendar event form.
    /// </summary>
    public class EditModel(CritteryMeContext context) : PageModel
    {
        /// <summary>
        /// Gets the dropdown options for calendar event types.
        /// </summary>
        public static IEnumerable<SelectListItem> EventTypes => Enum
            .GetNames<CalendarEventType>()
            .Order()
            .Select(type => new SelectListItem(type, type))
            .Prepend(new SelectListItem("Select...", string.Empty));

        /// <summary>
        /// Gets or sets the submitted form data.
        /// </summary>
        [BindProperty]
        public CalendarEvent FormData { get; set; } = null!;

        /// <summary>
        /// Loads the edit calendar event form.
        /// </summary>
        /// <param name="id">The ID of the interest to edit.</param>
        /// <returns>
        /// A <see cref="Task"/> representing the duration of the request.
        /// Results in a page view.
        /// </returns>
        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            CalendarEvent? dbCalendarEvent = await context.CalendarEvents
                .AsNoTracking()
                .SingleOrDefaultAsync(calendarEvent => calendarEvent.Id == id);

            if (dbCalendarEvent != null)
            {
                this.FormData = dbCalendarEvent;

                return this
                    .Page();
            }
            else
            {
                return this
                    .NotFound();
            }
        }

        /// <summary>
        /// Submits the edit calendar event form.
        /// </summary>
        /// <returns>
        /// A <see cref="Task"/> representing the duration of the request.
        /// Results in a page redirect.
        /// </returns>
        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            CalendarEvent? dbCalendar = await context.CalendarEvents
                .SingleOrDefaultAsync(calendarEvent => calendarEvent.Id == id);

            if (dbCalendar != null)
            {
                dbCalendar.DateStart = this.FormData.DateStart;
                dbCalendar.DateEnd = this.FormData.DateEnd;
                dbCalendar.Type = this.FormData.Type;
                dbCalendar.Name = this.FormData.Name;
                dbCalendar.Url = this.FormData.Url;

                _ = await context
                .SaveChangesAsync();

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
