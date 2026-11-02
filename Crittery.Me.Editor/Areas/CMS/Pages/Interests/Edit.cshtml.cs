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
    public class EditModel(CritteryMeContext context) : PageModel
    {
        /// <summary>
        /// Gets or sets the submitted form data.
        /// </summary>
        [BindProperty]
        public Interest FormData { get; set; } = null!;

        /// <summary>
        /// Loads the edit interest form.
        /// </summary>
        /// <param name="id">The ID of the interest to edit.</param>
        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            Interest? dbInterest = await context.Interests
                .AsNoTracking()
                .SingleOrDefaultAsync(interest => interest.Id == id);

            if (dbInterest != null)
            {
                this.FormData = dbInterest;
                
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
            Interest? dbInterest = await context.Interests
                .SingleOrDefaultAsync(interest => interest.Id == id);

            if (dbInterest != null)
            {
                dbInterest.Emoji = this.FormData.Emoji;
                dbInterest.Text = this.FormData.Text;

                _ = await context
                .SaveChangesAsync();

                return this
                    .RedirectToPage("/Interests/Index", new { Area = "CMS" });
            }
            else
            {
                return this
                    .NotFound("");
            }
        }
    }
}
