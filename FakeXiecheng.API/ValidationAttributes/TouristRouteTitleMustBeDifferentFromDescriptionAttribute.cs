using System;
using System.ComponentModel.DataAnnotations;
using FakeXiecheng.API.Dtos;

namespace FakeXiecheng.API.ValidationAttributes
{
    public class TouristRouteTitleMustBeDifferentFromDescriptionAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value,
            ValidationContext validationContext)
        {
            var touristRoute = (TouristRouteForCreationDto)validationContext.ObjectInstance;

            if (touristRoute.Title == touristRoute.Description)
            {
                return new ValidationResult(
                    "the provide description must be different from title",
                    new[] { nameof(TouristRouteForCreationDto) }
                );
            }

            return ValidationResult.Success;
        }
    }
}