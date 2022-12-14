using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalkDifficultyController : Controller
    {
        private readonly IWalkDifficultyRepository walkDifficultyRepository;
        private readonly IMapper mapper;

        public WalkDifficultyController(IWalkDifficultyRepository walkDifficultyRepository, IMapper mapper)
        {
            this.walkDifficultyRepository = walkDifficultyRepository;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllWalkDifficultiesAsync()
        {
            var WalkDiff = await walkDifficultyRepository.GetAllAsync();

            //Domain to DTO
            var WalkDiffDTO = mapper.Map<List<Models.DTO.WalkDifficulty>>(WalkDiff);

            return Ok(WalkDiffDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkDifficultyByIdAsync")]
        public async Task<IActionResult> GetWalkDifficultyByIdAsync(Guid id)
        {
            var WalkDiff = await walkDifficultyRepository.GetAsync(id);

            if (WalkDiff == null)
            {
                return NotFound("Walk Difficulty with Id not found");
            }

            //Domain object to DTO conversion
            var WalkDiffDTO = mapper.Map<Models.DTO.WalkDifficulty>(WalkDiff);

            return Ok(WalkDiffDTO);
        }


        [HttpPost]
        public async Task<IActionResult> AddWalkDifficultyAsync(Models.DTO.AddWalkDifficultyRequest addwalkDifficultyRequest)
        {
            //DTO to Domain
            var walkdifficultydomain = new Models.Domain.WalkDifficulty
            {
                Code = addwalkDifficultyRequest.Code
            };
            //Call Repository to update DB
            walkdifficultydomain = await walkDifficultyRepository.AddAsync(walkdifficultydomain);

            //Domain to DTO

            var WalkDiffDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkdifficultydomain);

            return CreatedAtAction(nameof(GetWalkDifficultyByIdAsync), new { id = WalkDiffDTO.Id }, WalkDiffDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkDifficultyAsync([FromRoute] Guid id, [FromBody] Models.DTO.UpdateWalkDifficultyRequest updateWalkDifficultyRequestDTO)
        {
            //Convert DTO to Domain
            var walkDiffDomain = new Models.Domain.WalkDifficulty
            {
                Code = updateWalkDifficultyRequestDTO.Code
            };
            //Update the DB
            var walkDiffDomian = await walkDifficultyRepository.UpdateAsync(id, walkDiffDomain);

            if (walkDiffDomian == null)
            {
                return NotFound("Difficulty with ID not Found");
            }
            //Domain to DTO
            var WalkDiffDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDiffDomian);
            //Return
            return Ok(WalkDiffDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]

        public async Task<IActionResult> DeleteWalkDifficulty(Guid id)
        {
            var walkDifficultyDomain = await walkDifficultyRepository.DeleteAsync(id);
            if (walkDifficultyDomain == null)
            {
                return NotFound();
            }
            //convert to DTO
            var WalkDiffDeletedDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficultyDomain);
            return Ok(WalkDiffDeletedDTO);

        }

    }
}
