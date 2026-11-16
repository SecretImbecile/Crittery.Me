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

namespace Crittery.Me.Editor.Areas.CMS.Pages.Link
{
    using System.Net.Mime;
    using Crittery.Me.Data;
    using Crittery.Me.Data.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Page model for the new link form.
    /// </summary>
    public class NewModel(CritteryMeContext context) : PageModel
    {
        /// <summary>
        /// Gets or sets the submitted form data.
        /// </summary>
        [BindProperty]
        public Link FormData { get; set; } = null!;

        /// <summary>
        /// Loads the new link form.
        /// </summary>
        public void OnGet()
        {
            this.FormData = new()
            {
                Order = default,
                Url = string.Empty,
                Text = string.Empty,
                Visiblity = Data.Enums.LinkVisiblity.OnAllDays,
                Icon = new()
                {
                    Files =
                    [
                        new()
                        {
                            Source = string.Empty,
                            MediaType = MediaTypeNames.Image.Png,
                        },
                        new()
                        {
                            Source = string.Empty,
                            MediaType = MediaTypeNames.Image.Webp,
                        },
                    ],
                    Alt = string.Empty,
                },
            };
        }

        /// <summary>
        /// Submits the new bio item form.
        /// </summary>
        /// <returns>
        /// A <see cref="Task"/> representing the duration of the HTTP request.
        /// Results in a page redirect.
        /// </returns>
        public async Task<IActionResult> OnPostAsync()
        {
            int? highestOrderValue = await context.Links
                .OrderByDescending(link => link.Order)
                .Select(link => link.Order)
                .FirstOrDefaultAsync();

            highestOrderValue = highestOrderValue ?? 0;

            Link newItem = this.FormData with
            {
                Id = Guid.NewGuid(),
                Order = highestOrderValue.Value + 1,
                Icon = this.FormData.Icon with
                {
                    Alt = string.Empty,
                },
            };

            _ = context.Links
                .Add(newItem);

            _ = await context
                .SaveChangesAsync();

            return this.RedirectToPage("/Link/Index", new { Area = "CMS" });
        }
    }
}
