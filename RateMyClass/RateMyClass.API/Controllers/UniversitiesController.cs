using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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

        [HttpGet("name")]
        public async Task<IActionResult> GetUniversitiesByName([FromQuery] UniversityNameGetRequest parameters)
        {
            if(parameters.Amount < 1)
            {
                return BadRequest();
            }

            var universities = await _universityInfoRepository.GetUniversitiesByName(parameters.Name, parameters.Amount);

            if (!universities.Any())
            {
                return NotFound();
            }

            return Ok(_mapper.Map<IEnumerable<UniversityWithoutCoursesDto>>(universities));
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetUniversityById([FromQuery] UniversityIdGetRequest parameters)
        {
            if (parameters.Id < 1)
            {
                return BadRequest();
            }
            var university = await _universityInfoRepository.UniversityExists(parameters.Id);

            if (university is null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<UniversityDto>(university));
        }

    }
}

public record UniversityNameGetRequest
{
    [BindRequired]
    public string Name { get; init; } = string.Empty;
    public int Amount { get; init; } = 10;
}

public record UniversityIdGetRequest
{
    [BindRequired]
    public int Id { get; init; }
}