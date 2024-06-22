using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
        private readonly IMapper _mapper;

        public CoursesController(
            IMapper mapper,
            IUniversityInfoRepository universityInfoRepository)
        {
            _universityInfoRepository = universityInfoRepository ??
                throw new ArgumentException(nameof(universityInfoRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("{courseId}", Name = "GetCourse")]
        public async Task<ActionResult<CourseDto>> GetCourse(int universityId, int courseId)
        {
            if (await _universityInfoRepository.UniversityExists(universityId) is null)
            {
                return NotFound();
            }

            var course = await _universityInfoRepository.GetCourseForUniversity(universityId, courseId);

            if ( course is null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<CourseDto>(course));
        }

        [HttpPost]
        public async Task<ActionResult<CourseDto>> CreateCourse(
            int universityId,
            [FromQuery] CreateCourseDto course) 
        {
            if (await _universityInfoRepository.UniversityExists(universityId) is null)
            {
                return NotFound();
            }

            var finalCourse = _mapper.Map<Entities.Course>(course);

            await _universityInfoRepository.AddCourseForUniversity(universityId, finalCourse);

            await _universityInfoRepository.SaveChanges();

            var createdCourse = _mapper.Map<Models.Get.CourseDto>(finalCourse);

            return CreatedAtRoute("GetCourse",
                new
                {
                    universityId = universityId,
                    courseId = createdCourse.Id
                },
                createdCourse);
        } 
    }
}