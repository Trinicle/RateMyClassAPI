using RateMyClass.API.Entities;

namespace RateMyClass.API.Services
{
    public interface IRatingInfoRepository
    {
        Task<IEnumerable<Rating>> GetRatingsForCourse(int courseId);
        Task<Rating?> GetRatingForCourseById(int courseId, int ratingId);
        Task<bool> DeleteRating(Course course, int ratingId);
        Task<bool> RatingExists(int rating);
        Task<bool> SaveChanges();
    }
}
