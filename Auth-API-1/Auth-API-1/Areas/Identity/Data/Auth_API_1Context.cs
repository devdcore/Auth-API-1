using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auth_API_1.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Auth_API_1.Data
{
    public class Auth_API_1Context : IdentityDbContext<Auth_API_1User>
    {
        public Auth_API_1Context(DbContextOptions<Auth_API_1Context> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
