using Microsoft.EntityFrameworkCore;
using RateMyClass.API.DbContexts;
using RateMyClass.API.Entities;

namespace RateMyClass.API.Services
{
    public class RatingInfoRepository : IRatingInfoRepository
    {
        private readonly UniversityInfoContext _context;

        public RatingInfoRepository(UniversityInfoContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<bool> AddRatingForCourse(Course course, Rating rating)
        {
            course.Ratings.Add(rating);

            await SaveChanges();

            return await RatingExists(rating.Id);
        }

        public async Task<bool> DeleteRating(Course course, int ratingId)
        {
            var rating = await GetRatingForCourseById(course.Id, ratingId);

            if (rating is null)
            {
                return false;
            }

            course.Ratings.Remove(rating);

            await SaveChanges();

            return !await RatingExists(ratingId);
        }

        public async Task<Rating?> GetRatingForCourseById(int courseId, int ratingId)
        {
            return await _context.Ratings
                .Where(r => r.CourseId == courseId && r.Id == ratingId)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Rating>> GetRatingsForCourse(int courseId)
        {
            return await _context.Ratings
                .Where(r => r.CourseId == courseId)
                .ToListAsync();
        }

        public async Task<bool> RatingExists(int ratingId)
        {
            var course = await _context.Ratings
                .Where (r => r.Id == ratingId)
                .FirstOrDefaultAsync();

            return course is not null ? true : false;
        }

        public async Task<bool> SaveChanges()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }
    }
}
