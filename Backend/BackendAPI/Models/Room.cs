﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BackendAPI.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackendAPI.Models;

public class Room : IEntityTypeConfiguration<Room>, IUserOwnedResource
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Name { get; set; }
    public double Grade { get; set; }

    public Room(string name, double grade)
    {
        Name = name;
        Grade = grade;
    }

    public Room() { }
    
    public string UserId { get; set; }
    public ApartmentUser User { get; set; }

    public int ApartmentId { get; set; }
    public virtual Apartment Apartment { get; set; }
    public virtual List<Object> Objects { get; set; }
    
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.HasMany(m => m.Objects)
            .WithOne(m => m.Room)
            .OnDelete(DeleteBehavior.Cascade);
        
        
    }
}