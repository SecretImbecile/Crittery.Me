// <copyright file="Status.cshtml.cs" company="Jack Kelly">
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

namespace Crittery.Me.Pages
{
    using System.Net;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    /// <summary>
    /// Page model for the error page that handles HTTP status codes.
    /// </summary>
    public class StatusModel : PageModel
    {
        /// <summary>
        /// Gets the text description of the HTTP status code.
        /// </summary>
        public string StatusDescription { get; private set; } = string.Empty;

        /// <summary>
        /// HTTP Get Action for page.
        /// </summary>
        /// <param name="statusCode">The HTTP status code to display.</param>
        public void OnGet(int statusCode)
        {
            HttpStatusCode statusCodeParsed = (HttpStatusCode)statusCode;

            if ((Enum.IsDefined(statusCodeParsed) || statusCode == 418) && statusCodeParsed >= HttpStatusCode.BadRequest)
            {
                this.Response.StatusCode = statusCode;
                this.StatusDescription = GetStatusDescription(statusCode);
            }
            else
            {
                this.Response.StatusCode = StatusCodes.Status404NotFound;
                this.StatusDescription = GetStatusDescription(404);
            }
        }

        /// <summary>
        /// Get a text description of common HTTP status codes.
        /// </summary>
        /// <param name="statusCode">The HTTP status code to describe.</param>
        /// <returns>A <see langword="string"/> text describing the HTTP status code.</returns>
        private static string GetStatusDescription(int statusCode)
        {
            return statusCode switch
            {
                400 => "Bad Request",
                403 => "Forbidden",
                404 => "Page Not Found",
                418 => "I'm a Teapot",
                500 => "An Unexpected Error Occured",
                >= 400 and < 500 => "An Unexpected Client Error Occurred",
                >= 500 and < 600 => "An Unexpected Server Error Occurred",
                _ => throw new IndexOutOfRangeException("Unexpected HTTP status code"),
            };
        }
    }
}
