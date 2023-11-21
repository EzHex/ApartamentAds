using System.Security.Claims;
using BackendAPI.Auth;
using BackendAPI.Dtos;
using BackendAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;

namespace BackendAPI.Controllers;

[ApiController]
[Authorize(Roles = Roles.User)]
[Route("api/apartments/{apartmentId}/rooms/{roomId}/objects/{objectId}/comments")]
public class CommentController : ControllerBase
{
    private readonly ApartmentAdsDbContext _context;
    private readonly IAuthorizationService _authorizationService;

    public CommentController(ApartmentAdsDbContext context, IAuthorizationService authorizationService)
    {
        _context = context;
        _authorizationService = authorizationService;
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
        
        comments.ForEach(c =>
        {
            var authorizationResult = _authorizationService
                .AuthorizeAsync(User, c, PolicyNames.ResourceOwner);
            if (!authorizationResult.Result.Succeeded)
                Forbid();
        });

        return Ok(comments.Select(c => new CommentDto(c.Content)));
    }
    
    [HttpPost]
    public async Task<ActionResult<CommentDto>> Create(int objectId, CommentDto commentDto)
    {
        var validator = new CommentDtoValidator();
        var result = await validator.ValidateAsync(commentDto);
        
        if (!result.IsValid)
            return UnprocessableEntity(result.Errors);

        var newComment = new Comment(commentDto.Content, DateTime.Now, objectId)
        {
            UserId = User.FindFirstValue(JwtRegisteredClaimNames.Sub)
        };
        
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
        
        var authorizationResult = _authorizationService
            .AuthorizeAsync(User, firstComment, PolicyNames.ResourceOwner);
        if (!authorizationResult.Result.Succeeded)
            return Forbid();

        _context.Comments.Remove(firstComment);
        await _context.SaveChangesAsync();
        
        return NoContent();
    }
}