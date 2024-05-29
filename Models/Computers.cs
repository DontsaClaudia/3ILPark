using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using _3ILPark.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace _3ILPark_API.Models
{
    [Index(nameof(Manufacturer), nameof(Model), nameof(Caption), nameof(Name), nameof(TotalPhysicalMemory), nameof(RoomId), IsUnique = true)]
    public class Computers
    {
        [Key]
        public int Id { get; set; }
        public string Manufacturer { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string Caption { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int TotalPhysicalMemory { get; set; }
        [ForeignKey("Rooms")]
        public int? RoomId { get; set; }
        public Rooms? Room { get; set; }
        public DateTime Created_at { get; set; } = DateTime.Now;
    }
}