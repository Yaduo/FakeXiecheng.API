﻿using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FakeXiecheng.API.Dtos;
using FakeXiecheng.API.Models;

namespace FakeXiecheng.API.Profiles
{
    public class TouristRouteProfile: Profile
    {
        public TouristRouteProfile()
        {
            CreateMap<TouristRoute, TouristRouteDto>()
                .ForMember(
                    dest => dest.Price,
                    opt => opt.MapFrom(src => src.OriginalPrice * (decimal)(src.DiscountPercent ?? 1)))
                .ForMember(
                    dest => dest.Pictures,
                    // TODO: 应该用automapper来映射
                    opt => opt.MapFrom(src => src.TouristRoutePictures.Select(p => new TouristRoutePictureDto() {
                        Id = p.Id,
                        Url = p.Url
                    })))
                ;

            CreateMap<TouristRouteForCreationDto, TouristRoute>()
                .ForMember(
                    dest => dest.Id,
                    opt => opt.MapFrom(src => Guid.NewGuid()));
        }
    }
}
