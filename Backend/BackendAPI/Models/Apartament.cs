using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackendAPI.Models;

public class Apartament : IEntityTypeConfiguration<Apartament>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    public string Address { get; set; }
    
    public int Floor { get; set; }
    
    public int Number { get; set; }
    
    public double Area { get; set; }
    
    public double Rating { get; set; }
    
    public byte[] Images { get; set; }

    public Apartament(string address, int floor, int number, double area, double rating, byte[] images)
    {
        Address = address;
        Floor = floor;
        Number = number;
        Area = area;
        Rating = rating;
        Images = images;
    }

    public Apartament()
    {
    }
    
    public virtual List<Room> Rooms { get; set; }
    
    public virtual Advertisement Advertisement { get; set; }

    public void Configure(EntityTypeBuilder<Apartament> builder)
    {
        builder.HasMany(m => m.Rooms)
            .WithOne(m => m.Apartament)
            .OnDelete(DeleteBehavior.Cascade);
    }
}