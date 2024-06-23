using RateMyClass.API.Entities;

namespace RateMyClass.API.Services
{
    public interface IUniversityInfoRepository
    {
        Task<IEnumerable<University>> GetUniversitiesByName(string name, int amount);
        Task<University?> GetUniversityById(int id, bool includeCourses);
        Task<bool> UniversityExists(int universityId);
        Task<bool> AddCourseForUniversity(University university, Course course);
        Task<bool> AddUniversity(University university);
        Task<bool> DeleteUniversity(int id);
        Task<bool> SaveChanges();
    }
}
