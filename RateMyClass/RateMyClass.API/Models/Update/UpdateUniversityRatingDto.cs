using System.ComponentModel.DataAnnotations;

namespace RateMyClass.API.Models.Update
{
    public class UpdateUniversityRatingDto
    {
        [Range(1, 5)]
        public int Quality { get; set; }
        [Range(1, 5)]
        public int Location { get; set; }
        [Range(1, 5)]
        public int Opportunities { get; set; }
        [Range(1, 5)]
        public int Facilities { get; set; }
        [Range(1, 5)]
        public int Internet { get; set; }
        [Range(1, 5)]
        public int Food { get; set; }
        [Range(1, 5)]
        public int Clubs { get; set; }
        [Range(1, 5)]
        public int Social { get; set; }
        [Range(1, 5)]
        public int Happiness { get; set; }
        [Range(1, 5)]
        public int Safety { get; set; }
        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;
    }
}
