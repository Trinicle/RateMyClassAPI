using System.ComponentModel.DataAnnotations;

namespace RateMyClass.API.Models.Create
{
    public class CreateCourseDto
    {
        [Required]
        [MaxLength(10)]
        public string name { get; set; } = string.Empty;
    }
}
