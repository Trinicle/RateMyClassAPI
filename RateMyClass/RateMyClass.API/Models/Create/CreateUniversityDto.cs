using System.ComponentModel.DataAnnotations;

namespace RateMyClass.API.Models.Create
{
    public class CreateUniversityDto
    {
        private string _name = string.Empty;
        private string _address = string.Empty;
        private string _city = string.Empty;
        private string _state = string.Empty;
        private string _zip = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Name { get => _name; set => _name = value.ToUpper(); } 
        [Required]
        [MaxLength(100)]
        public string Address { get => _address; set => _address = value.ToUpper(); } 
        [Required]
        [MaxLength(50)]
        public string City { get => _city; set => _city = value.ToUpper(); } 
        [Required]
        [MaxLength(50)]
        public string State { get => _state; set => _state = value.ToUpper(); }
        [Required]
        [StringLength(10, MinimumLength = 5)]
        public string Zip { get => _zip ; set => _zip = value.ToUpper(); }
        public string Website { get; set; } = "NOT AVAILABLE";
    }
}
