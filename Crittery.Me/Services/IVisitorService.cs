// <copyright file="IVisitorService.cs" company="Jack Kelly">
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

namespace Crittery.Me.Services
{
    using Crittery.Me.Data.Models;

    /// <summary>
    /// Interface for services recording <see cref="Visitor"/> interactions.
    /// </summary>
    public interface IVisitorService
    {
        /// <summary>
        /// Records a visit to the current page.
        /// </summary>
        /// <param name="httpRequest">The current page request.</param>
        /// <param name="pageRoute">The full page route.</param>
        /// <returns>A <see cref="Task"/> Representing the duration of the database operations.</returns>
        public Task RecordVisitAsync(HttpRequest httpRequest, string pageRoute);
    }
}
