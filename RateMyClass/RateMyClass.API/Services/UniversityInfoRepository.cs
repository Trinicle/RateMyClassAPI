using AutoMapper;
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

        public async Task<bool> AddUniversity(University university)
        {
            await _context.Universities.AddAsync(university);

            await SaveChanges();

            return await UniversityExists(university.Id);
        }

        public async Task<bool> DeleteUniversity(int id)
        {
            var university = await GetUniversityById(id, false);

            if (university is null)
            {
                return false;
            }

            _context.Universities.Remove(university);

            await SaveChanges();

            return !await UniversityExists(id);
        }

        public async Task<IEnumerable<University>> GetUniversities(int amount)
        {
            return await _context.Universities
               .Take(amount)
               .ToListAsync();
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

        public async Task<University?> GetUniversityById(int id, bool includeCourses)
        {
            if (includeCourses)
            {
                return await _context.Universities
                    .Include (c => c.Courses)
                    .Where(u => u.Id == id)
                    .FirstOrDefaultAsync();
            }

            return await _context.Universities
                    .Where(u => u.Id == id)
                    .FirstOrDefaultAsync();
        }

        public async Task<bool> SaveChanges()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }

        public async Task<bool> UniversityExists(int universityId)
        {
            var university = await _context.Universities
                                .Where(u => u.Id == universityId)
                                .FirstOrDefaultAsync();

            return university is not null ? true : false;
        }
    }
}
