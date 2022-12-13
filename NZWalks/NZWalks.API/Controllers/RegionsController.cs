using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    //2 ways to create route attribute.
    //[Route("Regions")]
    [Route("[controller]")] //this will automatically take the controller name
    //[Route("api/controller")]
    public class RegionsController : Controller
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        //Inject Repository service inside the controller constructor
        public RegionsController(IRegionRepository regionRepository , IMapper mapper)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task <IActionResult> GetAllRegions()
        {
            var regions = await regionRepository.GetAllAsync();

            // return DTO regions
            var RegionsDTO = new List<Models.DTO.Region>();

            regions.ToList().ForEach(region =>
            {
                var regionDTO = new Models.DTO.Region()
                {
                    //Mapping Domain object to DTO
                    Id = region.Id,
                    Code = region.Code,
                    Name = region.Name,
                    Area = region.Area,
                    Lat = region.Lat,
                    Long = region.Long,
                    Population = region.Population
                };
                RegionsDTO.Add(regionDTO);
            });

            //How to use mapper
            var regionsmapped = mapper.Map<List<Models.DTO.Region>>(regions);
            return Ok(regionsmapped);

            //Passing DTO in place of Domain directly
           // return Ok(RegionsDTO);
        }
    }
}
