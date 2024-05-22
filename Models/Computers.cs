using _3ILPark.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _3ILPark.Models
{
    public class Computers
    {
        [Key]
        public int Id { get; set; }
        public string Manufacturer { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string Caption { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int TotalPhysicalMemory { get; set; }
        [ForeignKey("Mapping")]
        public int MappingId { get; set; }
        public Mapping? position { get; set; }
        [ForeignKey("Rooms")]
        public int? RoomId { get; set; }
        public Rooms? Room { get; set; }
        public DateTime Created_at { get; set; } = DateTime.Now;

        public Computers()
        {


        }

        public static implicit operator Computers(int v)
        {
            throw new NotImplementedException();
        }
    }
}
