using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RateMyClass.API.Entities
{
    public class CourseRating
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Range(1, 5)]
        public int Quality { get; set; }

        [Required]
        [Range(1, 5)]
        public int Difficulty { get; set; }

        [Required]
        public bool TakeAgain { get; set; }

        public DateTime Date { get; set; }

        [ForeignKey("CourseId")]
        public Course? Course { get; set; }
        public int CourseId { get; set; }
    }
}
