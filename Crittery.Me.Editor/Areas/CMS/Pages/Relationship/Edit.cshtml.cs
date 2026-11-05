namespace Crittery.Me.Editor.Areas.CMS.Pages.Relationship
{
    using Crittery.Me.Data;
    using Crittery.Me.Data.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Page model for the edit relationship item form.
    /// </summary>
    public class EditModel(CritteryMeContext context) : PageModel
    {
        /// <summary>
        /// Gets or sets the submitted form data.
        /// </summary>
        [BindProperty]
        public RelationshipContent FormData { get; set; } = null!;

        /// <summary>
        /// Loads the edit relationship item form.
        /// </summary>
        /// <param name="id">The ID of the bio item to edit.</param>
        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            RelationshipContent? dbItem = await context.RelationshipItems
                .AsNoTracking()
                .SingleOrDefaultAsync(content => content.Id == id);

            if (dbItem != null)
            {
                this.FormData = dbItem;
                
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
        /// Submits the edit relationship item form.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            RelationshipContent? dbItem = await context.RelationshipItems
                .SingleOrDefaultAsync(content => content.Id == id);

            if (dbItem != null)
            {
                dbItem.Text = this.FormData.Text;

                _ = await context
                .SaveChangesAsync();

                return this
                    .RedirectToPage("/Relationship/Index", new { Area = "CMS" });
            }
            else
            {
                return this
                    .NotFound("");
            }
        }
    }
}
