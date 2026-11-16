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

namespace Crittery.Me.Editor.Areas.CMS.Pages.Bio
{
    using Crittery.Me.Data;
    using Crittery.Me.Data.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Page model for the bio listing.
    /// </summary>
    public class IndexModel(CritteryMeContext context) : PageModel
    {
        /// <summary>
        /// Gets or sets the bio items being listed.
        /// </summary>
        public List<BioContent> BioItems { get; set; } = [];

        /// <summary>
        /// Load the bio items.
        /// </summary>
        /// <returns>
        /// A <see cref="Task"/> representing the duration of the HTTP request.
        /// </returns>
        public async Task OnGetAsync()
        {
            this.BioItems = await context.BioItems
                .OrderBy(bioItem => bioItem.Order)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Delete a bio item.
        /// </summary>
        /// <param name="id">ID of the bio item to delete.</param>
        /// <returns>
        /// A <see cref="Task"/> representing the duration of the HTTP request.
        /// Results in a page redirect.
        /// </returns>
        public async Task<IActionResult> OnGetDeleteAsync(Guid id)
        {
            int count = await context.BioItems
                .Where(bioItem => bioItem.Id == id)
                .ExecuteDeleteAsync();

            if (count > 0)
            {
                return this
                    .RedirectToPage("/Bio/Index", new { Area = "CMS" });
            }
            else
            {
                return this
                    .NotFound();
            }
        }

        /// <summary>
        /// Shift the order of a bio item upwards.
        /// </summary>
        /// <param name="id">ID of the bio item to shift.</param>
        /// <returns>
        /// A <see cref="Task"/> representing the duration of the HTTP request.
        /// Results in a page redirect.
        /// </returns>
        public async Task<IActionResult> OnGetOrderUpAsync(Guid id)
        {
            List<BioContent> bioItems = await context.BioItems
                .OrderBy(bioItem => bioItem.Order)
                .ToListAsync();

            if (bioItems.Count <= 1)
            {
                // No other items to reorder
                goto complete;
            }

            int lastIndex = bioItems.Count - 1;
            int selectedIndex = bioItems.FindIndex(bioItem => bioItem.Id == id);

            if (selectedIndex > 0)
            {
                // Swap order in list
                BioContent selected = bioItems[selectedIndex];
                BioContent swap = bioItems[selectedIndex - 1];

                bioItems[selectedIndex] = swap;
                bioItems[selectedIndex - 1] = selected;

                // Renumber items in list
                for (int i = 0; i < bioItems.Count; i++)
                {
                    bioItems[i].Order = i + 1;
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
                .RedirectToPage("/Bio/Index", new { Area = "CMS" });
        }

        /// <summary>
        /// Shift the order of a bio item downwards.
        /// </summary>
        /// <param name="id">ID of the bio item to shift.</param>
        /// <returns>
        /// A <see cref="Task"/> representing the duration of the HTTP request.
        /// Results in a page redirect.
        /// </returns>
        public async Task<IActionResult> OnGetOrderDownAsync(Guid id)
        {
            List<BioContent> bioItems = await context.BioItems
                .OrderBy(bioItem => bioItem.Order)
                .ToListAsync();

            if (bioItems.Count <= 1)
            {
                // No other items to reorder
                goto complete;
            }

            int lastIndex = bioItems.Count - 1;
            int selectedIndex = bioItems.FindIndex(bioItem => bioItem.Id == id);

            if (selectedIndex >= lastIndex)
            {
                // Already at end of list
                goto complete;
            }
            else if (selectedIndex >= 0)
            {
                // Swap order in list
                BioContent selected = bioItems[selectedIndex];
                BioContent swap = bioItems[selectedIndex + 1];

                bioItems[selectedIndex] = swap;
                bioItems[selectedIndex + 1] = selected;

                // Renumber items in list
                for (int i = 0; i < bioItems.Count; i++)
                {
                    bioItems[i].Order = i + 1;
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
                .RedirectToPage("/Bio/Index", new { Area = "CMS" });
        }
    }
}
