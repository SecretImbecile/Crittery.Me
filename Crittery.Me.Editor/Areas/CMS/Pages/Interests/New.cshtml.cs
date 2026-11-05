using Crittery.Me.Data;
using Crittery.Me.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Crittery.Me.Editor.Areas.CMS.Pages.Interests
{
    /// <summary>
    /// Page model for the new interests form.
    /// </summary>
    public class NewModel(CritteryMeContext context) : PageModel
    {
        /// <summary>
        /// Gets or sets the submitted form data.
        /// </summary>
        [BindProperty]
        public InterestContent FormData { get; set; } = null!;

        /// <summary>
        /// Submits the new interest form.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync()
        {
            int? highestOrderValue = await context.Interests
                .OrderByDescending(interest => interest.Order)
                .Select(interest => interest.Order)
                .FirstOrDefaultAsync();

            highestOrderValue = highestOrderValue
                ?? 0;

            InterestContent newInterest = FormData with
            {
                Id = Guid.NewGuid(),
                Order = highestOrderValue.Value + 1,
            };

            _ = context.Interests
                .Add(newInterest);

            _ = await context
                .SaveChangesAsync();

            return this.RedirectToPage("/Interests/Index", new { Area = "CMS" });

        }
    }
}
