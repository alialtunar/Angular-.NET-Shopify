using Microsoft.AspNetCore.Identity;

namespace ServerApp.Models
{
    public class Role:IdentityRole<int>
    {
        public Role() : base() {}
    public Role(string roleName) : base(roleName) {}
    }
}