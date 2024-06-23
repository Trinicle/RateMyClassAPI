using System.ComponentModel.DataAnnotations;

namespace RateMyClass.API.Models.Update
{
    public class UpdateRatingdto
    {
        [MaxLength(200)]
        public string Description { get; set; } = string.Empty;

        [Range(1, 5)]
        public int Quality { get; set; } = 1;

        [Range(1, 5)]
        public int Difficulty { get; set; } = 1;

        public bool TakeAgain { get; set; } = false;
    }
}
