using AutoMapper;

namespace RateMyClass.API.Profiles
{
    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            CreateMap<Entities.Course, Models.Get.CourseDto>();
            CreateMap<Models.Create.CreateCourseDto, Entities.Course>();
            CreateMap<Entities.Course, Models.Get.CourseWithoutRatingsDto>();
        }
    }
}
