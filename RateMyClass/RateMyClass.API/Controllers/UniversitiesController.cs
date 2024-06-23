using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using RateMyClass.API.Entities;
using RateMyClass.API.Models.Create;
using RateMyClass.API.Models.Get;
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

        [HttpGet("search", Name = "GetUniversityByName")]
        public async Task<IActionResult> GetUniversitiesByName([FromQuery] UniversityNameGetRequest parameters)
        {
            if(parameters.amount < 1)
            {
                return BadRequest();
            }

            var universities = await _universityInfoRepository.GetUniversitiesByName(parameters.name, parameters.amount);

            if (!universities.Any())
            {
                return NotFound();
            }

            return Ok(_mapper.Map<IEnumerable<UniversityWithoutCoursesDto>>(universities));
        }

        [HttpGet("search/{id}", Name = "GetUniversityById")]
        public async Task<IActionResult> GetUniversityById([FromQuery] bool includeCourses, int id)
        {
            if (id < 1)
            {
                return BadRequest();
            }
            var university = await _universityInfoRepository.GetUniversityById(id, includeCourses);

            if (university is null)
            {
                return NotFound();
            }

            if (includeCourses)
            {
                return Ok(_mapper.Map<UniversityDto>(university));
            }

            return Ok(_mapper.Map<UniversityWithoutCoursesDto>(university));
        }

        [HttpPost("create")]
        public async Task<ActionResult<UniversityDto>> CreateUniversity([FromBody] CreateUniversityDto bodyUniversity)
        {
            var newUniversity = _mapper.Map<Entities.University>(bodyUniversity);

            if (!await _universityInfoRepository.AddUniversity(newUniversity))
            {
                return BadRequest();
            }

            var createdUniversity = _mapper.Map<Models.Get.UniversityDto>(newUniversity);

            return CreatedAtRoute("GetUniversityById",
                new
                {
                    id = createdUniversity.Id,
                    includeCourses = false
                },
                createdUniversity);
        }

        [HttpDelete("delete/{id}")]
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

            await _universityInfoRepository.DeleteUniversity(id);

            return Ok();
        }
    }
}

public record UniversityNameGetRequest
{
    [BindRequired]
    public string name { get; init; } = string.Empty;
    public int amount { get; init; } = 10;
}