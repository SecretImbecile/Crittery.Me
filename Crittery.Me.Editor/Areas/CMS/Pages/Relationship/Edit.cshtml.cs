// <copyright file="Edit.cshtml.cs" company="Jack Kelly">
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
    /// Page model for the edit relationship item form.
    /// </summary>
    public class EditModel(CritteryMeContext context) : PageModel
    {
        /// <summary>
        /// Gets or sets the submitted form data.
        /// </summary>
        [BindProperty]
        public RelationshipContent FormData { get; set; } = null!;

        /// <summary>
        /// Loads the edit relationship item form.
        /// </summary>
        /// <param name="id">The ID of the bio item to edit.</param>
        /// <returns>
        /// A <see cref="Task"/> representing the duration of the HTTP request.
        /// Results in a page view.
        /// </returns>
        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            RelationshipContent? dbItem = await context.RelationshipItems
                .AsNoTracking()
                .SingleOrDefaultAsync(content => content.Id == id);

            if (dbItem != null)
            {
                this.FormData = dbItem;

                return this
                    .Page();
            }
            else
            {
                return this
                    .NotFound();
            }
        }

        /// <summary>
        /// Submits the edit relationship item form.
        /// </summary>
        /// <returns>
        /// A <see cref="Task"/> representing the duration of the HTTP request.
        /// Results in a page redirect.
        /// </returns>
        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            RelationshipContent? dbItem = await context.RelationshipItems
                .SingleOrDefaultAsync(content => content.Id == id);

            if (dbItem != null)
            {
                dbItem.Text = this.FormData.Text;

                _ = await context
                .SaveChangesAsync();

                return this
                    .RedirectToPage("/Relationship/Index", new { Area = "CMS" });
            }
            else
            {
                return this
                    .NotFound();
            }
        }
    }
}
