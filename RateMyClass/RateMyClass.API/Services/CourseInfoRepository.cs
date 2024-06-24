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
            var course = await GetCourseForUniversityById(university.Id, courseId);

            if (course is null)
            {
                return false;
            }

            university.Courses.Remove(course);;

            await SaveChanges();

            return !await CourseExists(courseId);
        }

        public async Task<Course?> GetCourseForUniversityById(int universityId, int courseId)
        {
            return await _context.Courses
                .Where(c => c.UniversityId == universityId && c.Id == courseId)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Course>> GetCourseForUniversityByName(int universityId, string courseName, int amount)
        {
            string pattern = @"[%_\[\]^\\]";

            string nameSearchTerm = $"%{Regex.Replace(courseName ?? string.Empty, pattern, "\\$&")}%";

            return await _context.Courses
                .Where(c => c.UniversityId == universityId && EF.Functions.Like(c.Name, nameSearchTerm, "\\"))
                .Take(amount)
                .ToListAsync();
        }

        public async Task<IEnumerable<Course>> GetCoursesForUniversity(int universityId, int amount)
        {
            return await _context.Courses
                .Where(c => c.UniversityId == universityId)
                .Take(amount)
                .ToListAsync();
        }
        public async Task<bool> SaveChanges()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }
    }
}
