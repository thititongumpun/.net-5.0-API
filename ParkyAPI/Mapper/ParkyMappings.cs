using AutoMapper;
using ParkyAPI.Models;
using ParkyAPI.Models.Dtos;

namespace ParkyAPI.Mapper
{
    public class ParkyMappings : Profile
    {
        public ParkyMappings()
        {
            CreateMap<NationalPark, NationalParkDtos>().ReverseMap();
            CreateMap<Trail, TrailDtos>().ReverseMap();
            CreateMap<Trail, TrailCreateDtos>().ReverseMap();
            CreateMap<Trail, TrailUpdateDtos>().ReverseMap();
        }
    }
}