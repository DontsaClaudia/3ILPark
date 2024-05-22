using System.ComponentModel.DataAnnotations;

namespace _3ILPark.Models
{
    public class Mapping
    {
        [Key]
        public int Id { get; set; }
        public int Position { get; set; }
    }
}
