using System.ComponentModel.DataAnnotations;

namespace RateMyClass.API.Models.Update
{
    public class UpdateCourseDto
    {
        [Required]
        [MaxLength(10)]
        public string Name { get; set; } = string.Empty;
    }
}
