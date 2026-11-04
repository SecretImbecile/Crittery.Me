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
        /// Submits the new calendar event form.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync()
        {
            CalendarEvent newItem = FormData with
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
