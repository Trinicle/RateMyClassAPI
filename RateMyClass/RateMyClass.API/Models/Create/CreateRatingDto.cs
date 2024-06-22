using System.ComponentModel.DataAnnotations;

namespace RateMyClass.API.Models.Create
{
    public class CreateRatingDto
    {
        [Required]
        [MaxLength(200)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Range(1, 5)]
        public int Quality { get; set; }

        [Required]
        [Range(1, 5)]
        public int Difficulty { get; set; }

        [Required]
        public bool TakeAgain { get; set; }

        public DateTime Date
        {
            get
            {
                return DateTime.Now;
            }
        }
    }
}
