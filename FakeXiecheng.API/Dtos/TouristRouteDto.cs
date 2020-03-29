using System;
using System.Collections.Generic;

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
    }
}
