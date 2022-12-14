using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalksController : Controller
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;

        public WalksController(IWalkRepository walkRepository, IMapper mapper)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalksAsync()
        {
            //Fetch data from DB as domain model object
            var walkDomainData = await walkRepository.GetAllAsync();
            //Map / convert the data to DTO
            var walksDTO = mapper.Map<List<Models.DTO.Walk>>(walkDomainData);
            //Return the data to user
            return Ok(walksDTO);

        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkAsync")]
        public async Task<IActionResult> GetWalkAsync(Guid id)
        {
            //fetch walk from DB as Walk Model domain object
            var walkDomainObj = await walkRepository.GetAsync(id);
            //Convert to DTO from Domain Object
            var walkDTO = mapper.Map<Models.DTO.Walk>(walkDomainObj);
            //Return Ok and object
            return Ok(walkDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddWalkAsync([FromBody] Models.DTO.AddWalkRequest addWalkReq)
        {
            //map AddWalkRequest DTO to Domain model object
            var walkDomain = new Models.Domain.Walk
            {
                Length = addWalkReq.Length,
                Name = addWalkReq.Name,
                RegionId = addWalkReq.RegionId,
                WalkDifficultyId = addWalkReq.WalkDifficultyId
            };
            //add walk to the db
            walkDomain = await walkRepository.AddAsync(walkDomain);

            //Convert domain response to DTO
            var walkDTO = mapper.Map<Models.DTO.Walk>(walkDomain);

            //success status code return
            return CreatedAtAction(nameof(GetWalkAsync), new { id = walkDTO.Id }, walkDTO);

        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UodateWalkAsync([FromRoute] Guid id, [FromBody] Models.DTO.UpdateWalkRequest updateWalkRequest)
        {
            //convert DTO to domain Object
            var walkDomainModel = new Models.Domain.Walk
            {
                Length = updateWalkRequest.Length,
                Name = updateWalkRequest.Name,
                RegionId = updateWalkRequest.RegionId,
                WalkDifficultyId = updateWalkRequest.WalkDifficultyId
            };
            //Pass details to Repository
            var updatedWalk = await walkRepository.UpdateAsync(id, walkDomainModel);

            //From Repo Response handle null or data
            if (updatedWalk != null)
            {
                return Ok(mapper.Map<Models.DTO.Walk>(updatedWalk));
            }
            //convert back from Domain to DTO
            //Return data

            return NotFound("Walk to be updated with ID not found");
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkAsync(Guid id)
        {
            //Call repository to delete walk - stored as walk domain object
            var deletedWalk = await walkRepository.DeleteAsync(id);

            if (deletedWalk == null)
            {
                return NotFound("Not found");
            }

            var deletedWalkDTO = mapper.Map<Models.DTO.Walk>(deletedWalk);

            return Ok(deletedWalkDTO);
        }
    }
}
