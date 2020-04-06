using FakeXiecheng.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace FakeXiecheng.API.DbContexts
{
    public class TouristLibraryContext : IdentityDbContext<ApplicationUser>
    {
        public TouristLibraryContext(DbContextOptions<TouristLibraryContext> options)
           : base(options)
        {
        }

        public DbSet<TouristRoute> TouristRoutes { get; set; }
        public DbSet<TouristRoutePicture> TouristRoutePictures { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // seed the database with dummy data
            // seed tourist routes
            var touristRoutesJsonData = File.ReadAllText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"/DbContexts/touristRoutesMockData.json");
            IList<TouristRoute> touristRoutes = JsonConvert.DeserializeObject<IList<TouristRoute>>(touristRoutesJsonData);
            modelBuilder.Entity<TouristRoute>().HasData(touristRoutes);

            // seed tourist toutes images
            var touristRoutePicturesJsonData = File.ReadAllText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"/DbContexts/touristRoutePicturesMockData.json");
            IList<TouristRoutePicture> touristRoutePictures = JsonConvert.DeserializeObject<IList<TouristRoutePicture>>(touristRoutePicturesJsonData);
            modelBuilder.Entity<TouristRoutePicture>().HasData(touristRoutePictures);

            // update ApplicationUser's foreigner key to link with Roles 
            modelBuilder.Entity<ApplicationUser>(b => {
                b.HasMany(x => x.UserRoles).WithOne().HasForeignKey(ur => ur.UserId).IsRequired();
            });
           
            // add admin user 
            var adminUserId = "90184155-dee0-40c9-bb1e-b5ed07afc04e";
            var hasher = new PasswordHasher<ApplicationUser>();
            modelBuilder.Entity<ApplicationUser>().HasData(new ApplicationUser
            {
                Id = adminUserId,
                UserName = "admin@fakexiecheng.com",
                NormalizedUserName = "admin@fakexiecheng.com".ToUpper(),
                Email = "admin@fakexiecheng.com",
                NormalizedEmail = "admin@fakexiecheng.com".ToUpper(),
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "Fake123$"),
                SecurityStamp = string.Empty
            });

            // add system roles
            var AdminRoleId = "308660dc-ae51-480f-824d-7dca6714c3e2";
            var AuthorRoleId = "2aaf05a4-57ce-4a20-a997-06fe9f0d3809";
            var UserRoleId = "3dfa307e-d498-4ceb-a9c5-f0d55d103093";
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = AdminRoleId, Name = "Admin", NormalizedName = "Admin".ToUpper() },
                new IdentityRole { Id = AuthorRoleId, Name = "Author", NormalizedName = "Author".ToUpper() },
                new IdentityRole { Id = UserRoleId, Name = "User", NormalizedName = "User".ToUpper() }
            );

            // add user roles
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = AdminRoleId,
                UserId = adminUserId
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
