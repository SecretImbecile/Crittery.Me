using Crittery.Me.Data;
using Crittery.Me.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Crittery.Me.Editor.Areas.CMS.Pages.Bio
{
    /// <summary>
    /// Page model for the edit bio item form.
    /// </summary>
    public class EditModel(CritteryMeContext context) : PageModel
    {
        /// <summary>
        /// Gets or sets the submitted form data.
        /// </summary>
        [BindProperty]
        public BioContent FormData { get; set; } = null!;

        /// <summary>
        /// Loads the edit bio item form.
        /// </summary>
        /// <param name="id">The ID of the bio item to edit.</param>
        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            BioContent? dbItem = await context.BioItems
                .AsNoTracking()
                .SingleOrDefaultAsync(bioItem => bioItem.Id == id);

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
        /// Submits the new interest form.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            BioContent? dbItem = await context.BioItems
                .SingleOrDefaultAsync(bioItem => bioItem.Id == id);

            if (dbItem != null)
            {
                dbItem.Text = this.FormData.Text;

                _ = await context
                .SaveChangesAsync();

                return this
                    .RedirectToPage("/Bio/Index", new { Area = "CMS" });
            }
            else
            {
                return this
                    .NotFound("");
            }
        }
    }
}
