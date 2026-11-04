using Crittery.Me.Data;
using Crittery.Me.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Crittery.Me.Editor.Areas.CMS.Pages.Bio
{
    /// <summary>
    /// Page model for the new bio item form.
    /// </summary>
    public class NewModel(CritteryMeContext context) : PageModel
    {
        /// <summary>
        /// Gets or sets the submitted form data.
        /// </summary>
        [BindProperty]
        public BioItem FormData { get; set; } = null!;

        /// <summary>
        /// Submits the new bio item form.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync()
        {
            int? highestOrderValue = await context.BioItems
                .OrderByDescending(bioItem => bioItem.Order)
                .Select(bioItem => bioItem.Order)
                .FirstOrDefaultAsync();

            highestOrderValue = highestOrderValue ?? 0;

            BioItem newItem = FormData with
            {
                Id = Guid.NewGuid(),
                Order = highestOrderValue.Value + 1,
            };

            _ = context.BioItems
                .Add(newItem);

            _ = await context
                .SaveChangesAsync();

            return this.RedirectToPage("/Bio/Index", new { Area = "CMS" });

        }
    }
}
