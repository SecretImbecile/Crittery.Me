namespace Crittery.Me.Editor.Areas.CMS.Pages.Relationship
{
    using System.Net.WebSockets;
    using Crittery.Me.Data;
    using Crittery.Me.Data.Models;
    using Microsoft.AspNetCore.Http.Metadata;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Page model for the relationship listing.
    /// </summary>
    public class IndexModel(CritteryMeContext context) : PageModel
    {
        public List<RelationshipContent> Relationships { get; set; } = [];

        /// <summary>
        /// Load the relationship items.
        /// </summary>
        public async Task OnGetAsync()
        {
            this.Relationships = await context.RelationshipItems
                .OrderBy(content => content.Order)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Delete a relationship item.
        /// </summary>
        /// <param name="id">ID of the relationship item to delete.</param>
        public async Task<IActionResult> OnGetDeleteAsync(Guid id)
        {
            int count = await context.RelationshipItems
                .Where(content => content.Id == id)
                .ExecuteDeleteAsync();

            if (count > 0)
            {
                return this
                    .RedirectToPage("/Relationship/Index", new { Area = "CMS" });
            }
            else
            {
                return this
                    .NotFound();
            }
        }

        /// <summary>
        /// Shift the order of a relationship item upwards.
        /// </summary>
        /// <param name="id">ID of the relationship item to shift.</param>
        public async Task<IActionResult> OnGetOrderUpAsync(Guid id)
        {
            List<RelationshipContent> relationships = await context.RelationshipItems
                .OrderBy(content => content.Order)
                .ToListAsync();

            if (relationships.Count <= 1)
            {
                // No other items to reorder
                goto complete;
            }

            int lastIndex = relationships.Count - 1;
            int selectedIndex = relationships.FindIndex(content => content.Id == id);

            if (selectedIndex > 0)
            {
                // Swap order in list
                RelationshipContent selected = relationships[selectedIndex];
                RelationshipContent swap = relationships[selectedIndex - 1];

                relationships[selectedIndex] = swap;
                relationships[selectedIndex - 1] = selected;

                // Renumber items in list
                for(int i = 0; i < relationships.Count; i++)
                {
                    relationships[i].Order = i + 1;
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
                .RedirectToPage("/Relationship/Index", new { Area = "CMS" });
        }

        /// <summary>
        /// Shift the order of a relationship item downwards.
        /// </summary>
        /// <param name="id">ID of the relationship item to shift.</param>
        public async Task<IActionResult> OnGetOrderDownAsync(Guid id)
        {
            List<RelationshipContent> relationships = await context.RelationshipItems
                .OrderBy(content => content.Order)
                .ToListAsync();

            if (relationships.Count <= 1)
            {
                // No other items to reorder
                goto complete;
            }

            int lastIndex = relationships.Count - 1;
            int selectedIndex = relationships.FindIndex(content => content.Id == id);

            if (selectedIndex >= lastIndex)
            {
                // Already at end of list
                goto complete;
            }
            else if (selectedIndex >= 0)
            {
                // Swap order in list
                RelationshipContent selected = relationships[selectedIndex];
                RelationshipContent swap = relationships[selectedIndex + 1];

                relationships[selectedIndex] = swap;
                relationships[selectedIndex + 1] = selected;

                // Renumber items in list
                for (int i = 0; i < relationships.Count; i++)
                {
                    relationships[i].Order = i + 1;
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
                .RedirectToPage("/Relationship/Index", new { Area = "CMS" });
        }
    }
}
