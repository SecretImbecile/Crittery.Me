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
        /// Gets or sets the submitted form data.
        /// </summary>
        [BindProperty]
        public CalendarEvent FormData { get; set; } = null!;

        public IEnumerable<SelectListItem> EventTypes => Enum
            .GetNames<CalendarEventType>()
            .Order()
            .Select(type => new SelectListItem(type, type))
            .Prepend(new SelectListItem("Select...", string.Empty));

        /// <summary>
        /// Loads the edit calendar event form.
        /// </summary>
        /// <param name="id">The ID of the interest to edit.</param>
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
                    .NotFound("");
            }
        }

        /// <summary>
        /// Submits the edit calendar event form.
        /// </summary>
        /// <returns></returns>
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
                    .NotFound("");
            }
        }
    }
}
