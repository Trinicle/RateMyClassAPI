using System.ComponentModel.DataAnnotations;

namespace RateMyClass.API.Models.Create
{
    public class CreateUniversityRatingDto
    {
        [Required]
        [Range(1, 5)]
        public int Quality { get; set; }
        [Required]
        [Range(1, 5)]
        public int Location { get; set; }
        [Required]
        [Range(1, 5)]
        public int Opportunities { get; set; }
        [Required]
        [Range(1, 5)]
        public int Facilities { get; set; }
        [Required]
        [Range(1, 5)]
        public int Internet { get; set; }
        [Required]
        [Range(1, 5)]
        public int Food { get; set; }
        [Required]
        [Range(1, 5)]
        public int Clubs { get; set; }
        [Required]
        [Range(1, 5)]
        public int Social { get; set; }
        [Required]
        [Range(1, 5)]
        public int Happiness { get; set; }
        [Required]
        [Range(1, 5)]
        public int Safety { get; set; }
        [Required]
        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;
        public DateTime Date
        {
            get
            {
                return DateTime.Now;
            }
        }
    }
}
