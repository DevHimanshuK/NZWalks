using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
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
        public RegionsController(IRegionRepository regionRepository, IMapper mapper)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRegionsAsync()
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

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetRegionAsync")]
        public async Task<IActionResult> GetRegionAsync(Guid id)
        {
            var region = await regionRepository.GetAsync(id);

            if (region == null)
            {
                return NotFound("Region with provided ID not found");
            }
            var regionDTO = mapper.Map<Models.DTO.Region>(region);
            return Ok(regionDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddRegionAsync(Models.DTO.AddRegionRequest addRegionRequest)
        {
            //Validation method
            if (!ValidateAddRegionAsync(addRegionRequest))
            {
                //Model state is very useful for validation
                return BadRequest(ModelState);
            }

            //we dont want user to provide the Guid so we created separate addregionrequest DTO and called here

            //Request(DTO) to Domain Model
            var region = new Models.Domain.Region()
            {
                Code = addRegionRequest.Code,
                Name = addRegionRequest.Name,
                Area = addRegionRequest.Area,
                Lat = addRegionRequest.Lat,
                Long = addRegionRequest.Long,
                Population = addRegionRequest.Population
            };

            //Pass Details to Repo
            region = await regionRepository.AddAsync(region);

            //convert data back to DTO
            var regionDTO = new Models.DTO.Region
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Population = region.Population

            };

            //Created at action return sends status code 201 - Created
            //We can pass action name in double quotes, but to make it type safe we pass with nameof()
            return CreatedAtAction(nameof(GetRegionAsync), new { id = regionDTO.Id }, regionDTO);

        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteRegionAsync(Guid id)
        {
            //Get region from DB
            //Delete Region from DB
            var response = await regionRepository.DeleteAsync(id);

            //If no region exist respond not found
            if (response == null)
            {
                return NotFound("No region exist for provided ID");
            }
            //Convert response to DTO
            var regionDTO = new Models.DTO.Region
            {
                Id = response.Id,
                Code = response.Code,
                Name = response.Name,
                Area = response.Area,
                Lat = response.Lat,
                Long = response.Long,
                Population = response.Population

            };
            //Respond user of delete success
            //Return Ok
            return Ok(regionDTO);

        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateRegionAsync([FromRoute] Guid id, [FromBody] Models.DTO.UpdateRegionRequest updateRegionRequest)
        {
            //Validate update region request
            if(!ValidateUpdateRegionAsync(updateRegionRequest))
            {
                return BadRequest(ModelState);
            }

            //DTO to Domain Model
            var updateResult = new Models.Domain.Region()
            {
                Code = updateRegionRequest.Code,
                Name = updateRegionRequest.Name,
                Area = updateRegionRequest.Area,
                Lat = updateRegionRequest.Lat,
                Long = updateRegionRequest.Long,
                Population = updateRegionRequest.Population

            };

            //Update the detail if id exist
            updateResult = await regionRepository.UpdateAsync(id, updateResult);

            if (updateResult == null)
            {
                return NotFound();
            }

            //convert Domain response object to DTO
            var regionDTO = new Models.DTO.Region()
            {
                //Mapping Domain object to DTO
                Id = updateResult.Id,
                Code = updateResult.Code,
                Name = updateResult.Name,
                Area = updateResult.Area,
                Lat = updateResult.Lat,
                Long = updateResult.Long,
                Population = updateResult.Population
            };

            //return success code with DTO data
            return Ok(regionDTO);

        }

        #region Private methods
        private bool ValidateAddRegionAsync(Models.DTO.AddRegionRequest addRegionRequest)
        {
            if(addRegionRequest == null)
            {
                ModelState.AddModelError(nameof(addRegionRequest),
                    $"Data is required");
                return false;
            }
            if (string.IsNullOrWhiteSpace(addRegionRequest.Code))
            {
                ModelState.AddModelError(nameof(addRegionRequest.Code),
                    $"{addRegionRequest.Code} cannot be null or contain white space.");
                
            }
            if (string.IsNullOrWhiteSpace(addRegionRequest.Name))
            {
                ModelState.AddModelError(nameof(addRegionRequest.Name),
                    $"{addRegionRequest.Code} cannot be null or contain white space.");
            }
            if (addRegionRequest.Area <= 0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Area),
                    $"{addRegionRequest.Area} should be greater than 0.");
            }
            
            if (addRegionRequest.Population < 0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Population),
                    $"{addRegionRequest.Population} should be greater than 0.");
            }

            if(ModelState.ErrorCount > 0)
            {
                return false;
            }
            return true;
        }
        private bool ValidateUpdateRegionAsync(Models.DTO.UpdateRegionRequest updateRegionRequest)
        {
            if (updateRegionRequest == null)
            {
                ModelState.AddModelError(nameof(updateRegionRequest),
                    $"Data is required");
                return false;
            }
            if (string.IsNullOrWhiteSpace(updateRegionRequest.Code))
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Code),
                    $"{updateRegionRequest.Code} cannot be null or contain white space.");

            }
            if (string.IsNullOrWhiteSpace(updateRegionRequest.Name))
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Name),
                    $"{updateRegionRequest.Code} cannot be null or contain white space.");
            }
            if (updateRegionRequest.Area <= 0)
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Area),
                    $"{updateRegionRequest.Area} should be greater than 0.");
            }

            if (updateRegionRequest.Population < 0)
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Population),
                    $"{updateRegionRequest.Population} should be greater than 0.");
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
