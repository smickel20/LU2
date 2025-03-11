using System.ComponentModel.DataAnnotations;

namespace LU2.Models
{
    public class Environment2D
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(25, MinimumLength = 1, ErrorMessage = "De naam moet tussen 1 en 25 karakters lang zijn.")]
        public string Name { get; set; }

        [Range(1, float.MaxValue, ErrorMessage = "MaxHeight moet groter zijn dan 0.")]
        public float MaxHeight { get; set; }

        [Range(1, float.MaxValue, ErrorMessage = "MaxLength moet groter zijn dan 0.")]
        public float MaxLength { get; set; }

        [Required]
        public Guid userId { get; set; }
    }
}
