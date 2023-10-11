using BackendAPI.Dtos;
using BackendAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendAPI.Controllers;

[ApiController]
[Route("api/apartments/{apartmentId}/rooms/{roomId}/objects/{objectId}/comments")]
public class CommentController : ControllerBase
{
    private readonly ApartmentAdsDbContext _context;

    public CommentController(ApartmentAdsDbContext context)
    {
        this._context = context;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetList(int apartmentId, int roomId, int objectId)
    {
        var comments = await _context.Comments
            .Where(c => c.ObjectId == objectId)
            .Where(c => c.Object.RoomId == roomId)
            .Where(c => c.Object.Room.ApartmentId == apartmentId)
            .ToListAsync();
        
        if (comments.Count == 0)
            return NotFound();

        return Ok(comments.Select(c => new CommentDto(c.Content)));
    }
    
    [HttpPost]
    public async Task<ActionResult<CommentDto>> Create(int objectId, CommentDto commentDto)
    {
        var validator = new CommentDtoValidator();
        var result = await validator.ValidateAsync(commentDto);
        
        if (!result.IsValid)
            return UnprocessableEntity(result.Errors);

        var newComment = new Comment(commentDto.Content, DateTime.Now, objectId);
        
        _context.Comments.Add(newComment);
        await _context.SaveChangesAsync();
        
        return Created("", new CommentDto(newComment.Content));
    }
    
    [HttpDelete("{commentId}")]
    public async Task<ActionResult<CommentDto>> Delete(int apartmentId, int roomId, int objectId, int commentId)
    {
        var firstComment = await _context.Comments
            .FirstOrDefaultAsync(c => c.Id == commentId && c.ObjectId == objectId &&
                                      c.Object.RoomId == roomId && c.Object.Room.ApartmentId == apartmentId);
        
        if (firstComment == null)
            return NotFound();

        _context.Comments.Remove(firstComment);
        await _context.SaveChangesAsync();
        
        return NoContent();
    }
}