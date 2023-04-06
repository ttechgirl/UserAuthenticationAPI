using Domain.Extentions;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DbContext
{
    public class AppDbContext : IdentityDbContext<AppUsers, AppRoles, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public AppDbContext() { }
        DbSet<AppUsers> User{get; set;}
        DbSet<AppRoles> Role { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //builder.UseIdentityColumns();
            builder.Seed();
        }


    }

    public class MASFlightDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer("Server=SHAZYPC\\SQLEXPRESS;Database=UserRegistrationAPI;MultipleActiveResultSets=True;Trusted_Connection=True; ");
            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
