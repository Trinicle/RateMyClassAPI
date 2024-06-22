using RateMyClass.API.Entities;

namespace RateMyClass.API.Models.Get
{
    public class UniversityDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Zip { get; set; } = string.Empty;
        public string Website { get; set; } = string.Empty;
        public int NumberOfCourses
        {
            get
            {
                return Courses.Count;
            }
        }

        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}
