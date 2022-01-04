using AutoMapper;
using LocationSearch.API.LocationEndPoints;
using LocationSearch.ApplicationCore.Entities;

namespace LocationSearch.API
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Location, LocationDto>();
            CreateMap<LocationRequestDto, Location>();

            CreateMap<ListLocationRequest, LocationRequest>();
        }
    }
}
