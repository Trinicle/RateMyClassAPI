using Microsoft.EntityFrameworkCore;
using RateMyClass.API.DbContexts;
using RateMyClass.API.Entities;
using System.Text.RegularExpressions;

namespace RateMyClass.API.Services
{
    public class UniversityInfoRepository : IUniversityInfoRepository
    {
        private readonly UniversityInfoContext _context;

        public UniversityInfoRepository(UniversityInfoContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task AddCourseForUniversity(int UniversityId, Course course)
        {
            var university = await UniversityExists(UniversityId);
            if (university is not null)
            {
                university.Courses.Add(course);
            }
        }

        public void DeleteCourse(Course course)
        {
            throw new NotImplementedException();
        }

        public async Task<Course?> GetCourseForUniversity(int universityId, int courseId)
        {
            return await _context.Courses
                .Where(c => c.UniversityId == universityId && c.Id == courseId)
                .FirstOrDefaultAsync();
        }

        public Task<IEnumerable<Course>> GetCoursesForUniversity(int universityId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<University>> GetUniversitiesByName(string name, int amount)
        {
            string pattern = @"[%_\[\]^\\]";

            string nameSearchTerm = $"%{Regex.Replace(name ?? string.Empty, pattern, "\\$&")}%";

            return await _context.Universities
                .Where(u => EF.Functions.Like(u.Name, nameSearchTerm, "\\"))
                .Take(amount)
                .ToListAsync();
        }

        public async Task<bool> SaveChanges()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }

        public async Task<University?> UniversityExists(int universityId)
        {
            return await _context.Universities
                .Where(u => u.Id == universityId)
                .FirstOrDefaultAsync();
        }
    }
}
