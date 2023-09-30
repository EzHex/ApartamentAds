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
    
    public int RoomId { get; set; }
    public virtual Room Room { get; set; }
    
    public void Configure(EntityTypeBuilder<Object> builder)
    {
        
    }
}