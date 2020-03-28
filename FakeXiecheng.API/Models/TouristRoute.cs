using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FakeXiecheng.API.Models
{
    public class TouristRoute
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(1500)]
        public string Description { get; set; }

        public double OriginalPrice { get; set; }

        public string Coupons { get; set; }

        public string Points { get; set; }

        [Range(0.0, 100.0)]
        public double DiscountPercent { get; set; }

        public double? Rating { get; set; }

        [ForeignKey("TouristRoutePictureId")]
        public ICollection<TouristRoutePicture> TouristRoutePictures { get; set; }

        [MaxLength]
        public string Features { get; set; }

        [MaxLength]
        public string Fees { get; set; }

        [MaxLength]
        public string Notes { get; set; }
    }
}
