using API.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Primitives;

namespace API.Entities
{
    public class AppUser :IdentityUser<int>
    { 
        //public int Id { get; set; }
        //public string UserName { get; set; }
        //public byte[] PasswordHash { get; set; }       //removed in identity section
        //public byte[] PasswordSalt { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string KnownAs { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime LastActive { get; set; } = DateTime.UtcNow;
        public string Gender { get; set; }
        public string Introduction { get; set; }
        public string LookInfo { get; set; }
        public string Interest { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public List<Photo> Photos { get; set; } = new();      //relationship with another table called Photos

        public List<UserLike> LikedByUsers { get; set; }      //many to many relationship
        public List<UserLike> LikedUsers { get; set; }
        public List<Message> MessagesSent { get; set; }
        public List<Message> MessagesReceived { get; set; }
        public ICollection<AppUserRole> UserRoles { get; set; }

        //public int GetAge()
        //{               
        //    return DateOfBirth.CalculateAge();
        //}

    }
}
