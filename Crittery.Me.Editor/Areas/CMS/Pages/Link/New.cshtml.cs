namespace Crittery.Me.Editor.Areas.CMS.Pages.Link
{
    using Crittery.Me.Data;
    using Crittery.Me.Data.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;


    /// <summary>
    /// Page model for the new bio item form.
    /// </summary>
    public class NewModel(CritteryMeContext context) : PageModel
    {
        /// <summary>
        /// Gets or sets the submitted form data.
        /// </summary>
        [BindProperty]
        public Link FormData { get; set; } = null!;

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
                            MediaType = "image/png",
                        },
                        new()
                        {
                            Source = string.Empty,
                            MediaType = "image/webp",
                        }
                    ],
                    Alt = string.Empty,
                },
            };
        }

        /// <summary>
        /// Submits the new bio item form.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync()
        {
            int? highestOrderValue = await context.Links
                .OrderByDescending(link => link.Order)
                .Select(link => link.Order)
                .FirstOrDefaultAsync();

            highestOrderValue = highestOrderValue ?? 0;

            Link newItem = FormData with
            {
                Id = Guid.NewGuid(),
                Order = highestOrderValue.Value + 1,
                Icon = FormData.Icon with
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
