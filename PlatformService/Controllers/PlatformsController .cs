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
        public ActionResult<Platform> GetPlatformById(int id)
        {
            var platformItem = _repository.GetPlatformById(id);
            if (platformItem != null)
            {
                return Ok(platformItem);
            }
            return NotFound();
        }


    }
}