// <copyright file="CalendarEventType.cs" company="Jack Kelly">
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

namespace Crittery.Me.Data.Enums
{
    /// <summary>
    /// Valid types for calendar entries.
    /// </summary>
    public enum CalendarEventType
    {
        /// <summary>
        /// A regular meet.
        /// </summary>
        Meet = 0,

        /// <summary>
        /// A convention or larger event.
        /// </summary>
        Convention,
    }
}
