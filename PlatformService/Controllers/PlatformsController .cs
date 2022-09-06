using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;

namespace PlatformService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepo _repository;
        public PlatformsController(IPlatformRepo repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Platform>> GetPlatforms()
        {
            Console.WriteLine("--> Getting Platforms....");
            var platformItem = _repository.GetAllPlatforms();
            return Ok(platformItem);
        }

        [HttpGet("{id}")]
        public ActionResult<Platform> GetPlatformById(int id,string nama,string alamat)
        {
            var platformItem = _repository.GetPlatformById(id);
            if (platformItem != null)
            {
                platformItem.Name += " " + nama + " " + alamat;
                return Ok(platformItem);
            }
            return NotFound();
        }

        [HttpGet("ByName")]
        public ActionResult<IEnumerable<Platform>> GetByName(string name)
        {
            Console.WriteLine("--> Getting Platforms by name....");
            var platformItem = _repository.GetByName(name);
            return Ok(platformItem);
        }

        [HttpPost]
        public ActionResult<Platform> CreatePlatform(Platform platform)
        {
            _repository.CreatePlatform(platform);
            _repository.SaveChanges();

            return CreatedAtAction(nameof(GetPlatformById),
                new {Id=platform.Id},platform);
        }
    }
}