using RateMyClass.API.Entities;

namespace RateMyClass.API.Services
{
    public interface ICourseInfoRepository
    {
        Task<bool> AddCourseForUniversity(University university, Course course);
        Task<IEnumerable<Course>> GetCoursesForUniversity(int universityId, int? amount, bool includeRatings);
        Task<Course?> GetCourseForUniversityById(int universityId, int courseId, bool includeRatings);
        Task<IEnumerable<Course>> GetCourseForUniversityByName(int universityId, string courseName, int? amount, bool includeRatings);
        Task<bool> DeleteCourse(University university, int courseId);
        Task<bool> CourseExists(int courseId);
        Task<bool> SaveChanges();
    }
}
