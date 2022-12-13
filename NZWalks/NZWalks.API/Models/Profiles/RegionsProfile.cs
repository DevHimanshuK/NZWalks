using AutoMapper;
using Microsoft.AspNetCore.Routing.Constraints;

namespace NZWalks.API.Models.Profiles
{
    public class RegionsProfile : Profile
    {
        public RegionsProfile()
        {
            //creating map with args <Source, Destination>
            CreateMap<Models.Domain.Region, Models.DTO.Region>()
                .ReverseMap();
                //if mappings were not in correct struct. we can define mapping like
                
                //.ForMember(dest => dest.Id, options => options.MapFrom(src => src.Id));
        }
    }
}
