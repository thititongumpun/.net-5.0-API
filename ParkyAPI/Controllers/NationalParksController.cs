using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models;
using ParkyAPI.Models.Dtos;
using ParkyAPI.Repository.IRepository;

namespace ParkyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NationalParksController : ControllerBase
    {
        private readonly INationalParkRepository _repo;
        private readonly IMapper _mapper;
        public NationalParksController(INationalParkRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        /// <summary>
        /// Get list of national parks.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<NationalParkDtos>))]
        [ProducesResponseType(400)]
        public IActionResult GetNationalParks()
        {
            var objList = _repo.GetNationalParks();
            var objDtos = new List<NationalParkDtos>();
            foreach (var obj in objList)
            {
                objDtos.Add(_mapper.Map<NationalParkDtos>(obj));
            }
            return Ok(objDtos);
        }


        /// <summary>
        /// Get individual national park
        /// </summary>
        /// <param name="nationalParkId"> Id of national park.</param>
        /// <returns></returns>
        [HttpGet("{nationalParkId:int}", Name = "GetNationalPark")]
        public IActionResult GetNationalPark(int nationalParkId)
        {
            var obj = _repo.GetNationalPark(nationalParkId);
            if (obj == null)
            {
                return NotFound();
            }

            var objDtos = _mapper.Map<NationalParkDtos>(obj);

            return Ok(objDtos);
        }

        [HttpPost]
        public IActionResult CreateNationalPark([FromBody] NationalParkDtos nationalParkDtos )
        {
            if (nationalParkDtos == null)
            {
                return BadRequest(ModelState);
            }

            if (_repo.NationalParkExists(nationalParkDtos.Name))
            {
                ModelState.AddModelError("", "National Park Exists");
                return StatusCode(404, ModelState);
            }

            var nationalParkObj = _mapper.Map<NationalPark>(nationalParkDtos);

            if(!_repo.CreateNationalPark(nationalParkObj))
            {
                ModelState.AddModelError("", $"Error saving record {nationalParkObj.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetNationalPark", new { nationalParkId = nationalParkObj.Id }, nationalParkObj);
        }

        [HttpPatch("{nationalParkId:int}", Name = "UpdateNationalPark")]
        public IActionResult UpdateNationalPark(int nationalParkId, [FromBody] NationalParkDtos nationalParkDtos)
        {
            if (nationalParkDtos == null || nationalParkId != nationalParkDtos.Id)
            {
                return BadRequest(ModelState);
            }

            var nationalParkObj = _mapper.Map<NationalPark>(nationalParkDtos);

            if(!_repo.UpdateNationalPark(nationalParkObj))
            {
                ModelState.AddModelError("", $"Error updating record {nationalParkObj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{nationalParkId:int}", Name = "DeleteNationalPark")]
        public IActionResult DeleteNationalPark(int nationalParkId)
        {
            if (!_repo.NationalParkExists(nationalParkId))
            {
                return NotFound();
            }

            var nationalParkObj = _repo.GetNationalPark(nationalParkId);

            if(!_repo.DeleteNationalPark(nationalParkObj))
            {
                ModelState.AddModelError("", $"Error deleting record {nationalParkObj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}