using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _3ILPark.Models
{
    public class Rooms
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        [ForeignKey("Parks")]
        public int? ParksId { get; set; }
        public Parks? Park { get; set; }
        [ForeignKey("Rules")]
        public int? RulesId { get; set; }
        public Rules? Rule { get; set; }
        public DateTime? createDate { get; set; }


        public Rooms()
        {
            Id = 0;
        }

        public Rooms(int id, string name, DateTime createdate)
        {
            Id = id;
            Name = name;
            createDate = createdate;
        }
    }
}
