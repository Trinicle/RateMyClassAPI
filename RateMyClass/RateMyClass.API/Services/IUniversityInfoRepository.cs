using RateMyClass.API.Entities;

namespace RateMyClass.API.Services
{
    public interface IUniversityInfoRepository
    {
        Task<IEnumerable<University>> GetUniversitiesByName(string name, int amount);
        Task<University?> UniversityExists(int universityId);
        Task<IEnumerable<Course>> GetCoursesForUniversity(int universityId);
        Task<Course?> GetCourseForUniversity(int universityId, int courseId);
        Task AddCourseForUniversity(int UniversityId, Course course);
        void DeleteCourse(Course course);
        Task<bool> SaveChanges();
    }
}
