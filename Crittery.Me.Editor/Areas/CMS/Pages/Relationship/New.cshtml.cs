// <copyright file="New.cshtml.cs" company="Jack Kelly">
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
    /// Page model for the new relationship item form.
    /// </summary>
    public class NewModel(CritteryMeContext context) : PageModel
    {
        /// <summary>
        /// Gets or sets the submitted form data.
        /// </summary>
        [BindProperty]
        public RelationshipContent FormData { get; set; } = null!;

        /// <summary>
        /// Submits the new relationship item form.
        /// </summary>
        /// <returns>
        /// A <see cref="Task"/> representing the duration of the request.
        /// Results in a page redirect.
        /// </returns>
        public async Task<IActionResult> OnPostAsync()
        {
            int? highestOrderValue = await context.RelationshipItems
                .OrderByDescending(content => content.Order)
                .Select(content => content.Order)
                .FirstOrDefaultAsync();

            highestOrderValue = highestOrderValue ?? 0;

            RelationshipContent newItem = this.FormData with
            {
                Id = Guid.NewGuid(),
                Order = highestOrderValue.Value + 1,
            };

            _ = context.RelationshipItems
                .Add(newItem);

            _ = await context
                .SaveChangesAsync();

            return this.RedirectToPage("/Relationship/Index", new { Area = "CMS" });
        }
    }
}
