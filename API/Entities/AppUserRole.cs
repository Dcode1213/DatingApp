using Microsoft.AspNetCore.Identity;

namespace API.Entities
{
    public class AppUserRole : IdentityUserRole<int>  //int as id property
    {
                //it's managing join tables
        public AppUser User { get; set; }     
        public AppRole Role { get; set; } 
    }
}
