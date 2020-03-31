using System;
using System.Collections.Generic;
using FakeXiecheng.API.Models;

namespace FakeXiecheng.API.Dtos
{
    public class TouristRouteDto
    {
        public TouristRouteDto()
        {
            TouristRoutePictures = new List<TouristRoutePictureDto>();
        }
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal OriginalPrice { get; set; }
        public double? DiscountPercent { get; set; }
        public decimal Price { get; set; }
        public string Coupons { get; set; }
        public int? Points { get; set; }
        public double? Rating { get; set; }
        public string Features { get; set; }
        public string Fees { get; set; }
        public string Notes { get; set; }
        public IList<TouristRoutePictureDto> TouristRoutePictures { get; set; }

        public DateTime CreateTimeUTC { get; set; }

        public DateTime? UpdateTimeUTC { get; set; }

        public DateTime? DepartureTime { get; set; }

        public TravelDays? TravelDays { get; set; }

        public TripType? TripType { get; set; }

        public DepartureCity? DepartureCity { get; set; }
    }
}
