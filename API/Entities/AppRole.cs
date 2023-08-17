using Microsoft.AspNetCore.Identity;

namespace API.Entities
{
    public class AppRole : IdentityRole<int>         //int used for id property
    {
        public ICollection<AppUserRole> UserRoles { get; set; }
    }
}
