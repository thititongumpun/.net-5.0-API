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
    public class TrailsController : Controller
    {
        private readonly ITrailRepository _repo;
        private readonly IMapper _mapper;
        public TrailsController(ITrailRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetTrails()
        {
            var objList = _repo.GetTrails();
            var objDtos = new List<TrailDtos>();
            foreach (var obj in objList)
            {
                objDtos.Add(_mapper.Map<TrailDtos>(obj));
            }
            return Ok(objDtos);
        }

        [HttpGet("{trailId:int}", Name = "GetTrail")]
        public IActionResult GetTrail(int trailId)
        {
            var obj = _repo.GetTrail(trailId);
            if (obj == null)
            {
                return NotFound();
            }

            var objDtos = _mapper.Map<TrailDtos>(obj);

            return Ok(objDtos);
        }

        [HttpPost]
        public IActionResult CreateTrail([FromBody] TrailCreateDtos trailDtos )
        {
            if (trailDtos == null)
            {
                return BadRequest(ModelState);
            }

            if (_repo.TrailExists(trailDtos.Name))
            {
                ModelState.AddModelError("", "Trail Exists");
                return StatusCode(404, ModelState);
            }

            var trailObj = _mapper.Map<Trail>(trailDtos);

            if(!_repo.CreateTrail(trailObj))
            {
                ModelState.AddModelError("", $"Error saving record {trailObj.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetTrail", new { trailId = trailObj.Id }, trailObj);
        }

        [HttpPatch("{trailId:int}", Name = "UpdateTrail")]
        public IActionResult UpdateTrail(int trailId, [FromBody] TrailUpdateDtos trailDtos)
        {
            if (trailDtos == null || trailId != trailDtos.Id)
            {
                return BadRequest(ModelState);
            }

            var trailObj = _mapper.Map<Trail>(trailDtos);

            if(!_repo.UpdateTrail(trailObj))
            {
                ModelState.AddModelError("", $"Error updating record {trailObj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{trailId:int}", Name = "DeleteTrail")]
        public IActionResult DeleteTrail(int trailId)
        {
            if (!_repo.TrailExists(trailId))
            {
                return NotFound();
            }

            var trailObj = _repo.GetTrail(trailId);

            if(!_repo.DeleteTrail(trailObj))
            {
                ModelState.AddModelError("", $"Error deleting record {trailObj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}