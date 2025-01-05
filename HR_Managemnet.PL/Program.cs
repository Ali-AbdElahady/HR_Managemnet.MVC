using HR_Management.DAL.Context;
using HR_Management.DAL.Entities;
using HR_Managemnet.PL.ErrorsHandle;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.DbIntializer;
using Services.Extensions;
using Talabat.APIs.Middlewares;

namespace HR_Managemnet.PL
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddApplicationServices();
            builder.Services.AddIdentityServices(builder.Configuration);

            builder.Services.AddRazorPages();

            var app = builder.Build();

            #region Seeding Data and  Update Migration

            // Ask CLR for Creating Object From DbContext Explicitly
            using var Scope = app.Services.CreateScope();
            var Services = Scope.ServiceProvider;
            var LoggerFactory = Services.GetRequiredService<ILoggerFactory>();

            try
            {
                var dbContext = Services.GetRequiredService<HRSystemDbContext>();
                await dbContext.Database.MigrateAsync();

                await HRSystemSeed.SeedAsync(dbContext);

                var dbInitial = Services.GetRequiredService<IUserSeed>();
                await dbInitial.IntializeUser();
            }
            catch (Exception ex)
            {
                var Logger = LoggerFactory.CreateLogger<Program>();
                Logger.LogError(ex, "An Error Occured During Appling The Migration");
            }

            #endregion

            app.UseMiddleware<ExceptionMiddleWare>();
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();

            app.UseAuthorization();
            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/" && !context.User.Identity.IsAuthenticated)
                {
                    context.Response.Redirect("/Identity/Account/Login");
                }
                else
                {
                    await next();
                }
            });

            app.MapRazorPages();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}