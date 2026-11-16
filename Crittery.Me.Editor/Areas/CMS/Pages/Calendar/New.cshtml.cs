// <copyright file="New.cshtml.cs" company="Jack Kelly">
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

    /// <summary>
    /// Page model for the new calendar event form.
    /// </summary>
    public class NewModel(CritteryMeContext context) : PageModel
    {
        /// <summary>
        /// Gets dropdown options for event types.
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
        /// Submits the new calendar event form.
        /// </summary>
        /// <returns>
        /// A <see cref="Task"/> representing the duration of the request.
        /// Results in a page redirect.
        /// </returns>
        public async Task<IActionResult> OnPostAsync()
        {
            CalendarEvent newItem = this.FormData with
            {
                Id = Guid.NewGuid(),
            };

            _ = context.CalendarEvents
                .Add(newItem);

            _ = await context
                .SaveChangesAsync();

            return this.RedirectToPage("/Calendar/Index", new { Area = "CMS" });
        }
    }
}
