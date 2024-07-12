using RateMyClass.API.Entities;

namespace RateMyClass.API.Services
{
    public interface ICourseRatingInfoRepository
    {
        Task<bool> AddRatingForCourse(Course course, CourseRating rating);
        Task<IEnumerable<CourseRating>> GetRatingsForCourse(int courseId, int? amount);
        Task<CourseRating?> GetRatingForCourseById(int courseId, int ratingId);
        Task<bool> DeleteRating(Course course, int ratingId);
        Task<bool> RatingExists(int rating);
        Task<bool> SaveChanges();
    }
}
