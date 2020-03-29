﻿using FakeXiecheng.API.DbContexts;
using FakeXiecheng.API.Helpers;
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

        public IEnumerable<TouristRoute> GetTouristRoutes(TouristRouteFilterParameters filterParameters)
        {
            IQueryable<TouristRoute> result = _context.TouristRoutes.Include(t => t.TouristRoutePictures);
            // filter keyword
            if (!string.IsNullOrEmpty(filterParameters.Keyword))
            {
                result = result.Where(c => c.Title.Contains(filterParameters.Keyword.Trim()));
            }
            // filter rating
            if (filterParameters.RatingValue != 0)
            {
                //switch (filterParameters.RatingOperator)
                //{
                //    case OperatorType.equal:
                //        result = result.Where(c => c.Rating == filterParameters.RatingValue);
                //        break;
                //    case OperatorType.lessThan:
                //        result = result.Where(c => c.Rating <= filterParameters.RatingValue);
                //        break;
                //    case OperatorType.largerThan:
                //    default:
                //        result = result.Where(c => c.Rating >= filterParameters.RatingValue);
                //        break;
                //}
                result = filterParameters.RatingOperator switch
                {
                    OperatorType.equal => result.Where(c => c.Rating == filterParameters.RatingValue),
                    OperatorType.lessThan => result.Where(c => c.Rating <= filterParameters.RatingValue),
                    _ => result.Where(c => c.Rating >= filterParameters.RatingValue),
                };
            }

            // pagination
            var count = result.Count();
            var skip = (filterParameters.PageNumber - 1) * filterParameters.PageSize;
            result = result.Skip(skip);
            var display = Math.Min(count - skip, filterParameters.PageSize);
            if (display > 0)
            {
                result = result.Take(display);
            }

            return result.ToList();
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
