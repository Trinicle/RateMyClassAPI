using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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

        [HttpGet("searchName", Name = "GetUniversityByName")]
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

        [HttpGet("searchId", Name = "GetUniversityById")]
        public async Task<IActionResult> GetUniversityById([FromQuery] UniversityIdRequest parameters)
        {
            if (parameters.id < 1)
            {
                return BadRequest();
            }
            var university = await _universityInfoRepository.UniversityExists(parameters.id);

            if (university is null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<UniversityDto>(university));
        }

        [HttpPost("create")]
        public async Task<ActionResult<UniversityDto>> CreateUniversity(
            [FromQuery] CreateUniversityDto university)
        {

            var newUniversity = _mapper.Map<Entities.University>(university);

            bool returnBool = await _universityInfoRepository.AddUniversity(newUniversity);

            if (!returnBool)
            {
                return BadRequest();
            }

            var createdUniversity = _mapper.Map<Models.Get.UniversityDto>(newUniversity);

            return CreatedAtRoute("GetUniversityById",
                new
                {
                    id = createdUniversity.Id,
                },
                createdUniversity);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteUniversity(
            [FromQuery] UniversityIdRequest parameters)
        {
            if (parameters.id < 1)
            {
                return BadRequest();
            }
            var university = await _universityInfoRepository.UniversityExists(parameters.id);

            if (university is null)
            {
                return NotFound();
            }

            await _universityInfoRepository.DeleteUniversity(parameters.id);

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

public record UniversityIdRequest
{
    [BindRequired]
    public int id { get; init; }
}