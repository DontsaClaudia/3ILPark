using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _3ILPark.Models
{
    public class Network
    {
        [Key]
        public int NetworkId { get; set; }
        [ForeignKey("Computer")]
        public int ComputerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string MACAddress { get; set; } = string.Empty;
        public int Speed { get; set; }
        public string DefaultIPGateway { get; set; } = string.Empty;
        public Network() { }

    }
}
