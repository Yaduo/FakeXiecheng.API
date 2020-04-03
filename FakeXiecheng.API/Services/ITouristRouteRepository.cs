using FakeXiecheng.API.Helpers;
using FakeXiecheng.API.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FakeXiecheng.API.Services
{
    public interface ITouristRouteRepository
    {
        //bool TouristRouteExists(Guid touristRouteId);
        //PaginationList<TouristRoute> GetTouristRoutes(TouristRouteFilterParameters filterParameters);
        //IEnumerable<TouristRoute> GetTouristRoutesByIdList(IEnumerable<Guid> touristRouteIds);
        //TouristRoute GetTouristRouteById(Guid routeId);
        //void AddTouristRoute(TouristRoute route);
        //void UpdateTouristRoute(TouristRoute route);
        //void DeleteTouristRoute(TouristRoute route);
        //IEnumerable<TouristRoutePicture> GetPicturesByTouristRouteId(Guid touristRouteId);
        //TouristRoutePicture GetPicturesByTouristRouteIdAndPictureId(Guid touristRouteId, int pictureId);
        //void AddTouristRoutePicture(Guid touristRouteId, TouristRoutePicture picture);
        //bool Save();

        Task<bool> TouristRouteExistsAsync(Guid touristRouteId);
        Task<PaginationList<TouristRoute>> GetTouristRoutesAsync(TouristRouteFilterParameters filterParameters);
        Task<IEnumerable<TouristRoute>> GetTouristRoutesByIdListAsync(IEnumerable<Guid> touristRouteIds);
        Task<TouristRoute> GetTouristRouteByIdAsync(Guid routeId);
        //Task AddTouristRouteAsync(TouristRoute route);
        // 更合理是不用 async 来添加 TouristRoute
        void AddTouristRoute(TouristRoute route);
        void UpdateTouristRoute(TouristRoute route);
        void DeleteTouristRoute(TouristRoute route);
        Task<IEnumerable<TouristRoutePicture>> GetPicturesByTouristRouteIdAsync(Guid touristRouteId);
        Task<TouristRoutePicture> GetPicturesByTouristRouteIdAndPictureIdAsync(Guid touristRouteId, int pictureId);
        // 想一想 为什么 add picture 要用 async？？
        Task AddTouristRoutePictureAsync(Guid touristRouteId, TouristRoutePicture picture);
        Task AddRangeForTouristRoutePictureListAsync(Guid touristRouteId, IEnumerable<TouristRoutePicture> pictures);
        Task<bool> SaveAsync();
        Task<object> GetFakeImageContentFromExternalAPI(string url);
    }
}
