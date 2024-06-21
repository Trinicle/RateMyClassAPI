using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RateMyClass.API.Entities
{
    public class Class
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();

        [ForeignKey("UniversityId")]
        public University? University { get; set; }
        public int UniversityId { get; set; }

        public Class(string name) {
            Name = name;
        }
    }
}
