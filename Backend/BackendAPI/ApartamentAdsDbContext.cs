using BackendAPI.Models;
using Microsoft.EntityFrameworkCore;
using Object = BackendAPI.Models.Object;

namespace BackendAPI;

public class ApartamentAdsDbContext : DbContext
{
    public ApartamentAdsDbContext(DbContextOptions options) : base(options) { }

    protected ApartamentAdsDbContext() { }

    public virtual DbSet<Apartament> Apartaments { get; set; } = null!;
    public virtual DbSet<Room> Rooms { get; set; } = null!;
    public virtual DbSet<Object> Objects { get; set; } = null!;
    public virtual DbSet<User> Users { get; set; } = null!;
    public virtual DbSet<Admin> Admins { get; set; } = null!;
    public virtual DbSet<Advertisement> Advertisements { get; set; } = null!;
}