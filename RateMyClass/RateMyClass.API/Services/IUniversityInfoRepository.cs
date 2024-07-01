using RateMyClass.API.Entities;

namespace RateMyClass.API.Services
{
    public interface IUniversityInfoRepository
    {
        Task<IEnumerable<University>> GetUniversities(int amount);
        Task<IEnumerable<University>> GetUniversitiesByName(string name, int amount);
        Task<University?> GetUniversityById(int id, bool includeLists);
        Task<bool> UniversityExists(int universityId);
        Task<bool> AddUniversity(University university);
        Task<bool> DeleteUniversity(int id);
        Task<bool> SaveChanges();
    }
}
