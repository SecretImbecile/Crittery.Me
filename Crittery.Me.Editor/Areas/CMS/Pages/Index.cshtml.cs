namespace Crittery.Me.Editor.Areas.CMS.Pages
{
    using Crittery.Me.Data;
    using Crittery.Me.Data.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;

    public class IndexModel(CritteryMeContext context) : PageModel
    {
        private const int TopN = 10;
        private const int TopNPhotos = 6;

        /// <summary>
        /// Fetch the dashboard AJAX partial for bio content.
        /// </summary>
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
        public async Task<IActionResult> OnGetRelationshipsAsync()
        {
            IEnumerable<RelationshipContent> model = await context.RelationshipItems
                .OrderBy(content => content.Order)
                .Take(TopN)
                .ToListAsync();

            return this
                .Partial("Partials/_Dashboard_Relationships", model);
        }
    }
}
