using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RateMyClass.API.DbContexts;
using RateMyClass.API.Entities;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace RateMyClass.API.Services
{
    public class UniversityInfoRepository : IUniversityInfoRepository
    {
        private readonly UniversityInfoContext _context;
        private readonly IMapper _mapper;

        public UniversityInfoRepository(UniversityInfoContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<bool> AddCourseForUniversity(University university, Course course)
        {
            university.Courses.Add(course);

            await SaveChanges();

            return true;
        }

        public async Task<bool> AddUniversity(University university)
        {
            await _context.Universities.AddAsync(university);

            await SaveChanges();

            var newUniversity = _mapper.Map<Models.Get.UniversityDto>(university);

            if (newUniversity is not null)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteUniversity(int id)
        {
            var university = await UniversityExists(id);

            if (university is null)
            {
                return false;
            }

            await _context.Universities
                .Where(u => u.Id == id)
                .ExecuteDeleteAsync();

            return true;
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
