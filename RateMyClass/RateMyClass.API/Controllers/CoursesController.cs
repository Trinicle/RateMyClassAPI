using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using RateMyClass.API.Entities;
using RateMyClass.API.Models.Create;
using RateMyClass.API.Models.Get;
using RateMyClass.API.Models.Update;
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
        public async Task<IActionResult> GetCourseById(int universityId,
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

                return Ok(_mapper.Map<IEnumerable<CourseWithoutRatingsDto>>(courses));
            }

            IEnumerable<Course> coursesByName = await _courseInfoRepository.GetCourseForUniversityByName(universityId, name, amount);

            return Ok(_mapper.Map<IEnumerable<CourseWithoutRatingsDto>>(coursesByName));
        }

        [HttpGet("{courseId}", Name = "GetCourseById")]
        public async Task<IActionResult> GetCourseById(int universityId, int courseId,
            [FromQuery] bool includeRatings = false)
        {
            if (courseId < 1 || universityId < 1)
            {
                return BadRequest();
            }

            Course? course = await _courseInfoRepository.GetCourseForUniversityById(universityId, courseId);

            if (course is null)
            {
                return NotFound();
            }

            if (includeRatings)
            {
                return Ok(_mapper.Map<CourseDto>(course));
            }

            return Ok(_mapper.Map<CourseWithoutRatingsDto>(course));
        }

        [HttpPost]
        public async Task<IActionResult> CreateCourse(int universityId, [FromBody] CreateCourseDto course) 
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

            Course finalCourse = _mapper.Map<Course>(course);

            bool returnBool = await _courseInfoRepository.AddCourseForUniversity(university, finalCourse);

            if (!returnBool)
            {
                return BadRequest();
            }

            var createdCourse = _mapper.Map<CourseDto>(finalCourse);

            return CreatedAtRoute("GetCourseById",
                new
                {
                    universityId = universityId,
                    courseId = createdCourse.Id
                },
                createdCourse);
        }
        
        [HttpDelete("{courseId}")]
        public async Task<IActionResult> DeleteCourse(int universityId, int courseId)
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

            bool returnBool = await _courseInfoRepository.DeleteCourse(university, courseId);

            if (!returnBool)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpPut("{courseId}")]
        public async Task<IActionResult> PutCourse(int universityId, int courseId, [FromBody] UpdateCourseDto course)
        {
            if (courseId < 1 || universityId < 1)
            {
                return BadRequest();
            }

            Course? currentCourse = await _courseInfoRepository.GetCourseForUniversityById(universityId, courseId);

            if (currentCourse is null)
            {
                return NotFound();
            }

            _mapper.Map(course, currentCourse);

            await _courseInfoRepository.SaveChanges();

            return Ok(_mapper.Map<CourseDto>(currentCourse));
        }

        //[HttpPatch("{courseId}")]
        //public async Task<ActionResult<CourseDto>> PatchCourse(int universityId, int courseId, [FromBody] JsonPatchDocument<UpdateCourseDto> patchdoc)
        //{
        //    if (courseId < 1 || universityId < 1)
        //    {
        //        return BadRequest();
        //    }

        //    University? currentUniversity = await _universityInfoRepository.GetUniversityById(universityId, false);

        //    if (currentUniversity is null)
        //    {
        //        return NotFound();
        //    }

        //    Course? currentCourse = await _courseInfoRepository.GetCourseForUniversityById(universityId, courseId);

        //    if (currentCourse is null)
        //    {
        //        return NotFound();
        //    }

        //    var courseUpdate = _mapper.Map<UpdateCourseDto>(currentCourse);
        //    patchdoc.ApplyTo(courseUpdate);

        //    TryValidateModel(courseUpdate);

        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest();
        //    }

        //    _mapper.Map(courseUpdate, currentCourse);

        //    await _courseInfoRepository.SaveChanges();

        //    return Ok(_mapper.Map<CourseDto>(currentCourse));
        //}
    }
}