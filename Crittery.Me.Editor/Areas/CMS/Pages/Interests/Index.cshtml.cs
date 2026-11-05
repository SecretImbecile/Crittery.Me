using Crittery.Me.Data;
using Crittery.Me.Data.Models;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;

namespace Crittery.Me.Editor.Areas.CMS.Pages.Interests
{
    /// <summary>
    /// Page model for the interests listing.
    /// </summary>
    public class IndexModel(CritteryMeContext context) : PageModel
    {
        public List<InterestContent> Interests { get; set; } = [];

        /// <summary>
        /// Load the interest list.
        /// </summary>
        public async Task OnGetAsync()
        {
            this.Interests = await context.Interests
                .OrderBy(interest => interest.Order)
                .AsNoTracking()
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

        public async Task<IActionResult> OnGetOrderUpAsync(Guid id)
        {
            List<InterestContent> interests = await context.Interests
                .OrderBy(interest => interest.Order)
                .ToListAsync();

            if (interests.Count <= 1)
            {
                // No other items to reorder
                goto complete;
            }

            int lastIndex = interests.Count - 1;
            int selectedIndex = interests.FindIndex(interest => interest.Id == id);

            if (selectedIndex > 0)
            {
                // Swap order in list
                InterestContent selected = interests[selectedIndex];
                InterestContent swap = interests[selectedIndex - 1];

                interests[selectedIndex] = swap;
                interests[selectedIndex - 1] = selected;

                // Renumber items in list
                for(int i = 0; i < interests.Count; i++)
                {
                    interests[i].Order = i + 1;
                }

                _ = await context
                    .SaveChangesAsync();

                goto complete;
            }
            else if (selectedIndex == 0)
            {
                // Already at start of list
                goto complete;
            }
            else
            {
                // ID not found
                return this
                    .NotFound();
            }

        complete:
            return this
                .RedirectToPage("/Interests/Index", new { Area = "CMS" });
        }

        public async Task<IActionResult> OnGetOrderDownAsync(Guid id)
        {
            List<InterestContent> interests = await context.Interests
                .OrderBy(interest => interest.Order)
                .ToListAsync();

            if (interests.Count <= 1)
            {
                // No other items to reorder
                goto complete;
            }

            int lastIndex = interests.Count - 1;
            int selectedIndex = interests.FindIndex(interest => interest.Id == id);

            if (selectedIndex >= lastIndex)
            {
                // Already at end of list
                goto complete;
            }
            else if (selectedIndex >= 0)
            {
                // Swap order in list
                InterestContent selected = interests[selectedIndex];
                InterestContent swap = interests[selectedIndex + 1];

                interests[selectedIndex] = swap;
                interests[selectedIndex + 1] = selected;

                // Renumber items in list
                for (int i = 0; i < interests.Count; i++)
                {
                    interests[i].Order = i + 1;
                }

                _ = await context
                    .SaveChangesAsync();

                goto complete;
            }
            else
            {
                // ID not found
                return this
                    .NotFound();
            }

        complete:
            return this
                .RedirectToPage("/Interests/Index", new { Area = "CMS" });
        }
    }
}
