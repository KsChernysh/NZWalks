using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.Repositories;

namespace NZWalksAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class WalkDifficultyController : Controller
    {
        private readonly IWalkDifficultyRepository walkRepository;
        private readonly IMapper mapper;

        public WalkDifficultyController(IWalkDifficultyRepository walkRepository, IMapper mapper)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;

        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            //get data from DB(domain model)
            var domainwalks = await walkRepository.GetAllAsync();
            //Convert data to DTO
            var walksDTO = mapper.Map<List<Models.DTO.WalkDifficulty>>(domainwalks);
            //return response ok
            return Ok(walksDTO);

        }
        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetAsync")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            //get data from db
            var walkdomain = await walkRepository.GetAsync(id);
            // convert domain data to dto (mapper)
            var walksDTO = mapper.Map<List<Models.DTO.WalkDifficulty>>(walkdomain);
            return Ok(walksDTO);
        }
        [HttpPost]


        public async Task<IActionResult> AddAsync([FromBody] Models.DTO.AddWalkDifficultyRequest addWalkRequest)
        {
            //convert DTO to domain
            var walkDomain = new Models.Domain.WalkDifficulty()
            {
              Code = addWalkRequest.Code

            };
            // pass domain object to repository for create(add walk) to DB
            walkDomain = await walkRepository.AddAsync(walkDomain);
            // convert domain back to DTO
            var walkDTO = new Models.DTO.WalkDifficulty()
            {
                Id = walkDomain.Id,
                Code = addWalkRequest.Code
            };
            //sent DTO response back to the client
            return CreatedAtAction(nameof(GetAsync), new { id = walkDTO.Id }, walkDTO);

        }
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkAsync([FromRoute] Guid id, [FromBody] Models.DTO.UpdateWalkDifficultyRequest updateWalkRequest)
        {
            // Convert DTO to domain 
            var walkDomain = new Models.Domain.WalkDifficulty()
            {
                Code = updateWalkRequest.Code
            };
            // push data to repository
            walkDomain = await walkRepository.UpdateAsync(id, walkDomain);

            // handle null
            if (walkDomain == null)
            {
                return NotFound();
            }
            // convert domain to DTO
            var walkDTO = new Models.DTO.WalkDifficulty()
            {
                Id = walkDomain.Id,
               Code=  walkDomain.Code

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
            var walkDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDomain);
            return Ok(walkDTO);

        
    }
}
}
