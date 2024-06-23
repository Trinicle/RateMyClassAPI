using AutoMapper;

namespace RateMyClass.API.Profiles
{
    public class UniversityProfile : Profile
    {
        public UniversityProfile() 
        {
            CreateMap<Entities.University, Models.Get.UniversityWithoutCoursesDto>();
            CreateMap<Entities.University, Models.Get.UniversityDto>();
            CreateMap<Models.Create.CreateUniversityDto, Entities.University>();
            CreateMap<Models.Update.UpdateUniversityDto, Entities.University>();
            CreateMap<Entities.University, Models.Update.UpdateUniversityDto>();
        }
    }
}
