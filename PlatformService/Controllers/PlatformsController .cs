using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepo _repository;
        private readonly ICommandDataClient _commandDataClient;

        public PlatformsController(IPlatformRepo repository, ICommandDataClient commandDataClient)
        {
            _repository = repository;
            _commandDataClient = commandDataClient;
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
        public async Task<ActionResult<Platform>> CreatePlatform(Platform platform)
        {
            _repository.CreatePlatform(platform);
            _repository.SaveChanges();

            try
            {
                await _commandDataClient.SendPlatformToCommand(platform);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"--> could not send synchronous data: {ex.Message}");
            }

            return CreatedAtAction(nameof(GetPlatformById),
                new {Id=platform.Id},platform);
        }

        [HttpPut]
        public ActionResult<Platform> UpdatePlatform(Platform platform)
        {
            try
            {
                 _repository.UpdatePlatform(platform);
                 return platform;
            }
            catch (System.Exception ex)
            {
                return BadRequest($"Gagal update data {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public ActionResult DeletePlatform(int id)
        {
            try
            {
                _repository.DeletePlatform(id);
                return Ok($"Data id {id} berhasil diupdate");
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}