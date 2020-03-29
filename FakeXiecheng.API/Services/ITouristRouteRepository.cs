using FakeXiecheng.API.Helpers;
using FakeXiecheng.API.Models;
using System;
using System.Collections.Generic;

namespace FakeXiecheng.API.Services
{
    public interface ITouristRouteRepository
    {
        bool TouristRouteExists(Guid touristRouteId);
        PaginationList<TouristRoute> GetTouristRoutes(TouristRouteFilterParameters filterParameters);
        TouristRoute GetTouristRoute(Guid routeId);
        void AddTouristRoute(TouristRoute route);
        void UpdateTouristRoute(TouristRoute route);
        void DeleteCTouristRoute(TouristRoute route);
        IEnumerable<TouristRoutePicture> GetPicturesByTouristRouteId(Guid touristRouteId);
        TouristRoutePicture GetPicturesByTouristRouteIdAndPictureId(Guid touristRouteId, int pictureId);
        void AddTouristRoutePicture(Guid touristRouteId, TouristRoutePicture picture);
        bool Save();
    }
}
