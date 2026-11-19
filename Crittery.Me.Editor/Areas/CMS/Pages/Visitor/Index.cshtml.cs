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

namespace Crittery.Me.Editor.Areas.CMS.Pages.Visitor
{
    using Crittery.Me.Data;
    using Crittery.Me.Data.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Page model for the visitor listing.
    /// </summary>
    public class IndexModel(CritteryMeContext context) : PageModel
    {
        /// <summary>
        /// Gets or sets the visitors to list.
        /// </summary>
        public List<Visitor> Visitors { get; set; } = [];

        /// <summary>
        /// Gets the visitors to list, grouped by page.
        /// </summary>
        public IEnumerable<IGrouping<string, Visitor>> VisitorsByPage => this.Visitors
            .GroupBy(visitor => visitor.Page);

        /// <summary>
        /// Gets the total visitor count.
        /// </summary>
        public int TotalVisitors => this.Visitors
            .Sum(visitor => visitor.Count);

        /// <summary>
        /// Load the interest list.
        /// </summary>
        /// <returns>
        /// A <see cref="Task"/> representing the duration of the HTTP request.
        /// </returns>
        public async Task OnGetAsync()
        {
            this.Visitors = await context.Visitors
                .OrderBy(visitor => visitor.Page)
                .ThenBy(visitor => visitor.Referrer)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}