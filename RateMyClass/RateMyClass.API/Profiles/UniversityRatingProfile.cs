using AutoMapper;

namespace RateMyClass.API.Profiles
{
    public class UniversityRatingProfile : Profile
    {
        public UniversityRatingProfile()
        {
            CreateMap<Entities.UniversityRating, Models.Get.UniversityRatingDto>();
            CreateMap<Models.Create.CreateUniversityRatingDto, Entities.UniversityRating>();
            CreateMap<Entities.UniversityRating, Models.Update.UpdateUniversityRatingDto>();
            CreateMap<Models.Update.UpdateUniversityRatingDto, Entities.UniversityRating>();
            CreateMap<Entities.UniversityRating, Models.Update.UpdateUniversityRatingDto>();
        }
    }
}
