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
    [Route("api/universities/{universityId}/courses/{courseId}/ratings")]
    [ApiController]
    public class CourseRatingsController : ControllerBase
    {
        private readonly IUniversityInfoRepository _universityInfoRepository;
        private readonly ICourseInfoRepository _courseInfoRepository;
        private readonly ICourseRatingInfoRepository _ratingInfoRepository;
        private readonly IMapper _mapper;

        public CourseRatingsController(IUniversityInfoRepository universityInfoRepository, ICourseInfoRepository courseInfoRepository, ICourseRatingInfoRepository ratingInfoRepository, IMapper mapper)
        {
            _universityInfoRepository = universityInfoRepository ??
                throw new ArgumentException(nameof(universityInfoRepository));
            _courseInfoRepository = courseInfoRepository ??
                throw new ArgumentException(nameof(courseInfoRepository));
            _ratingInfoRepository = ratingInfoRepository ??
                throw new AbandonedMutexException(nameof(ratingInfoRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet(Name = "GetCourseRatings")]
        public async Task<IActionResult> GetCourseRatings(int universityId, int courseId, [FromQuery] int? amount)
        {
            if (universityId < 1 || courseId < 1 || amount < 1)
            {
                return BadRequest();
            }

            Course? course = await _courseInfoRepository.GetCourseForUniversityById(universityId, courseId, false);

            if (course is null)
            {
                return NotFound();
            }

            IEnumerable<CourseRating> ratings = await _ratingInfoRepository.GetRatingsForCourse(courseId, amount);

            return Ok(_mapper.Map<IEnumerable<CourseRatingDto>>(ratings));
        }


        [HttpGet("{ratingId}", Name = "GetCourseRatingById")]
        public async Task<IActionResult> GetCourseRatingById(int universityId, int courseId, int ratingId)
        {
            if (courseId < 1 || universityId < 1 || ratingId < 1)
            {
                return BadRequest();
            }

            Course? course = await _courseInfoRepository.GetCourseForUniversityById(universityId, courseId, false);

            if (course is null)
            {
                return NotFound();
            }

            CourseRating? rating = await _ratingInfoRepository.GetRatingForCourseById(courseId, ratingId);

            if (rating is null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<CourseRatingDto>(rating));
        }

        [HttpPost]
        public async Task<IActionResult> CreateCourseRating(int universityId, int courseId, [FromBody] CreateCourseRatingDto rating)
        {
            if (courseId < 1 || universityId < 1)
            {
                return BadRequest();
            }

            Course? course = await _courseInfoRepository.GetCourseForUniversityById(universityId, courseId, false);

            if (course is null)
            {
                return NotFound();
            }

            CourseRating finalRating = _mapper.Map<CourseRating>(rating);

            bool returnBool = await _ratingInfoRepository.AddRatingForCourse(course, finalRating);

            if (!returnBool)
            {
                return BadRequest();
            }

            var createdRating = _mapper.Map<CourseRatingDto>(finalRating);

            return CreatedAtRoute("GetCourseRatingById",
                new
                {
                    universityId = universityId,
                    courseId = courseId,
                    ratingId = createdRating.Id
                },
                createdRating);
        }

        [HttpDelete("{ratingId}")]
        public async Task<IActionResult> DeleteCourseRating(int universityId, int courseId, int ratingId)
        {
            if (universityId < 1)
            {
                return BadRequest();
            }

            Course? course = await _courseInfoRepository.GetCourseForUniversityById(universityId, courseId, false);

            if (course is null)
            {
                return NotFound();
            }

            bool returnBool = await _ratingInfoRepository.DeleteRating(course, ratingId);

            if (!returnBool)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpPut("{ratingId}")]
        public async Task<IActionResult> PutCourseRating(int universityId, int courseId, int ratingId, [FromBody] UpdateCourseRatingDto rating)
        {
            if (courseId < 1 || universityId < 1 || ratingId < 1)
            {
                return BadRequest();
            }

            Course? course = await _courseInfoRepository.GetCourseForUniversityById(universityId, courseId, false);

            if (course is null)
            {
                return NotFound();
            }

            CourseRating? currentRating = await _ratingInfoRepository.GetRatingForCourseById(courseId, ratingId);

            if (currentRating is null)
            {
                return NotFound();
            }

            _mapper.Map(rating, currentRating);

            await _ratingInfoRepository.SaveChanges();

            return Ok(_mapper.Map<CourseRatingDto>(currentRating));
        }

        [HttpPatch("{ratingId}")]
        public async Task<IActionResult> PatchCourseRating(int universityId, int courseId, int ratingId, [FromBody] JsonPatchDocument<UpdateCourseRatingDto> patchdoc)
        {
            if (courseId < 1 || universityId < 1 || ratingId < 1)
            {
                return BadRequest();
            }

            Course? course = await _courseInfoRepository.GetCourseForUniversityById(universityId, courseId, false);

            if (course is null)
            {
                return NotFound();
            }

            CourseRating? currentRating = await _ratingInfoRepository.GetRatingForCourseById(courseId, ratingId);

            if (currentRating is null)
            {
                return NotFound();
            }

            var ratingUpdate = _mapper.Map<UpdateCourseRatingDto>(currentRating);
            patchdoc.ApplyTo(ratingUpdate);

            TryValidateModel(ratingUpdate);

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            _mapper.Map(ratingUpdate, currentRating);

            await _ratingInfoRepository.SaveChanges();

            return Ok(_mapper.Map<CourseRatingDto>(currentRating));
        }
    }
}
