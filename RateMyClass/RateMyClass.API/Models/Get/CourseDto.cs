using RateMyClass.API.Entities;

namespace RateMyClass.API.Models.Get
{
    public class CourseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int NumberOfRatings
        {
            get
            {
                return Ratings.Count;
            }
        }

        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
    }
}
