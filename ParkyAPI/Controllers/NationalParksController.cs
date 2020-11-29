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
    public class NationalParksController : Controller
    {
        private readonly INationalParkRepository _repo;
        private readonly IMapper _mapper;
        public NationalParksController(INationalParkRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
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
        public IActionResult CreateNationalParks([FromBody] NationalParkDtos nationalParkDtos )
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
    }
}