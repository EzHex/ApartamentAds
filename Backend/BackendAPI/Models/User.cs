using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackendAPI.Models;

public class User : IEntityTypeConfiguration<User>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Login { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public DateTime LastLogin { get; set; }
    
    public virtual List<Apartment> Apartments { get; set; }
    
    public User(string login, string email, string password, string name, DateTime lastLogin)
    {
        Login = login;
        Email = email;
        Password = password;
        Name = name;
        LastLogin = lastLogin;
    }

    public void Configure(EntityTypeBuilder<User> builder)
    {
        
    }
}