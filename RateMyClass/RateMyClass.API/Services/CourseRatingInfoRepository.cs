﻿using Microsoft.EntityFrameworkCore;
using RateMyClass.API.DbContexts;
using RateMyClass.API.Entities;

namespace RateMyClass.API.Services
{
    public class CourseRatingInfoRepository : ICourseRatingInfoRepository
    {
        private readonly UniversityInfoContext _context;

        public CourseRatingInfoRepository(UniversityInfoContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<bool> AddRatingForCourse(Course course, CourseRating rating)
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

        public async Task<CourseRating?> GetRatingForCourseById(int courseId, int ratingId)
        {
            return await _context.CourseRatings
                .Where(r => r.CourseId == courseId && r.Id == ratingId)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<CourseRating>> GetRatingsForCourse(int courseId, int? amount)
        {
            var query = _context.CourseRatings
                 .Where(r => r.CourseId == courseId);

            if (amount.HasValue)
            {
                query = query.Take(amount.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<bool> RatingExists(int ratingId)
        {
            var course = await _context.CourseRatings
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
