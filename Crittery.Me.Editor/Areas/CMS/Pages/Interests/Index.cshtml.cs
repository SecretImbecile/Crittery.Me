using Crittery.Me.Data;
using Crittery.Me.Data.Models;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Crittery.Me.Editor.Areas.CMS.Pages.Interests
{
    /// <summary>
    /// Page model for the interests listing.
    /// </summary>
    public class IndexModel(CritteryMeContext context) : PageModel
    {
        public List<Interest> Interests { get; set; } = [];

        /// <summary>
        /// Load the interest list.
        /// </summary>
        public async Task OnGetAsync()
        {
            this.Interests = await context.Interests
                .OrderBy(interest => interest.Order)
                .ToListAsync();
        }

        /// <summary>
        /// Delete an interest.
        /// </summary>
        /// <param name="id">ID of the interest to delete.</param>
        public async Task<IActionResult> OnGetDeleteAsync(Guid id)
        {
            int count = await context.Interests
                .Where(interest => interest.Id == id)
                .ExecuteDeleteAsync();

            if (count > 0)
            {
                return this
                    .RedirectToPage("/Interests/Index", new { Area = "CMS" });
            }
            else
            {
                return this
                    .NotFound();
            }
        }
    }
}
