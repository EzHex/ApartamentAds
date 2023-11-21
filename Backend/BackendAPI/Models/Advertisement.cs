using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BackendAPI.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackendAPI.Models;

public class Advertisement : IEntityTypeConfiguration<Advertisement>, IUserOwnedResource
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    public string Title { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public DateTime Date { get; set; }

    public Advertisement(string title, string description, DateTime date, double price, int apartmentId)
    {
        Title = title;
        Description = description;
        Date = date;
        Price = price;
        ApartmentId = apartmentId;
    }
    
    public string UserId { get; set; }
    public ApartmentUser User { get; set; }

    public int ApartmentId { get; set; }
    public virtual Apartment Apartment { get; set; }

    public void Configure(EntityTypeBuilder<Advertisement> builder) { }
}