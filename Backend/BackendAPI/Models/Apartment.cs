﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackendAPI.Models;

public class Apartment : IEntityTypeConfiguration<Apartment>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    public string Address { get; set; }
    
    public int Floor { get; set; }
    
    public int Number { get; set; }
    
    public double Area { get; set; }
    
    public double Rating { get; set; }
    
    public Apartment(string address, int floor, int number, double area, double rating, int userId)
    {
        Address = address;
        Floor = floor;
        Number = number;
        Area = area;
        Rating = rating;
        UserId = userId;
    }

    public Apartment() { }
    
    public virtual List<Room> Rooms { get; set; }
    
    public virtual Advertisement? Advertisement { get; set; }
    
    public int UserId { get; set; }
    public virtual User User { get; set; }

    public void Configure(EntityTypeBuilder<Apartment> builder)
    {
        builder.HasMany(m => m.Rooms)
            .WithOne(m => m.Apartment)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(m => m.User)
            .WithMany(m => m.Apartments);
    }
}