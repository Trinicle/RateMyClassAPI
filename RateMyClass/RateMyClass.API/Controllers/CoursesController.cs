using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using RateMyClass.API.Entities;
using RateMyClass.API.Models.Create;
using RateMyClass.API.Models.Get;
using RateMyClass.API.Services;

namespace RateMyClass.API.Controllers
{
    [Route("api/universities/{universityId}/courses")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly IUniversityInfoRepository _universityInfoRepository;
        private readonly ICourseInfoRepository _courseInfoRepository;
        private readonly IMapper _mapper;

        public CoursesController(
            IMapper mapper,
            IUniversityInfoRepository universityInfoRepository,
            ICourseInfoRepository courseInfoRepository)
        {
            _universityInfoRepository = universityInfoRepository ??
                throw new ArgumentException(nameof(universityInfoRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            _courseInfoRepository = courseInfoRepository;
        }

        [HttpGet(Name = "GetCourses")]
        public async Task<ActionResult<CourseDto>> GetCourseById(int universityId)
        {
            if (!await _universityInfoRepository.UniversityExists(universityId))
            {
                return NotFound();
            }
            var courses = await _courseInfoRepository.GetCoursesForUniversity(universityId);

            return Ok(_mapper.Map<IEnumerable<CourseDto>>(courses));
        }

        [HttpGet("search/{id}", Name = "GetCourseById")]
        public async Task<ActionResult<CourseDto>> GetCourseById(int universityId, int id,
            [FromQuery] bool includeRatings)
        {
            if (!await _universityInfoRepository.UniversityExists(universityId))
            {
                return NotFound();
            }
            var course = await _courseInfoRepository.GetCourseForUniversityById(universityId, id);

            if (course is null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<CourseDto>(course));
        }

        [HttpGet("search", Name = "GetCourseByName")]
        public async Task<ActionResult<CourseDto>> GetCourseByName(int universityId, [FromQuery] CourseNameRequest parameters)
        {
            if (!await _universityInfoRepository.UniversityExists(universityId))
            {
                return NotFound();
            }
            var course = await _courseInfoRepository.GetCourseForUniversityByName(universityId, parameters.name, parameters.amount);

            if (course is null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<IEnumerable<CourseDto>>(course));
        }

        [HttpPost("create")]
        public async Task<ActionResult<CourseDto>> CreateCourse(int universityId, [FromBody] CreateCourseDto course) 
        {

            University? university = await _universityInfoRepository.GetUniversityById(universityId, false);

            if (university is null)
            {
                return NotFound();
            }

            var finalCourse = _mapper.Map<Entities.Course>(course);

            bool returnBool = await _universityInfoRepository.AddCourseForUniversity(university, finalCourse);

            if (!returnBool)
            {
                return BadRequest();
            }

            var createdCourse = _mapper.Map<Models.Get.CourseDto>(finalCourse);

            return CreatedAtRoute("GetCourseById",
                new
                {
                    universityId = universityId,
                    courseId = createdCourse.Id
                },
                createdCourse);
        }
        
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteCourse(int universityId, int id)
        {
            University? university = await _universityInfoRepository.GetUniversityById(universityId, false);

            if (university is null)
            {
                return NotFound();
            }

            bool returnBool = await _courseInfoRepository.DeleteCourse(university, id);

            if (!returnBool)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
public record CourseNameRequest
{
    [BindRequired]
    public string name { get; init; } = string.Empty;
    public int amount { get; init; } = 10;
}