using Microsoft.EntityFrameworkCore;
using RateMyClass.API.DbContexts;
using RateMyClass.API.Entities;
using System.Text.RegularExpressions;

namespace RateMyClass.API.Services
{
    public class CourseInfoRepository : ICourseInfoRepository
    {
        private readonly UniversityInfoContext _context;

        public CourseInfoRepository(UniversityInfoContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> AddCourseForUniversity(University university, Course course)
        {
            university.Courses.Add(course);

            await SaveChanges();

            return await CourseExists(course.Id);
        }

        public async Task<bool> CourseExists(int courseId)
        {
            var course = await _context.Courses
                            .Where(c => c.Id == courseId)
                            .FirstOrDefaultAsync();

            return course is not null ? true : false;
        }

        public async Task<bool> DeleteCourse(University university, int courseId)
        {
            var course = await GetCourseForUniversityById(university.Id, courseId, false);

            if (course is null)
            {
                return false;
            }

            university.Courses.Remove(course);;

            await SaveChanges();

            return !await CourseExists(courseId);
        }

        public async Task<Course?> GetCourseForUniversityById(int universityId, int courseId, bool includeCourses)
        {
            if (includeCourses)
            {
                return await _context.Courses
                    .Where(c => c.UniversityId == universityId && c.Id == courseId)
                    .Include(r => r.Ratings)
                    .FirstOrDefaultAsync();
            }
            return await _context.Courses
                .Where(c => c.UniversityId == universityId && c.Id == courseId)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Course>> GetCourseForUniversityByName(int universityId, string courseName, int? amount, bool includeRatings)
        {
            string pattern = @"[%_\[\]^\\]";

            string nameSearchTerm = $"%{Regex.Replace(courseName ?? string.Empty, pattern, "\\$&")}%";
            var query = _context.Courses
                .Where(c => c.UniversityId == universityId && EF.Functions.Like(c.Name, nameSearchTerm, "\\"));
            
            if (amount.HasValue)
            {
                query = query.Take(amount.Value);
            }

            if (includeRatings)
            {
                query = query.Include(r => r.Ratings);
            }
 
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Course>> GetCoursesForUniversity(int universityId, int? amount, bool includeRatings)
        {
            var query = _context.Courses.Where(c => c.UniversityId == universityId);

            if (amount.HasValue)
            {
                query = query.Take(amount.Value);
            }

            if (includeRatings)
            {
                query = query.Include(r => r.Ratings);
            }

            return await query.ToListAsync();
        }
        public async Task<bool> SaveChanges()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }
    }
}
