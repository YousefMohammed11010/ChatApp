using ChatApp.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsOnline { get; set; }

        public ICollection<Message> SentMessages { get; set; } = new List<Message>();
        public ICollection<Message> ReceivedMessages { get; set; } = new List<Message>();

        [NotMapped]
        public ICollection<Contact> Contacts { get; set; }

        public List<RefreshToken>? RefreshTokens { get; set; }
    }
}
