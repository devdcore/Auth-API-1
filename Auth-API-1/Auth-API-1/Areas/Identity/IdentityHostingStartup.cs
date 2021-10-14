using System;
using Auth_API_1.Areas.Identity.Data;
using Auth_API_1.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(Auth_API_1.Areas.Identity.IdentityHostingStartup))]
namespace Auth_API_1.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<Auth_API_1Context>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("Auth_API_1ContextConnection")));

                services.AddDefaultIdentity<Auth_API_1User>(options => options.SignIn.RequireConfirmedAccount = false)
                    .AddEntityFrameworkStores<Auth_API_1Context>();
            });
        }
    }
}