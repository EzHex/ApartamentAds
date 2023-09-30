using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackendAPI.Models;

public class Room : IEntityTypeConfiguration<Room>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    public double Grade { get; set; }
    
    public int ApartamentId { get; set; }
    public virtual Apartament Apartament { get; set; }
    public virtual List<Object> Objects { get; set; }
    
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.HasMany(m => m.Objects)
            .WithOne(m => m.Room)
            .OnDelete(DeleteBehavior.Cascade);
    }
}