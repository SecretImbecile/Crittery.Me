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
        public List<CalendarEvent> Events { get; set; } = [];

        public IEnumerable<CalendarEvent> UpcomingEvents => this.Events
            .Where(calendarEvent => calendarEvent.DateEnd >= DateOnly.FromDateTime(DateTime.Today));

        public IEnumerable<CalendarEvent> PastEvents => this.Events
            .Where(calendarEvent => calendarEvent.DateEnd < DateOnly.FromDateTime(DateTime.Today));

        /// <summary>
        /// Load the calendar events.
        /// </summary>
        public async Task OnGetAsync()
        {
            this.Events = await context.CalendarEvents
                .OrderByDescending(calendarEvent => calendarEvent.DateStart)
                .ToListAsync();
        }

        /// <summary>
        /// Delete all past calendar events.
        /// </summary>
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
