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

namespace Crittery.Me.Editor.Areas.CMS.Pages.Interests
{
    using Crittery.Me.Data;
    using Crittery.Me.Data.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Page model for the interests listing.
    /// </summary>
    public class IndexModel(CritteryMeContext context) : PageModel
    {
        /// <summary>
        /// Gets or sets the interest to list.
        /// </summary>
        public List<InterestContent> Interests { get; set; } = [];

        /// <summary>
        /// Load the interest list.
        /// </summary>
        /// <returns>
        /// A <see cref="Task"/> representing the duration of the HTTP request.
        /// </returns>
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
        /// <returns>
        /// A <see cref="Task"/> representing the duration of the HTTP request.
        /// Results in a page redirect.
        /// </returns>
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

        /// <summary>
        /// Shift the order of a interest upwards.
        /// </summary>
        /// <param name="id">ID of the interest to shift.</param>
        /// <returns>
        /// A <see cref="Task"/> representing the duration of the HTTP request.
        /// Results in a page redirect.
        /// </returns>
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
                for (int i = 0; i < interests.Count; i++)
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

        /// <summary>
        /// Shift the order of a interest downwards.
        /// </summary>
        /// <param name="id">ID of the interest to shift.</param>
        /// <returns>
        /// A <see cref="Task"/> representing the duration of the HTTP request.
        /// Results in a page redirect.
        /// </returns>
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
