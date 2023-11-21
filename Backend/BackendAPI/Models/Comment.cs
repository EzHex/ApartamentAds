using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BackendAPI.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackendAPI.Models;

public class Comment : IEntityTypeConfiguration<Comment>, IUserOwnedResource
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    public string Content { get; set; }
    
    public DateTime Date { get; set; }

    public Comment(string content, DateTime date, int objectId)
    {
        Content = content;
        Date = date;
        ObjectId = objectId;
    }

    public string UserId { get; set; }
    public ApartmentUser User { get; set; }
    
    public int ObjectId { get; set; }
    
    public virtual Object Object { get; set; }
    
    public void Configure(EntityTypeBuilder<Comment> builder) { }
}