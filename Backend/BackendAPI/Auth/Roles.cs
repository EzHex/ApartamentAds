using BackendAPI.Models;

namespace BackendAPI.Auth;

public static class Roles
{
    public const string Admin = nameof(Admin);
    public const string User = nameof(ApartmentUser);
    
    public static readonly IReadOnlyCollection<string> All = new[] { Admin, User };
}