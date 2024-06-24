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
    public class RatingsController : ControllerBase
    {
        private readonly IUniversityInfoRepository _universityInfoRepository;
        private readonly ICourseInfoRepository _courseInfoRepository;
        private readonly IRatingInfoRepository _ratingInfoRepository;
        private readonly IMapper _mapper;

        public RatingsController(IUniversityInfoRepository universityInfoRepository, ICourseInfoRepository courseInfoRepository, IRatingInfoRepository ratingInfoRepository, IMapper mapper)
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

        [HttpGet(Name = "GetRatings")]
        public async Task<IActionResult> GetRatings(int universityId, int courseId, [FromQuery] int amount = 10)
        {
            if (universityId < 1 || courseId < 1 || amount < 1)
            {
                return BadRequest();
            }

            Course? course = await _courseInfoRepository.GetCourseForUniversityById(universityId, courseId);

            if (course is null)
            {
                return NotFound();
            }

            IEnumerable<Rating> ratings = await _ratingInfoRepository.GetRatingsForCourse(courseId);

            return Ok(_mapper.Map<IEnumerable<RatingDto>>(ratings));
        }


        [HttpGet("{ratingId}", Name = "GetRatingById")]
        public async Task<IActionResult> GetRatingById(int universityId, int courseId, int ratingId)
        {
            if (courseId < 1 || universityId < 1 || ratingId < 1)
            {
                return BadRequest();
            }

            Course? course = await _courseInfoRepository.GetCourseForUniversityById(universityId, courseId);

            if (course is null)
            {
                return NotFound();
            }

            Rating? rating = await _ratingInfoRepository.GetRatingForCourseById(courseId, ratingId);

            if (rating is null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<RatingDto>(rating));
        }

        [HttpPost]
        public async Task<IActionResult> CreateRating(int universityId, int courseId, [FromBody] CreateRatingDto rating)
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

            Rating finalRating = _mapper.Map<Rating>(rating);

            bool returnBool = await _ratingInfoRepository.AddRatingForCourse(course, finalRating);

            if (!returnBool)
            {
                return BadRequest();
            }

            var createdRating = _mapper.Map<RatingDto>(finalRating);

            return CreatedAtRoute("GetRatingById",
                new
                {
                    universityId = universityId,
                    courseId = courseId,
                    ratingId = createdRating.Id
                },
                createdRating);
        }

        [HttpDelete("{ratingId}")]
        public async Task<IActionResult> DeleteRating(int universityId, int courseId, int ratingId)
        {
            if (universityId < 1)
            {
                return BadRequest();
            }

            Course? course = await _courseInfoRepository.GetCourseForUniversityById(universityId, courseId);

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
        public async Task<IActionResult> PutRating(int universityId, int courseId, int ratingId, [FromBody] UpdateRatingdto rating)
        {
            if (courseId < 1 || universityId < 1 || ratingId < 1)
            {
                return BadRequest();
            }

            Course? course = await _courseInfoRepository.GetCourseForUniversityById(universityId, courseId);

            if (course is null)
            {
                return NotFound();
            }

            Rating? currentRating = await _ratingInfoRepository.GetRatingForCourseById(courseId, ratingId);

            if (currentRating is null)
            {
                return NotFound();
            }

            _mapper.Map(rating, currentRating);

            await _ratingInfoRepository.SaveChanges();

            return Ok(_mapper.Map<RatingDto>(currentRating));
        }

        [HttpPatch("{ratingId}")]
        public async Task<IActionResult> PatchRating(int universityId, int courseId, int ratingId, [FromBody] JsonPatchDocument<UpdateRatingdto> patchdoc)
        {
            if (courseId < 1 || universityId < 1 || ratingId < 1)
            {
                return BadRequest();
            }

            Course? course = await _courseInfoRepository.GetCourseForUniversityById(universityId, courseId);

            if (course is null)
            {
                return NotFound();
            }

            Rating? currentRating = await _ratingInfoRepository.GetRatingForCourseById(courseId, ratingId);

            if (currentRating is null)
            {
                return NotFound();
            }

            var ratingUpdate = _mapper.Map<UpdateRatingdto>(currentRating);
            patchdoc.ApplyTo(ratingUpdate);

            TryValidateModel(ratingUpdate);

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            _mapper.Map(ratingUpdate, currentRating);

            await _ratingInfoRepository.SaveChanges();

            return Ok(_mapper.Map<RatingDto>(currentRating));
        }
    }
}
