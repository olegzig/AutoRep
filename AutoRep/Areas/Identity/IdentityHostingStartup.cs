using AutoRep.Data;
using AutoRep.Models;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(AutoRep.Areas.Identity.IdentityHostingStartup))]

namespace AutoRep.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                services.AddDbContext<AuthContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("AuthContextConnection")));

                services.AddDefaultIdentity<SUser>(options =>
                {
                    options.User.AllowedUserNameCharacters = "$.@abcdefghijklmnopqrstuvwxyzабвгдеёжзийклмнопрстуфхцчшщъыьэюяАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯABCDEFGHIJKLMNOPQRSTUVWXYZ 1234567890-+=";
                    options.User.RequireUniqueEmail = true;//ну да
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                }).AddRoles<IdentityRole>().AddEntityFrameworkStores<AuthContext>().AddDefaultTokenProviders();
            });
        }
    }
}