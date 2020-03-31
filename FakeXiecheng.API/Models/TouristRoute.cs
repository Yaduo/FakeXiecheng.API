using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FakeXiecheng.API.Models
{
    public class TouristRoute
    {
        public TouristRoute()
        {
            TouristRoutePictures = new List<TouristRoutePicture>();
        }

        [Key]    
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        [MaxLength(1500)]
        public string Description { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal OriginalPrice {get; set;}

        public string Coupons {get; set;}

        public int? Points { get; set; }

        [Range(0.0, 100.0)]
        public double? DiscountPercent { get; set; }

        public double? Rating { get; set; }

        public ICollection<TouristRoutePicture> TouristRoutePictures { get; set; }

        [MaxLength]
        public string Features { get; set; }
        
        [MaxLength]
        public string Fees { get; set; }

        [MaxLength]
        public string Notes { get; set; }

        public DateTime CreateTimeUTC { get; set; } = DateTime.UtcNow;

        public DateTime? UpdateTimeUTC { get; set; }
    } 
}
