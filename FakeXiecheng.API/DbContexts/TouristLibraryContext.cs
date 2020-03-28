using FakeXiecheng.API.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace FakeXiecheng.API.DbContexts
{
    public class TouristLibraryContext : DbContext
    {
        public TouristLibraryContext(DbContextOptions<TouristLibraryContext> options)
           : base(options)
        {
        }

        public DbSet<TouristRoute> TouristRoutes { get; set; }
        public DbSet<TouristRoutePicture> TouristRoutePictures { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
