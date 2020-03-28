using FakeXiecheng.API.DbContexts;
using FakeXiecheng.API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FakeXiecheng.API.Services
{
    public class TouristRouteRepository : ITouristRouteRepository, IDisposable
    {
        private readonly TouristLibraryContext _context;

        public TouristRouteRepository(TouristLibraryContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public bool TouristRouteExists(Guid touristRouteId)
        {
            if (touristRouteId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(touristRouteId));
            }
            return _context.TouristRoutes.Any(t => t.Id == touristRouteId);
        }

        public IEnumerable<TouristRoute> GetAllTouristRoutes()
        {
            return _context.TouristRoutes
                .Include(t => t.TouristRoutePictures)
                .OrderBy(t => t.Title).ToList();
        }

        public TouristRoute GetTouristRoute(Guid routeId)
        {
            if (routeId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(routeId));
            }
            return _context.TouristRoutes
                .Include(t => t.TouristRoutePictures)
                .Where(c => c.Id == routeId)
                .FirstOrDefault();
        }

        public void AddTouristRoute(TouristRoute route)
        {
            if (route == null)
            {
                throw new ArgumentNullException(nameof(route));
            }
            _context.TouristRoutes.Add(route);
        }

        public void UpdateTouristRoute(TouristRoute route)
        {
            // no code in this implementation
        }

        public void DeleteCTouristRoute(TouristRoute route)
        {
            _context.TouristRoutes.Remove(route);
        }

        public IEnumerable<TouristRoutePicture> GetPicturesByTouristRouteId(Guid touristRouteId)
        {
            return _context.TouristRoutePictures
                .Where(p => p.TouristRouteId == touristRouteId)
                .ToList();
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose resources when needed
            }
        }
    }
}
