using System.ComponentModel.DataAnnotations;

namespace RateMyClass.API.Models.Create
{
    public class CreateUniversityDto
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [MaxLength(100)]
        public string Address { get; set; } = string.Empty;
        [Required]
        [MaxLength(50)]
        public string City { get; set; } = string.Empty;
        [Required]
        [MaxLength(50)]
        public string State { get; set; } = string.Empty;
        [Required]
        [StringLength(10, MinimumLength = 5)]
        public string Zip { get; set; } = string.Empty;
        public string Website { get; set; } = "NOT AVAILABLE";
    }
}
