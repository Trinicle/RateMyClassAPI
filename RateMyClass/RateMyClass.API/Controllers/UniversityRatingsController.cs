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
    [Route("api/universities/{universityId}/ratings")]
    [ApiController]
    public class UniversityRatingsController : ControllerBase
    {
        private readonly IUniversityInfoRepository _universityInfoRepository;
        private readonly IUniversityRatingInfoRepository _ratingInfoRepository;
        private readonly IMapper _mapper;

        public UniversityRatingsController(
            IUniversityRatingInfoRepository universityRatingInfoRepository,
            IUniversityInfoRepository universityInfoRepository,
            IMapper mapper)
        {
            _universityInfoRepository = universityInfoRepository ?? throw new ArgumentException(nameof(universityInfoRepository));
            _ratingInfoRepository = universityRatingInfoRepository ?? throw new ArgumentException(nameof(universityRatingInfoRepository));
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
        }


        [HttpGet(Name = "GetUniversityRatings")]
        public async Task<IActionResult> GetUniversityRatings(int universityId, [FromQuery] int amount = 10)
        {
            if (universityId < 1 || amount < 1)
            {
                return BadRequest();
            }

            IEnumerable<UniversityRating> ratings = await _ratingInfoRepository.GetRatingsForUniversity(universityId, amount);

            return Ok(_mapper.Map<IEnumerable<UniversityRatingDto>>(ratings));
        }


        [HttpGet("{ratingId}", Name = "GetUniversityRatingById")]
        public async Task<IActionResult> GetUniversityRatingById(int universityId, int ratingId)
        {
            if (universityId < 1 || ratingId < 1)
            {
                return BadRequest();
            }

            UniversityRating? rating = await _ratingInfoRepository.GetRatingForUniversityById(universityId, ratingId);

            if (rating is null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<UniversityRatingDto>(rating));
        }

        [HttpPost]
        public async Task<IActionResult> CreateUniversityRating(int universityId, [FromBody] CreateUniversityRatingDto rating)
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

            UniversityRating finalRating = _mapper.Map<UniversityRating>(rating);

            bool returnBool = await _ratingInfoRepository.AddRatingForUniversity(university, finalRating);

            if (!returnBool)
            {
                return BadRequest();
            }

            var createdRating = _mapper.Map<UniversityRatingDto>(finalRating);

            return CreatedAtRoute("GetUniversityRatingById",
                new
                {
                    universityId = universityId,
                    ratingId = createdRating.Id
                },
                createdRating);
        }

        [HttpDelete("{ratingId}")]
        public async Task<IActionResult> DeleteUniversityRating(int universityId, int ratingId)
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

            bool returnBool = await _ratingInfoRepository.DeleteRating(university, ratingId);

            if (!returnBool)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpPut("{ratingId}")]
        public async Task<IActionResult> PutUniversityRating(int universityId, int ratingId, [FromBody] UpdateUniversityRatingDto rating)
        {
            if (universityId < 1 || ratingId < 1)
            {
                return BadRequest();
            }

            University? university = await _universityInfoRepository.GetUniversityById(universityId, false);

            if (university is null)
            {
                return NotFound();
            }

            UniversityRating? currentRating = await _ratingInfoRepository.GetRatingForUniversityById(universityId, ratingId);

            if (currentRating is null)
            {
                return NotFound();
            }

            _mapper.Map(rating, currentRating);

            await _ratingInfoRepository.SaveChanges();

            return Ok(_mapper.Map<UniversityRatingDto>(currentRating));
        }

        [HttpPatch("{ratingId}")]
        public async Task<IActionResult> PatchUniversityRating(int universityId, int ratingId, [FromBody] JsonPatchDocument<UpdateUniversityRatingDto> patchdoc)
        {
            if (universityId < 1 || ratingId < 1)
            {
                return BadRequest();
            }

            University? university = await _universityInfoRepository.GetUniversityById(universityId, false);

            if (university is null)
            {
                return NotFound();
            }

            UniversityRating? currentRating = await _ratingInfoRepository.GetRatingForUniversityById(universityId, ratingId);

            if (currentRating is null)
            {
                return NotFound();
            }

            var ratingUpdate = _mapper.Map<UpdateUniversityRatingDto>(currentRating);
            patchdoc.ApplyTo(ratingUpdate);

            TryValidateModel(ratingUpdate);

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            _mapper.Map(ratingUpdate, currentRating);

            await _ratingInfoRepository.SaveChanges();

            return Ok(_mapper.Map<UniversityRatingDto>(currentRating));
        }
    }
}
