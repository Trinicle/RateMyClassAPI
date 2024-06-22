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
        private readonly IMapper _mapper;

        public RatingsController(ICourseInfoRepository courseInfoRepository, IMapper mapper)
        {
            _courseInfoRepository = courseInfoRepository;
            _mapper = mapper;
        }

        
    }
}
