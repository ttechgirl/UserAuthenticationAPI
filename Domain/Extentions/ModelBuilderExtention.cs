using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Extentions
{
    public static class ModelBuilderExtention
    {
        public static PasswordHasher<AppUsers> Hasher { get; set; } = new PasswordHasher<AppUsers>();

        public static void Seed(this ModelBuilder builder)
        {

            builder.Entity<AppUsers>().HasData(
            new AppUsers
            {
                Id = Guid.NewGuid(),
                FirstName = "Akeem",
                LastName = "Mustapha",
                PhoneNumber = "08055423378",
                Email = "akeem234@gmail.com",
                PasswordHash = Hasher.HashPassword(null, "AHakeem1%")

            });

            builder.Entity<AppRoles>().HasData(
            new AppRoles
            {
                Id = Guid.Parse("69D545C8-B65E-4FFF-82AC-BCE73AC289A3"),
                Name = "User",
                NormalizedName = "User",
                ConcurrencyStamp ="1"

            });

            builder.Entity<AppRoles>().HasData(
           new AppRoles
           {
               Id = Guid.Parse("DF718532-FD67-45B0-8C70-0659331F1FAB"),
               Name = "Admin",
               NormalizedName = "Admin",
               ConcurrencyStamp = "2"

           });


        }

    }
}

