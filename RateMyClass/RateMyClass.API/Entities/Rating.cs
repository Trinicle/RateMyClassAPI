using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RateMyClass.API.Entities
{
    public class Rating
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int Quality { get; set; }

        [Required]
        public int Difficulty { get; set; }

        [Required]
        public bool TakeAgain { get; set; }

        public DateTime Date { get; set; }

        [ForeignKey("ClassId")]
        public Class? Class { get; set; }
        public int ClassId { get; set; }
    }
}
