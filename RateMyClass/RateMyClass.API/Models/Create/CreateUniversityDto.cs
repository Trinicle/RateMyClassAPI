using System.ComponentModel.DataAnnotations;

namespace RateMyClass.API.Models.Create
{
    public class CreateUniversityDto
    {
        [Required]
        [MaxLength(50)]
        public string name { get; set; } = string.Empty;
        [Required]
        [MaxLength(100)]
        public string address { get; set; } = string.Empty;
        [Required]
        [MaxLength(50)]
        public string city { get; set; } = string.Empty;
        [Required]
        [MaxLength(50)]
        public string state { get; set; } = string.Empty;
        [Required]
        [StringLength(10, MinimumLength = 5)]
        public string zip { get; set; } = string.Empty;
        public string website { get; set; } = "NOT AVAILABLE";
    }
}
