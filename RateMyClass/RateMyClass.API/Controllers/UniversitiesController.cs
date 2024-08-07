﻿using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using RateMyClass.API.Entities;
using RateMyClass.API.Models.Create;
using RateMyClass.API.Models.Get;
using RateMyClass.API.Models.Update;
using RateMyClass.API.Services;

namespace RateMyClass.API.Controllers
{
    [Route("api/universities")]
    [ApiController]
    public class UniversitiesController : ControllerBase
    {
        private readonly IUniversityInfoRepository _universityInfoRepository;
        private readonly IMapper _mapper;

        public UniversitiesController(IUniversityInfoRepository universityInfoRepository, IMapper mapper)
        {
            _universityInfoRepository = universityInfoRepository ??
                throw new ArgumentException(nameof(universityInfoRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet(Name = "GetUniversities")]
        public async Task<IActionResult> GetUniversitiesByName(
            [FromQuery] string? name,
            [FromQuery] int amount = 10)
        {
            if (amount < 1)
            {
                return BadRequest();
            }

            if (name is null)
            {
                IEnumerable<University> universities = await _universityInfoRepository.GetUniversities(amount);


                return Ok(_mapper.Map<IEnumerable<University>>(universities));
            }

            IEnumerable<University> universitiesByName = await _universityInfoRepository.GetUniversitiesByName(name, amount);

            if (!universitiesByName.Any())
            {
                return NotFound();
            }

            return Ok(_mapper.Map<IEnumerable<UniversityWithoutListsDto>>(universitiesByName));
        }

        [HttpGet("{id}", Name = "GetUniversityById")]
        public async Task<IActionResult> GetUniversityById(int id, [FromQuery] bool includeLists = false)
        {
            if (id < 1)
            {
                return BadRequest();
            }

            University? university = await _universityInfoRepository.GetUniversityById(id, includeLists);

            if (university is null)
            {
                return NotFound();
            }

            if (includeLists)
            {

                return Ok(_mapper.Map<UniversityDto>(university));
            }


            return Ok(_mapper.Map<UniversityWithoutListsDto>(university));
        }

        [HttpPost]
        public async Task<IActionResult> CreateUniversity([FromBody] CreateUniversityDto bodyUniversity)
        {
            University newUniversity = _mapper.Map<University>(bodyUniversity);

            if (!await _universityInfoRepository.AddUniversity(newUniversity))
            {
                return BadRequest();
            }

            UniversityDto createdUniversity = _mapper.Map<UniversityDto>(newUniversity);

            return CreatedAtRoute("GetUniversityById",
                new
                {
                    id = createdUniversity.Id,
                    includeCourses = false
                },
                createdUniversity);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUniversity(int id)
        {
            if (id < 1)
            {
                return BadRequest();
            }

            if (!await _universityInfoRepository.UniversityExists(id))
            {
                return NotFound();
            }

            if (!await _universityInfoRepository.DeleteUniversity(id))
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUniversity(int id, [FromBody] UpdateUniversityDto university)
        {
            if (id < 1)
            {
                return BadRequest();
            }

            University? currentUniversity = await _universityInfoRepository.GetUniversityById(id, false);

            if (currentUniversity is null)
            {
                return NotFound();
            }

            _mapper.Map(university, currentUniversity);

            await _universityInfoRepository.SaveChanges();

            return Ok(_mapper.Map<UniversityDto>(currentUniversity));
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchUniversity(int id, [FromBody] JsonPatchDocument<UpdateUniversityDto> patchdoc)
        {
            if (id < 1)
            {
                return BadRequest();
            }

            var currentUniversity = await _universityInfoRepository.GetUniversityById(id, false);

            if (currentUniversity is null)
            {
                return NotFound();
            }

            var universityUpdate = _mapper.Map<UpdateUniversityDto>(currentUniversity);
            patchdoc.ApplyTo(universityUpdate);

            TryValidateModel(universityUpdate);

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            _mapper.Map(universityUpdate, currentUniversity);

            await _universityInfoRepository.SaveChanges();

            return Ok(_mapper.Map<UniversityDto>(currentUniversity));
        }
    }
}