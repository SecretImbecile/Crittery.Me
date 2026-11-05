namespace Crittery.Me.Editor.Areas.CMS.Pages.Relationship
{
    using Crittery.Me.Data;
    using Crittery.Me.Data.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Page model for the new relationship item form.
    /// </summary>
    public class NewModel(CritteryMeContext context) : PageModel
    {
        /// <summary>
        /// Gets or sets the submitted form data.
        /// </summary>
        [BindProperty]
        public RelationshipContent FormData { get; set; } = null!;

        /// <summary>
        /// Submits the new relationship item form.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync()
        {
            int? highestOrderValue = await context.RelationshipItems
                .OrderByDescending(content => content.Order)
                .Select(content => content.Order)
                .FirstOrDefaultAsync();

            highestOrderValue = highestOrderValue ?? 0;

            RelationshipContent newItem = FormData with
            {
                Id = Guid.NewGuid(),
                Order = highestOrderValue.Value + 1,
            };

            _ = context.RelationshipItems
                .Add(newItem);

            _ = await context
                .SaveChangesAsync();

            return this.RedirectToPage("/Relationship/Index", new { Area = "CMS" });

        }
    }
}
