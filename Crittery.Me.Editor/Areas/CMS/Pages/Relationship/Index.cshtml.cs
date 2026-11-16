// <copyright file="Index.cshtml.cs" company="Jack Kelly">
// Copyright © 2026 Jack Kelly.
//
// The crittery.me website is free software: you can redistribute it and/or modify it
// under the terms of the GNU General Public License as published by the
// Free Software Foundation, either version 3 of the License, or
// (at your option)any later version.
//
// The crittery.me source code is distributed in the hope that it will be useful, but
// WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
// or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for
// more details.
//
// You should have received a copy of the GNU General Public License along with
// The crittery.me source code. If not, see [https://www.gnu.org/licenses/].
// </copyright>

namespace Crittery.Me.Editor.Areas.CMS.Pages.Relationship
{
    using Crittery.Me.Data;
    using Crittery.Me.Data.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Page model for the relationship listing.
    /// </summary>
    public class IndexModel(CritteryMeContext context) : PageModel
    {
        /// <summary>
        /// Gets or sets the relationships to list.
        /// </summary>
        public List<RelationshipContent> Relationships { get; set; } = [];

        /// <summary>
        /// Load the relationship items.
        /// </summary>
        /// <returns>
        /// A <see cref="Task"/> representing the duration of the request.
        /// Results in a page view.
        /// </returns>
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
        /// <returns>
        /// A <see cref="Task"/> representing the duration of the request.
        /// Results in a page redirect.
        /// </returns>
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
        /// <returns>
        /// A <see cref="Task"/> representing the duration of the request.
        /// Results in a page redirect.
        /// </returns>
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
                for (int i = 0; i < relationships.Count; i++)
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
        /// <returns>
        /// A <see cref="Task"/> representing the duration of the request.
        /// Results in a page redirect.
        /// </returns>
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
