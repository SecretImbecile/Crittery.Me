// <copyright file="VisitorService.cs" company="Jack Kelly">
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

namespace Crittery.Me.Services.Implementations
{
    using Crittery.Me.Data;
    using Crittery.Me.Data.Models;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Service recording <see cref="Visitor"/> interactions.
    /// </summary>
    internal class VisitorService(CritteryMeContext context, ILogger<VisitorService> logger) : IVisitorService
    {
        /// <inheritdoc/>
        public async Task RecordVisitAsync(HttpRequest httpRequest, string pageRoute)
        {
            string? referrer = httpRequest.Headers.Referer
                .FirstOrDefault()
                ?? string.Empty;

            try
            {
                Visitor? existingVistor = await context.Visitors
                    .Where(visitor => EF.Functions.Like(visitor.Page, pageRoute))
                    .Where(visitor => EF.Functions.Like(visitor.Referrer, referrer))
                    .SingleOrDefaultAsync();

                if (existingVistor != null)
                {
                    existingVistor.Count += 1;
                    existingVistor.ModifiedOn = DateTime.Now;

                    _ = await context
                        .SaveChangesAsync();
                }
                else
                {
                    Visitor newVisitor = new Visitor()
                    {
                        Page = pageRoute,
                        Referrer = referrer,
                        Count = 1,
                        ModifiedOn = DateTime.Now,
                    };

                    _ = context.Visitors
                        .Add(newVisitor);
                    _ = await context
                        .SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Failed to log visitor");
            }
        }
    }
}
