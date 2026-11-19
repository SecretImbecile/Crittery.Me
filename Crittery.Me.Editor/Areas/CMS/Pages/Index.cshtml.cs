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

namespace Crittery.Me.Editor.Areas.CMS.Pages
{
    using Crittery.Me.Data;
    using Crittery.Me.Data.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Page model for the CMS dashboard.
    /// </summary>
    public class IndexModel(CritteryMeContext context) : PageModel
    {
        private const int TopN = 10;
        private const int TopNPhotos = 6;

        /// <summary>
        /// Fetch the dashboard AJAX partial for bio content.
        /// </summary>
        /// <returns>
        /// A <see cref="Task"/> representing the duration of the request.
        /// Results in a partial view.
        /// </returns>
        public async Task<IActionResult> OnGetBioAsync()
        {
            IEnumerable<BioContent> model = await context.BioItems
                .OrderBy(content => content.Order)
                .Take(TopN)
                .ToListAsync();

            return this
                .Partial("Partials/_Dashboard_Bio", model);
        }

        /// <summary>
        /// Fetch the dashboard AJAX partial for calendar events.
        /// </summary>
        /// <returns>
        /// A <see cref="Task"/> representing the duration of the request.
        /// Results in a partial view.
        /// </returns>
        public async Task<IActionResult> OnGetCalendarAsync()
        {
            IEnumerable<CalendarEvent> model = await context.CalendarEvents
                .OrderByDescending(calendarEvent => calendarEvent.DateEnd)
                .Take(TopN)
                .ToListAsync();

            return this
                .Partial("Partials/_Dashboard_Calendar", model);
        }

        /// <summary>
        /// Fetch the dashboard AJAX partial for photos.
        /// </summary>
        /// <returns>
        /// A <see cref="Task"/> representing the duration of the request.
        /// Results in a partial view.
        /// </returns>
        public async Task<IActionResult> OnGetPhotosAsync()
        {
            IEnumerable<Crittery.Me.Data.Models.Photo> model = await context.Photos
                .OrderByDescending(photo => photo.CapturedOn)
                .Take(TopNPhotos)
                .ToListAsync();

            return this
                .Partial("Partials/_Dashboard_Photos", model);
        }

        /// <summary>
        /// Fetch the dashboard AJAX partial for interest content.
        /// </summary>
        /// <returns>
        /// A <see cref="Task"/> representing the duration of the request.
        /// Results in a partial view.
        /// </returns>
        public async Task<IActionResult> OnGetInterestsAsync()
        {
            IEnumerable<InterestContent> model = await context.Interests
                .OrderBy(content => content.Order)
                .Take(TopN)
                .ToListAsync();

            return this
                .Partial("Partials/_Dashboard_Interests", model);
        }

        /// <summary>
        /// Fetch the dashboard AJAX partial for links.
        /// </summary>
        /// <returns>
        /// A <see cref="Task"/> representing the duration of the request.
        /// Results in a partial view.
        /// </returns>
        public async Task<IActionResult> OnGetLinksAsync()
        {
            IEnumerable<Crittery.Me.Data.Models.Link> model = await context.Links
                .OrderBy(content => content.Order)
                .Take(TopN)
                .ToListAsync();

            return this
                .Partial("Partials/_Dashboard_Links", model);
        }

        /// <summary>
        /// Fetch the dashboard AJAX partial for relationship content.
        /// </summary>
        /// <returns>
        /// A <see cref="Task"/> representing the duration of the request.
        /// Results in a partial view.
        /// </returns>
        public async Task<IActionResult> OnGetRelationshipsAsync()
        {
            IEnumerable<RelationshipContent> model = await context.RelationshipItems
                .OrderBy(content => content.Order)
                .Take(TopN)
                .ToListAsync();

            return this
                .Partial("Partials/_Dashboard_Relationships", model);
        }

        /// <summary>
        /// Fetch the dashboard AJAX partial for visitor/view count.
        /// </summary>
        /// <returns>
        /// A <see cref="Task"/> representing the duration of the request.
        /// Results in a partial view.
        /// </returns>
        public async Task<IActionResult> OnGetVisitorsAsync()
        {
            int model = await context.Visitors
                .SumAsync(visitor => visitor.Count);

            return this
                .Partial("Partials/_Dashboard_Visitors", model);
        }
    }
}
