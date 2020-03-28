using System;
using System.ComponentModel.DataAnnotations;

namespace FakeXiecheng.API.Models
{
    public class TouristRoutePicture
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        public string Url { get; set; }
    }
}
