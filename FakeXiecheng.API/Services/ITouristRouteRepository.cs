using FakeXiecheng.API.Models;
using System;
using System.Collections.Generic;

namespace FakeXiecheng.API.Services
{
    public interface ITouristRouteRepository
    {
        IEnumerable<TouristRoute> GetAllTouristRoutes();
        TouristRoute GetTouristRoute(Guid routeId);
        void AddTouristRoute(TouristRoute route);
        void UpdateTouristRoute(TouristRoute route);
        void DeleteCTouristRoute(TouristRoute route);
        bool Save();
    }
}
