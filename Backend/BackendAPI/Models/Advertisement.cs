using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackendAPI.Models;

public class Advertisement : IEntityTypeConfiguration<Advertisement>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    public string Title { get; set; }
    public string Description { get; set; }
    
    public int ApartamentId { get; set; }
    public virtual Apartament Apartament { get; set; }

    public void Configure(EntityTypeBuilder<Advertisement> builder)
    {
        builder.HasOne(m => m.Apartament)
            .WithOne(m => m.Advertisement);
    }
}