namespace _3ILPark.Models
{
    public class Networks
    {
        public int NetworkId { get; set; }
        public int ComputerId { get; set; }
        public string Name { get; set; }
        public string? MACAddress { get; set; }
        public string? Description { get; set; }
        public string[] IPAddress { get; set; }
        public string[] IPSubnet { get; set; }
        public string[] DNSServerSearchOrder { get; set; }
        public string DHCPServer { get; set; }
        public string DefaultIPGateway { get; set; }
        public bool DHCPEnabled { get; set; }
        public int Speed { get; set; }
    }
}