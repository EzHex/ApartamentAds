using BackendAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Object = BackendAPI.Models.Object;

namespace BackendAPI;

public class ApartmentAdsDbContext : IdentityDbContext<ApartmentUser>
{
    public ApartmentAdsDbContext(DbContextOptions options) : base(options) { }

    protected ApartmentAdsDbContext() { }

    public virtual DbSet<Apartment> Apartments { get; set; } = null!;
    public virtual DbSet<Room> Rooms { get; set; } = null!;
    public virtual DbSet<Object> Objects { get; set; } = null!;
    public virtual DbSet<Advertisement> Advertisements { get; set; } = null!;
    public virtual DbSet<Comment> Comments { get; set; } = null!;
    
    
}