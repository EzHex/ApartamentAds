using Microsoft.AspNetCore.Identity;

namespace BackendAPI.Models;

public sealed class ApartmentUser : IdentityUser
{
    public DateTime LastLogin { get; set; }
    
    public ApartmentUser(string userName, string email)
    {
        UserName = userName;
        Email = email;
    }
}