using RateMyClass.API.Entities;

namespace RateMyClass.API.Services
{
    public interface ICourseInfoRepository
    {
        Task<IEnumerable<Course>> GetCourse(int Id);

    }
}
