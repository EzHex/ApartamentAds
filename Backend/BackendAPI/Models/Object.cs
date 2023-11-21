using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BackendAPI.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackendAPI.Models;

public class Object : IEntityTypeConfiguration<Object>, IUserOwnedResource
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Name { get; set; }
    public double Grade { get; set; }
    public string Description { get; set; }
    public string Image { get; set; }

    public Object(string name, double grade, string description, string image, int roomId)
    {
        Name = name;
        Grade = grade;
        Description = description;
        Image = image;
        RoomId = roomId;
    }

    public Object() { }

    public string UserId { get; set; }
    public ApartmentUser User { get; set; }
    
    public int RoomId { get; set; }
    public virtual Room Room { get; set; }
    
    public virtual List<Comment> Comments { get; set; }

    public void Configure(EntityTypeBuilder<Object> builder)
    {
        builder.HasMany(m => m.Comments)
            .WithOne(m => m.Object)
            .OnDelete(DeleteBehavior.Cascade);
    }
}