using FakeXiecheng.API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace FakeXiecheng.API.DbContexts
{
    public class TouristLibraryContext : IdentityDbContext
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

            base.OnModelCreating(modelBuilder);
        }
    }
}
