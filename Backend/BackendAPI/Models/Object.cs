using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackendAPI.Models;

public class Object : IEntityTypeConfiguration<Object>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Name { get; set; }
    public double Grade { get; set; }
    public string Description { get; set; }
    public string Image { get; set; }

    public Object(string name, double grade, string description, string image)
    {
        Name = name;
        Grade = grade;
        Description = description;
        Image = image;
    }

    public Object() { }

    public int RoomId { get; set; }
    public virtual Room Room { get; set; }
    
    public void Configure(EntityTypeBuilder<Object> builder) { }
}