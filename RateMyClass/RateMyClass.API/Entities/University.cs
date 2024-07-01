using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RateMyClass.API.Entities
{
    public class University
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
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
        public ICollection<Course> Courses { get; set; } = new List<Course>();
        public ICollection<UniversityRating> Ratings { get; set; } = new List<UniversityRating>();
    }
}
