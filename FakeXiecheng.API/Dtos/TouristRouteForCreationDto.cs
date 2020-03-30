using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FakeXiecheng.API.ValidationAttributes;

namespace FakeXiecheng.API.Dtos
{
    [TouristRouteTitleMustBeDifferentFromDescriptionAttribute(
          ErrorMessage = "Title must be different from description.")]
    public class TouristRouteForCreationDto //: IValidatableObject 
    {
        [Required(ErrorMessage = "You should fill out a title.")]
        [MaxLength(100, ErrorMessage = "The title shouldn't have more than 100 characters.")]
        public string Title { get; set; }
        [Required]
        [MaxLength(1500, ErrorMessage = "The description shouldn't have more than 1500 characters.")]
        public string Description { get; set; }
        public decimal OriginalPrice { get; set; }
        public double? DiscountPercent { get; set; }
        public string Coupons { get; set; }
        public int? Points { get; set; }
        public double? Rating { get; set; }
        public string Features { get; set; }
        public string Fees { get; set; }
        public string Notes { get; set; }
        public ICollection<TouristRoutePictureForCreationDto> TouristRoutePictures { get; set; }
            = new List<TouristRoutePictureForCreationDto>();

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    if (Title == Description)
        //    {
        //        yield return new ValidationResult(
        //            "the provide description must be different from title",
        //            new[] { "TouristRouteForCreationDto" }
        //            );
        //    }

        //}
    }
}
