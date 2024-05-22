using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _3ILPark.Models
{
    public class Users
    {
        [Key]
        public int Id { get; set; }
        public string Firstname { get; set; } = string.Empty;
        public string Lastname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Format("@.com");
        public string Password { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        [ForeignKey("Rules")]
        public int? Rulesid { get; set; }
        public Rules? Rules { get; set; }
        public bool IsEmailConfirmed { get; set; } = false;
        public bool IsPhoneConfirmed { get; set; } = false;
        public bool IsConnected { get; set; } = false;
        public DateTime Created { get; set; } = DateTime.Now;

        public Users()
        {
            Id = 0;
            Email = string.Empty;
            Password = string.Empty;
            PhoneNumber = string.Empty;
        }

        public Users(int id, string firstname, string lastname, string email, string password, string phonenumber, string username, Rules rules, bool isconnected, DateTime created)
        {
            Id = id;
            Firstname = firstname;
            Lastname = lastname;
            Email = email;
            Password = password;
            PhoneNumber = phonenumber;
            Username = username;
            Rules = rules;
            IsConnected = isconnected;
            Created = created;


        }
    }
}
