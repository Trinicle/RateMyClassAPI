using RateMyClass.API.Entities;

namespace RateMyClass.API.Services
{
    public interface ICourseInfoRepository
    {
        Task<IEnumerable<Course>> GetCoursesForUniversity(int universityId);
        Task<Course?> GetCourseForUniversityById(int universityId, int courseId);
        Task<IEnumerable<Course>> GetCourseForUniversityByName(int universityId, string courseName, int amount);
        Task<bool> DeleteCourse(University university, int courseId);
        Task<bool> CourseExists(int courseId);
        Task<bool> SaveChanges();
    }
}
