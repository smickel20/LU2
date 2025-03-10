using System.ComponentModel.DataAnnotations;

namespace LU2.Models
{
    public class Environment2D
    {

        public Guid Id { get; set; }
        public string Name { get; set; }
        public float MaxHeight { get; set; }
        public float MaxLength { get; set; }
        public Guid userId { get; set; }
    }
}
