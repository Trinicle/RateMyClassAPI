using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RateMyClass.API.Services;

namespace RateMyClass.API.Controllers
{
    [Route("api/universities/{universityId}/courses/{courseId}")]
    [ApiController]
    public class RatingsController : ControllerBase
    {
        private readonly ICourseInfoRepository _courseInfoRepository;
        private readonly IRatingInfoRepository _ratingInfoRepository;
        private readonly IMapper _mapper;

        public RatingsController(ICourseInfoRepository courseInfoRepository, IRatingInfoRepository ratingInfoRepository, IMapper mapper)
        {
            _courseInfoRepository = courseInfoRepository ?? 
                throw new ArgumentException(nameof(courseInfoRepository));
            _ratingInfoRepository = ratingInfoRepository ??
                throw new AbandonedMutexException(nameof(ratingInfoRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }
    }
}
