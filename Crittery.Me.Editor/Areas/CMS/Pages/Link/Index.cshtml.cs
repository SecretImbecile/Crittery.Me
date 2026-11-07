namespace Crittery.Me.Editor.Areas.CMS.Pages.Link
{
    using Crittery.Me.Data;
    using Crittery.Me.Data.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Page model for the links listing.
    /// </summary>
    public class IndexModel(CritteryMeContext context) : PageModel
    {
        public List<Link> Links { get; set; } = [];

        /// <summary>
        /// Load the links.
        /// </summary>
        public async Task OnGetAsync()
        {
            this.Links = await context.Links
                .OrderBy(bioItem => bioItem.Order)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Delete a link.
        /// </summary>
        /// <param name="id">ID of the link to delete.</param>
        public async Task<IActionResult> OnGetDeleteAsync(Guid id)
        {
            int count = await context.Links
                .Where(link => link.Id == id)
                .ExecuteDeleteAsync();

            if (count > 0)
            {
                return this
                    .RedirectToPage("/Link/Index", new { Area = "CMS" });
            }
            else
            {
                return this
                    .NotFound();
            }
        }

        /// <summary>
        /// Shift the order of a link upwards.
        /// </summary>
        /// <param name="id">ID of the link to shift.</param>
        public async Task<IActionResult> OnGetOrderUpAsync(Guid id)
        {
            List<Link> links = await context.Links
                .OrderBy(link => link.Order)
                .ToListAsync();

            if (links.Count <= 1)
            {
                // No other items to reorder
                goto complete;
            }

            int lastIndex = links.Count - 1;
            int selectedIndex = links.FindIndex(link => link.Id == id);

            if (selectedIndex > 0)
            {
                // Swap order in list
                Link selected = links[selectedIndex];
                Link swap = links[selectedIndex - 1];

                links[selectedIndex] = swap;
                links[selectedIndex - 1] = selected;

                // Renumber items in list
                for(int i = 0; i < links.Count; i++)
                {
                    links[i].Order = i + 1;
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
                .RedirectToPage("/Link/Index", new { Area = "CMS" });
        }

        /// <summary>
        /// Shift the order of a link downwards.
        /// </summary>
        /// <param name="id">ID of the link to shift.</param>
        public async Task<IActionResult> OnGetOrderDownAsync(Guid id)
        {
            List<Link> links = await context.Links
                .OrderBy(link => link.Order)
                .ToListAsync();

            if (links.Count <= 1)
            {
                // No other items to reorder
                goto complete;
            }

            int lastIndex = links.Count - 1;
            int selectedIndex = links.FindIndex(link => link.Id == id);

            if (selectedIndex >= lastIndex)
            {
                // Already at end of list
                goto complete;
            }
            else if (selectedIndex >= 0)
            {
                // Swap order in list
                Link selected = links[selectedIndex];
                Link swap = links[selectedIndex + 1];

                links[selectedIndex] = swap;
                links[selectedIndex + 1] = selected;

                // Renumber items in list
                for (int i = 0; i < links.Count; i++)
                {
                    links[i].Order = i + 1;
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
                .RedirectToPage("/Link/Index", new { Area = "CMS" });
        }
    }
}
