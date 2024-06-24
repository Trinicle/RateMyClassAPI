using System.ComponentModel.DataAnnotations;

namespace RateMyClass.API.Models.Update
{
    public class UpdateUniversityDto
    {
        private string _Name = string.Empty;
        private string _Address = string.Empty;
        private string _City = string.Empty;
        private string _State = string.Empty;
        private string _Zip = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Name { get => _Name; set => _Name = value.ToUpper(); }

        [Required]
        [MaxLength(100)]
        public string Address { get => _Address; set => _Address = value.ToUpper(); }

        [Required]
        [MaxLength(50)]
        public string City { get => _City; set => _City = value.ToUpper(); }

        [Required]
        [MaxLength(50)]
        public string State { get => _State; set => _State= value.ToUpper(); }

        [Required]
        [StringLength(10, MinimumLength = 5)]
        public string Zip { get => _Zip; set => _Zip = value.ToUpper(); }

        public string Website { get; set; } = "NOT AVAILABLE";
    }
}
