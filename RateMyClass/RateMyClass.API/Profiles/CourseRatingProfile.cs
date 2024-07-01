using AutoMapper;

namespace RateMyClass.API.Profiles
{
    public class CourseRatingProfile : Profile
    {
        public CourseRatingProfile()
        {
            CreateMap<Entities.CourseRating, Models.Get.CourseRatingDto>();
            CreateMap<Models.Create.CreateCourseRatingDto, Entities.CourseRating>();
            CreateMap<Entities.CourseRating, Models.Update.UpdateCourseRatingDto>();
            CreateMap<Models.Update.UpdateCourseRatingDto, Entities.CourseRating>();
            CreateMap<Entities.CourseRating, Models.Update.UpdateCourseRatingDto>();
        }
    }
}
