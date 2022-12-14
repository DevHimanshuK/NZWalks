using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
        private readonly IRegionRepository regionRepository;
        private readonly IWalkDifficultyRepository walkDifficultyRepository;

        public WalksController(IWalkRepository walkRepository, IMapper mapper,
            IRegionRepository regionRepository, IWalkDifficultyRepository walkDifficultyRepository)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
            this.regionRepository = regionRepository;
            this.walkDifficultyRepository = walkDifficultyRepository;
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
            //Call Validation Method
            if (!(await ValidateAddWalkAsync(addWalkReq)))
            {
                return BadRequest(ModelState);
            }

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
        public async Task<IActionResult> UpdateWalkAsync([FromRoute] Guid id, [FromBody] Models.DTO.UpdateWalkRequest updateWalkRequest)
        {
            //Add Validation
            if (!(await ValidateUpdateWalkAsync(updateWalkRequest)))
            {
                return BadRequest(ModelState);
            }

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

        #region Private methods
        private async Task<bool> ValidateAddWalkAsync(Models.DTO.AddWalkRequest addWalkReq)
        {
            if (addWalkReq == null)
            {
                ModelState.AddModelError(nameof(addWalkReq),
                    $"{nameof(addWalkReq)} cannot be empty");
                return false;
            }

            if (string.IsNullOrWhiteSpace(addWalkReq.Name))
            {
                ModelState.AddModelError(nameof(addWalkReq.Name),
                    $"{nameof(addWalkReq.Name)} is Required");
            }
            if (addWalkReq.Length <= 0)
            {
                ModelState.AddModelError(nameof(addWalkReq.Length),
                    $"{nameof(addWalkReq.Length)} should be greater than 0");
            }
            //We are passing 2 GUID, those should be valid
            var region = await regionRepository.GetAsync(addWalkReq.RegionId);
            if (region == null)
            {
                ModelState.AddModelError(nameof(addWalkReq.RegionId),
                    $"{nameof(addWalkReq.RegionId)} is Invalid");
            }
            var walkDifficulty = await walkDifficultyRepository.GetAsync(addWalkReq.WalkDifficultyId);
            if (walkDifficulty == null)
            {
                ModelState.AddModelError(nameof(addWalkReq.WalkDifficultyId),
                    $"{nameof(addWalkReq.WalkDifficultyId)} is Invalid");
            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }
            return true;
        }

        private async Task<bool> ValidateUpdateWalkAsync(Models.DTO.UpdateWalkRequest updateWalkRequest)
        {
            if (updateWalkRequest == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest),
                    $"{nameof(updateWalkRequest)} cannot be empty");
                return false;
            }

            if (string.IsNullOrWhiteSpace(updateWalkRequest.Name))
            {
                ModelState.AddModelError(nameof(updateWalkRequest.Name),
                    $"{nameof(updateWalkRequest.Name)} is Required");
            }
            if (updateWalkRequest.Length <= 0)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.Length),
                    $"{nameof(updateWalkRequest.Length)} should be greater than 0");
            }
            //We are passing 2 GUID, those should be valid
            var region = await regionRepository.GetAsync(updateWalkRequest.RegionId);
            if (region == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.RegionId),
                    $"{nameof(updateWalkRequest.RegionId)} is Invalid");
            }
            var walkDifficulty = await walkDifficultyRepository.GetAsync(updateWalkRequest.WalkDifficultyId);
            if (walkDifficulty == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.WalkDifficultyId),
                    $"{nameof(updateWalkRequest.WalkDifficultyId)} is Invalid");
            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }
            return true;
        }

        #endregion
    }
}
