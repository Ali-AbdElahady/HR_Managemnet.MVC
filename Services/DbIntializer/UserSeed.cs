using HR_Management.DAL.Context;
using HR_Management.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DbIntializer
{
    public class UserSeed : IUserSeed
    {
        private readonly HRSystemDbContext hrDbContext;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public UserSeed(HRSystemDbContext _hrDbContext, UserManager<ApplicationUser> _userManager, RoleManager<IdentityRole> _roleManager)
        {
            hrDbContext = _hrDbContext;
            userManager = _userManager;
            roleManager = _roleManager;
        }
        public async Task IntializeUser()
        {
            if(!await roleManager.RoleExistsAsync(WebSiteRoles.WebSite_Admin))
            {
                await roleManager.CreateAsync(new IdentityRole(WebSiteRoles.WebSite_Admin));

                var userCreated = await userManager.CreateAsync(
                    new ApplicationUser()
                    {
                        UserName = WebSiteRoles.WebSite_Admin
                    },"Admin1");
                var AdminUser = await hrDbContext.ApplicationUsers.FirstOrDefaultAsync(U=>U.UserName == WebSiteRoles.WebSite_Admin);
                if(AdminUser != null)
                {
                    await userManager.AddToRoleAsync(AdminUser, WebSiteRoles.WebSite_Admin);
                }
            }
        }
    }
}
