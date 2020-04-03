using FakeXiecheng.API.DbContexts;
using FakeXiecheng.API.Dtos;
using FakeXiecheng.API.Helpers;
using FakeXiecheng.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace FakeXiecheng.API.Services
{
    public class TouristRouteRepository : ITouristRouteRepository, IDisposable
    {
        private readonly TouristLibraryContext _context;
        private readonly IPropertyMappingService _propertyMappingService;
        private readonly IHttpClientFactory _httpClientFactory;
        private CancellationTokenSource _cancellationTokenSource;
        private readonly ILogger<TouristRouteRepository> _logger;

        public TouristRouteRepository(
            TouristLibraryContext context,
            IPropertyMappingService propertyMappingService,
            IHttpClientFactory httpClientFactory,
            ILogger<TouristRouteRepository> logger
        )
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _propertyMappingService = propertyMappingService ?? throw new ArgumentNullException(nameof(propertyMappingService));
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> TouristRouteExistsAsync(Guid touristRouteId)
        {
            if (touristRouteId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(touristRouteId));
            }
            return await _context.TouristRoutes.AnyAsync(t => t.Id == touristRouteId);
        }

        public async Task<PaginationList<TouristRoute>> GetTouristRoutesAsync(TouristRouteFilterParameters filterParameters)
        {
            //IQueryable<TouristRoute> collectionBeforePaging =
            //    _context.TouristRoutes
            //    .Include(t => t.TouristRoutePictures);

            IQueryable<TouristRoute> collectionBeforePaging = _context.TouristRoutes
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
            return await PaginationList<TouristRoute>.Create(
                    collectionBeforePaging,
                    filterParameters.PageNumber,
                    filterParameters.PageSize
                );
        }

        public async Task<IEnumerable<TouristRoute>> GetTouristRoutesByIdListAsync(IEnumerable<Guid> touristRouteIds)
        {
            if (touristRouteIds == null)
            {
                throw new ArgumentNullException(nameof(touristRouteIds));
            }
            return await _context.TouristRoutes.Where(t => touristRouteIds.Contains(t.Id)).ToListAsync();
        }

        public async Task<TouristRoute> GetTouristRouteByIdAsync(Guid routeId)
        {
            var calculate = CpuFullyLoadedTasker.ComplicatCalculation();

            if (routeId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(routeId));
            }
            return await _context.TouristRoutes
                .Include(t => t.TouristRoutePictures)
                .Where(c => c.Id == routeId)
                .FirstOrDefaultAsync();
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

        public void DeleteTouristRoute(TouristRoute route)
        {
            _context.TouristRoutes.Remove(route);
        }

        public async Task<IEnumerable<TouristRoutePicture>> GetPicturesByTouristRouteIdAsync(Guid touristRouteId)
        {
            if (touristRouteId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(touristRouteId));
            }
            return await _context.TouristRoutePictures
                .Where(p => p.TouristRouteId == touristRouteId)
                .ToListAsync();
        }

        public async Task<TouristRoutePicture> GetPicturesByTouristRouteIdAndPictureIdAsync(Guid touristRouteId, int pictureId)
        {
            if (touristRouteId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(touristRouteId));
            }
            return await _context.TouristRoutePictures
                .Where(p => p.TouristRouteId == touristRouteId && p.Id == pictureId)
                .SingleOrDefaultAsync();
        }

        public async Task AddTouristRoutePictureAsync(Guid touristRouteId, TouristRoutePicture picture)
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
            await _context.TouristRoutePictures.AddAsync(picture);
        }

        public async Task AddRangeForTouristRoutePictureListAsync(
            Guid touristRouteId,
            IEnumerable<TouristRoutePicture> pictures
        )
        {
            if (touristRouteId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(touristRouteId));
            }
            if (pictures.Count() == 0)
            {
                throw new ArgumentNullException("no picture insert");
            }
            var picturesToAdd = pictures.Select(p =>
            {
                p.TouristRouteId = touristRouteId;
                return p;
            });
            await _context.TouristRoutePictures.AddRangeAsync(picturesToAdd);
        }

        public async Task<bool> SaveAsync()
        {
            return (await _context.SaveChangesAsync()) >= 0;
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
                if (_context != null)
                {
                    _context.Dispose();
                }
                if (_cancellationTokenSource != null)
                {
                    _cancellationTokenSource.Dispose();
                    _cancellationTokenSource = null;
                }
            }
        }

        public async Task<object> DownloadFakeImageFromExternalAPI(string url)
        {
            // Call API by http GET request
            // commet是因为有更好的方式，就是HttpClientFactory
            // 需要提前注入依赖 services.AddHttpClient();
            // var httpClient = new HttpClient(); 

            var httpClient = _httpClientFactory.CreateClient();

            _cancellationTokenSource = new CancellationTokenSource();

            return await this.GetFakeImagehttpResponse(httpClient, url, _cancellationTokenSource.Token);

            //var response = await httpClient.GetAsync(url);

            //if (response.IsSuccessStatusCode)
            //{
            //    return JsonConvert.DeserializeObject<object>(
            //        await response.Content.ReadAsStringAsync());
            //}

            //return null;
        }

        public async Task<IEnumerable<object>> DownloadFakeImageListFromExternalAPI(IEnumerable<string> urls)
        {
            var httpClient = _httpClientFactory.CreateClient();

            //IList<object> results = new List<object>();
            //foreach (var url in urls)
            //{
            //    var response = await httpClient.GetAsync(url);
            //    if (response.IsSuccessStatusCode)
            //    {
            //        results.Add(JsonConvert.DeserializeObject<object>(
            //            await response.Content.ReadAsStringAsync()));

            //    }
            //}
            //return results;

            _cancellationTokenSource = new CancellationTokenSource();

            // create the tasks
            var downloadFakeImageTasksQuery =
                 from url
                 in urls
                 select GetFakeImagehttpResponse(httpClient, url, _cancellationTokenSource.Token);

            // start the tasks
            var downloadFakeImageTasks = downloadFakeImageTasksQuery.ToList();

            try
            {
                return await Task.WhenAll(downloadFakeImageTasks);
            }
            catch (OperationCanceledException operationCanceledException)
            {
                _logger.LogInformation($"{operationCanceledException.Message}");
                foreach (var task in downloadFakeImageTasks)
                {
                    _logger.LogInformation($"Task {task.Id} has status {task.Status}");
                }
                return new List<object>();
            }
            catch (Exception exception)
            {
                _logger.LogError($"{exception.Message}");
                // 异常需要一层一层向上抛， 继续向上抛出异常，抛给controller
                throw exception;
            }
        }

        private async Task<object> GetFakeImagehttpResponse(
            HttpClient httpClient,
            string url,
            CancellationToken cancellationToken
        )
        {
            // 测试，随便抛出一个异常，也需要被上层处理
            //throw new Exception("Cannot download book cover, writer isn't finishing book fast enough.");

            var response = await httpClient.GetAsync(url, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<object>(
                    await response.Content.ReadAsStringAsync());
            }

            // 任务终止，抛出异常
            _cancellationTokenSource.Cancel();

            return null;
        }
    }
}
