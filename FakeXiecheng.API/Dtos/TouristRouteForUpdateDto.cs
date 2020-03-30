using System;
using System.ComponentModel.DataAnnotations;
using FakeXiecheng.API.ValidationAttributes;

namespace FakeXiecheng.API.Dtos
{
    public class TouristRouteForUpdateDto : TouristRouteForManipulationDto
    {
        [Required(ErrorMessage = "You should fill out a description.")]
        public override string Description { get => base.Description; set => base.Description = value; }
    }
}
