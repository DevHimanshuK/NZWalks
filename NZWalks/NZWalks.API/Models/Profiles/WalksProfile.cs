using AutoMapper;

namespace NZWalks.API.Models.Profiles
{
    public class WalksProfile : Profile
    {
        public WalksProfile()
        {
            //creating map with args <Source, Destination>
            CreateMap < Models.Domain.Walk, Models.DTO.Walk> ()
                .ReverseMap();

            CreateMap < Models.Domain.WalkDifficulty, Models.DTO.WalkDifficulty> ()
                .ReverseMap();
        }
      
    }
}
