using FakeXiecheng.API.DbContexts;
using FakeXiecheng.API.Dtos;
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
        private readonly IPropertyMappingService _propertyMappingService;

        public TouristRouteRepository(
            TouristLibraryContext context,
            IPropertyMappingService propertyMappingService
        )
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _propertyMappingService = propertyMappingService ?? throw new ArgumentNullException(nameof(propertyMappingService));
        }

        public bool TouristRouteExists(Guid touristRouteId)
        {
            if (touristRouteId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(touristRouteId));
            }
            return _context.TouristRoutes.Any(t => t.Id == touristRouteId);
        }

        public PaginationList<TouristRoute> GetTouristRoutes(TouristRouteFilterParameters filterParameters)
        {
            //IQueryable<TouristRoute> collectionBeforePaging =
            //    _context.TouristRoutes
            //    .Include(t => t.TouristRoutePictures);

            IQueryable<TouristRoute> collectionBeforePaging =_context.TouristRoutes
                .Include(t => t.TouristRoutePictures)
                .ApplySort(filterParameters.OrderBy, _propertyMappingService.GetPropertyMapping<TouristRouteDto, TouristRoute>());
            // filter keyword
            if (!string.IsNullOrEmpty(filterParameters.Keyword))
            {
                collectionBeforePaging = collectionBeforePaging.Where(c => c.Title.Contains(filterParameters.Keyword.Trim()));
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
                collectionBeforePaging = filterParameters.RatingOperator switch
                {
                    OperatorType.equal => collectionBeforePaging.Where(c => c.Rating == filterParameters.RatingValue),
                    OperatorType.lessThan => collectionBeforePaging.Where(c => c.Rating <= filterParameters.RatingValue),
                    _ => collectionBeforePaging.Where(c => c.Rating >= filterParameters.RatingValue),
                };
            }

            // pagination
            //var count = result.Count();
            //var skip = (filterParameters.PageNumber - 1) * filterParameters.PageSize;
            //result = result.Skip(skip);
            //var display = Math.Min(count - skip, filterParameters.PageSize);
            //if (display > 0)
            //{
            //    result = result.Take(display);
            //}
            //return result.ToList();
            return PaginationList<TouristRoute>.Create(
                    collectionBeforePaging,
                    filterParameters.PageNumber,
                    filterParameters.PageSize
                );
        }

        public IEnumerable<TouristRoute> GetTouristRoutesByIdList(IEnumerable<Guid> touristRouteIds)
        {
            if (touristRouteIds == null)
            {
                throw new ArgumentNullException(nameof(touristRouteIds));
            }
            return _context.TouristRoutes.Where(t => touristRouteIds.Contains(t.Id)).ToList();
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
            if (touristRouteId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(touristRouteId));
            }
            return _context.TouristRoutePictures
                .Where(p => p.TouristRouteId == touristRouteId)
                .ToList();
        }

        public TouristRoutePicture GetPicturesByTouristRouteIdAndPictureId(Guid touristRouteId, int pictureId)
        {
            if (touristRouteId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(touristRouteId));
            }
            return _context.TouristRoutePictures
                .Where(p => p.TouristRouteId == touristRouteId && p.Id == pictureId)
                .SingleOrDefault();
        }

        public void AddTouristRoutePicture(Guid touristRouteId, TouristRoutePicture picture)
        {
            if (touristRouteId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(touristRouteId));
            }
            if (picture == null)
            {
                throw new ArgumentNullException(nameof(picture));
            }
            picture.TouristRouteId = touristRouteId;
            _context.TouristRoutePictures.Add(picture);
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
