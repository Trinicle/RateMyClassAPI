using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RateMyClass.API.Entities
{
    public class Course
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(10)]
        public string Name { get; set; } = string.Empty;
        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();

        [ForeignKey("UniversityId")]
        public University? University { get; set; }
        public int UniversityId { get; set; }
    }
}
