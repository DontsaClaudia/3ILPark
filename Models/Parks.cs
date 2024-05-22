using System.ComponentModel.DataAnnotations;

namespace _3ILPark.Models
{
    public class Parks
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Adress { get; set; }
        public DateTime? CreateDate { get; set; }

        public Parks()
        {

        }


    }
}
