﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Repositories;

namespace NZWalksAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegionController : Controller
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionController(IRegionRepository regionRepository, IMapper mapper)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllRegions()
        {
            var regions = await regionRepository.GetAllAsync();
            /* var regionsDTO = new List<Models.DTO.Region>();
             regions.ToList().ForEach(region =>
             {
                     var regionDTO = new Models.DTO.Region()
                     {
                         Id = region.Id,
                         Code = region.Code, 
                         Name = region.Name,
                         Area = region.Area,
                         Lat = region.Lat,   
                         Long = region.Long, 
                         Population = region.Population, 

                     };
                     regionsDTO.Add(regionDTO);


             });*/
            var regionsDTO = mapper.Map<List<Models.Domain.Region>>(regions);
            return Ok(regionsDTO);
        }
    }
}
