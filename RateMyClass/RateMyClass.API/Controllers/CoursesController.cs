using AutoMapper;
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
        public async Task<ActionResult<CourseDto>> GetCourseById(int universityId,
            [FromQuery] string? name,
            [FromQuery] int amount = 10)
        {
            if (universityId < 1 || amount < 1)
            {
                return BadRequest();
            }

            if (!await _universityInfoRepository.UniversityExists(universityId))
            {
                return NotFound();
            }

            if (name is null)
            {
               IEnumerable<Course> courses = await _courseInfoRepository.GetCoursesForUniversity(universityId, amount);

                return Ok(_mapper.Map<IEnumerable<CourseDto>>(courses));
            }

            IEnumerable<Course> coursesByName = await _courseInfoRepository.GetCourseForUniversityByName(universityId, name, amount);

            return Ok(_mapper.Map<IEnumerable<CourseDto>>(coursesByName));
        }

        [HttpGet("{courseId}", Name = "GetCourseById")]
        public async Task<ActionResult<CourseDto>> GetCourseById(int universityId, int courseId,
            [FromQuery] bool includeRatings = false)
        {
            if (courseId < 1 || universityId < 1)
            {
                return BadRequest();
            }

            if (!await _universityInfoRepository.UniversityExists(universityId))
            {
                return NotFound();
            }

            Course? course = await _courseInfoRepository.GetCourseForUniversityById(universityId, courseId);

            if (course is null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<CourseDto>(course));
        }

        [HttpPost("create")]
        public async Task<ActionResult<CourseDto>> CreateCourse(int universityId, [FromBody] CreateCourseDto course) 
        {
            if (universityId < 1)
            {
                return BadRequest();
            }

            University? university = await _universityInfoRepository.GetUniversityById(universityId, false);

            if (university is null)
            {
                return NotFound();
            }

            Course finalCourse = _mapper.Map<Entities.Course>(course);

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
            if (universityId < 1)
            {
                return BadRequest();
            }

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