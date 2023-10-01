using BackendAPI.Models;
using Microsoft.EntityFrameworkCore;
using Object = BackendAPI.Models.Object;

namespace BackendAPI;

public class ApartmentAdsDbContext : DbContext
{
    public ApartmentAdsDbContext(DbContextOptions options) : base(options) { }

    protected ApartmentAdsDbContext() { }

    public virtual DbSet<Apartment> Apartments { get; set; } = null!;
    public virtual DbSet<Room> Rooms { get; set; } = null!;
    public virtual DbSet<Object> Objects { get; set; } = null!;
    public virtual DbSet<User> Users { get; set; } = null!;
    public virtual DbSet<Admin> Admins { get; set; } = null!;
    public virtual DbSet<Advertisement> Advertisements { get; set; } = null!;
}