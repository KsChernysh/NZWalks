using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.Repositories;

namespace NZWalksAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
   
    public class WalkController : Controller
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;

        public WalkController(IWalkRepository walkRepository, IMapper mapper)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
            
        }

        [HttpGet]
       public async Task<IActionResult> GetAllAsync()
        {
            //get data from DB(domain model)
            var domainwalks = await  walkRepository.GetAllAsync();
            //Convert data to DTO
            var walksDTO = mapper.Map<List<Models.DTO.Walk>>(domainwalks);
            //return response ok
            return Ok(walksDTO);
 
        }
        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetAsync")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            //get data from db
           var walkdomain = await  walkRepository.GetAsync(id);
            // convert domain data to dto (mapper)
            var walksDTO = mapper.Map<List<Models.DTO.Walk>>(walkdomain);
            return  Ok(walksDTO);
        }
        [HttpPost]
        

        public async Task<IActionResult> AddWalkAsync([FromBody] Models.DTO.AddWalkRequest addWalkRequest)
        {
            //convert DTO to domain
            var walkDomain = new Models.Domain.Walk() {
                Length = addWalkRequest.Length,
                Name= addWalkRequest.Name,
                RegionId = addWalkRequest.RegionId,
                WalkDifficultyId = addWalkRequest.WalkDifficultyId
            
            };
            // pass domain object to repository for create(add walk) to DB
           walkDomain = await walkRepository.AddWalkAsync(walkDomain);
            // convert domain back to DTO
            var walkDTO = new Models.DTO.Walk() {
                Id = walkDomain.Id, 
                Length = walkDomain.Length,
                Name = walkDomain.Name,
                RegionId = walkDomain.RegionId,
                WalkDifficultyId = walkDomain.WalkDifficultyId

            };
            //sent DTO response back to the client
            return CreatedAtAction(nameof(GetAsync), new {id = walkDTO.Id }, walkDTO);

        }
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkAsync([FromRoute] Guid id, [FromBody] Models.DTO.UpdateWalkRequest updateWalkRequest)
        {
            // Convert DTO to domain 
            var walkDomain = new Models.Domain.Walk()
            {
                Length = updateWalkRequest.Length,
                Name = updateWalkRequest.Name,
                RegionId = updateWalkRequest.RegionId,
                WalkDifficultyId = updateWalkRequest.WalkDifficultyId
            };
            // push data to repository
           walkDomain =  await walkRepository.UpdateAsync(id, walkDomain);

            // handle null
           if(walkDomain == null)
            {
                return NotFound();
            }
            // convert domain to DTO
            var walkDTO = new Models.DTO.Walk()
                {
                    Id = walkDomain.Id,
                    Length = walkDomain.Length,
                    Name = walkDomain.Name,
                    RegionId = walkDomain.RegionId,
                    WalkDifficultyId = walkDomain.WalkDifficultyId

                };
                return Ok(walkDTO);
  
        }
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkAsync(Guid id)
        {
            var walkDomain = await walkRepository.DeleteAsync(id);
            if (walkDomain == null)
            {
                return NotFound();
            }
            var walkDTO = mapper.Map<Models.DTO.Walk>(walkDomain);
            return Ok(walkDTO);

        }
    }
}
