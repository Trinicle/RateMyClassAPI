using RateMyClass.API.Entities;

namespace RateMyClass.API.Services
{
    public interface IUniversityRatingInfoRepository
    {
        Task<bool> AddRatingForUniversity(University university, UniversityRating rating);
        Task<IEnumerable<UniversityRating>> GetRatingsForUniversity(int universityId, int amount);
        Task<UniversityRating?> GetRatingForUniversityById(int universityId, int ratingId);
        Task<bool> DeleteRating(University university, int ratingId);
        Task<bool> RatingExists(int rating);
        Task<bool> SaveChanges();
    }
}
