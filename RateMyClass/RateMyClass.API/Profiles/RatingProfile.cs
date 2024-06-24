using AutoMapper;

namespace RateMyClass.API.Profiles
{
    public class RatingProfile : Profile
    {
        public RatingProfile() 
        {
            CreateMap<Entities.Rating, Models.Get.RatingDto>();
            CreateMap<Models.Create.CreateRatingDto, Entities.Rating>();
            CreateMap<Entities.Rating, Models.Update.UpdateRatingdto>();
            CreateMap<Models.Update.UpdateRatingdto, Entities.Rating>();
            CreateMap<Entities.Rating, Models.Update.UpdateCourseDto>();
        }
    }
}
