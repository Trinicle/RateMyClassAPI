using AutoMapper;

namespace RateMyClass.API.Profiles
{
    public class UniversityProfile : Profile
    {
        public UniversityProfile() {
            CreateMap<Entities.University, Models.Get.UniversityWithoutCoursesDto>();
            CreateMap<Entities.University, Models.Get.UniversityDto>();
        }
    }
}
