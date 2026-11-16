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

namespace Crittery.Me.Editor.Areas.CMS.Pages.Link
{
    using Crittery.Me.Data;
    using Crittery.Me.Data.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Page model for the edit link form.
    /// </summary>
    public class EditModel(CritteryMeContext context) : PageModel
    {
        /// <summary>
        /// Gets or sets the submitted form data.
        /// </summary>
        [BindProperty]
        public Link FormData { get; set; } = null!;

        /// <summary>
        /// Loads the edit link form.
        /// </summary>
        /// <param name="id">The ID of the link to edit.</param>
        /// <returns>
        /// A <see cref="Task"/> representing the duration of the HTTP request.
        /// Results in a page view.
        /// </returns>
        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            Link? dbItem = await context.Links
                .AsNoTracking()
                .SingleOrDefaultAsync(link => link.Id == id);

            if (dbItem != null)
            {
                // Ensure files are in the same order as the form
                dbItem.Icon.Files = dbItem.Icon.Files
                    .OrderByDescending(file => file.MediaType.EndsWith("webp", StringComparison.OrdinalIgnoreCase))
                    .ToList();

                this.FormData = dbItem;
                return this.Page();
            }
            else
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// Submits the edit link form.
        /// </summary>
        /// <param name="id">The ID of the link to edit.</param>
        /// <returns>
        /// A <see cref="Task"/> representing the duration of the HTTP request.
        /// Results in a page redirect.
        /// </returns>
        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            Link? dbItem = await context.Links
                .SingleOrDefaultAsync(link => link.Id == id);

            if (dbItem != null)
            {
                dbItem.Url = this.FormData.Url;
                dbItem.Text = this.FormData.Text;
                dbItem.Title = this.FormData.Title;
                dbItem.Visiblity = this.FormData.Visiblity;
                dbItem.Icon.Alt = string.Empty;
                dbItem.Icon.Files = this.FormData.Icon.Files;

                _ = await context
                .SaveChangesAsync();

                return this
                    .RedirectToPage("/Link/Index", new { Area = "CMS" });
            }
            else
            {
                return this
                    .NotFound();
            }
        }
    }
}
