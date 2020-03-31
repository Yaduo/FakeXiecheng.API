using System;
using FakeXiecheng.API.Models;

namespace FakeXiecheng.API.Dtos
{
    public class TouristRouteForCreationWithTripAttributeDto: TouristRouteForCreationDto
    {
        public DateTime? DepartureTime { get; set; }

        public TravelDays? TravelDays { get; set; }

        public TripType? TripType { get; set; }

        public DepartureCity? DepartureCity { get; set; }
    }
}
