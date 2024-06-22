using RateMyClass.API.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

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
