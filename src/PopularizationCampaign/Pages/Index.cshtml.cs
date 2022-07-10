using Data;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PopularizationCampaign.ViewModels.Index;

namespace PopularizationCampaign.Pages
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext appDbContext;

        public IndexModel(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        [BindProperty]
        public ContactFormInputModel Input { get; set; }

        public IActionResult OnGet()
        {
            if (HasVoted())
            {
                return this.LocalRedirectPermanent("/Thanks");
            }

            return this.Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (this.ModelState.ErrorCount > 0)
            {
                return this.Page();
            }

            if (!HasVoted())
            {
                var userType = this.Input.UserType.ToString();
                var userExists = await this.appDbContext
                    .Users
                    .AsNoTracking()
                    .Where(u => u.Email == this.Input.Email)
                    .Where(u => u.Type == userType)
                    .AnyAsync();

                if (!userExists)
                {
                    var user = new User
                    {
                        Email = this.Input.Email,
                        Type = this.Input.UserType.ToString(),
                    };

                    await this.appDbContext.Users.AddAsync(user);
                    await this.appDbContext.SaveChangesAsync();
                }
            }

            this.HttpContext.Response.Cookies.Append("done", "True");
            return this.LocalRedirectPermanent("/Thanks");
        }

        public bool HasVoted()
        {
            var value = this.HttpContext.Request.Cookies["done"];
            return value == "True";
        }
    }
}