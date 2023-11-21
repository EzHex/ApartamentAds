using System.Security.Claims;
using BackendAPI.Auth;
using BackendAPI.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Object = BackendAPI.Models.Object;

namespace BackendAPI.Controllers;

[ApiController]
[Authorize(Roles = Roles.User)]
[Route("api/apartments/{apartmentId}/rooms/{roomId}/objects")]
public class ObjectController : ControllerBase
{
    private readonly ApartmentAdsDbContext _context;
    private readonly IAuthorizationService _authorizationService;

    public ObjectController(ApartmentAdsDbContext context, IAuthorizationService authorizationService)
    {
        _context = context;
        _authorizationService = authorizationService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetList(int apartmentId, int roomId)
    {
        var objects = await _context.Objects
            .Where(o => o.RoomId == roomId)
            .Where(o => o.Room.ApartmentId == apartmentId)
            .ToListAsync();

        if (objects.Count == 0)
            return NotFound();
        
        objects.ForEach(o =>
        {
            var authorizationResult = _authorizationService
                .AuthorizeAsync(User, o, PolicyNames.ResourceOwner);
            if (!authorizationResult.Result.Succeeded)
                Forbid();
        });
        
        return Ok(objects.Select(o => new ObjectDto(o.Id, o.Name, o.Description, o.Image, o.Grade)));
    }
    
    [HttpGet("{objectId}")]
    public async Task<ActionResult<ObjectDto>> Get(int apartmentId, int roomId, int objectId)
    {
        var firstObject = await _context.Objects
            .Where(o => o.RoomId == roomId)
            .Where(o => o.Room.ApartmentId == apartmentId)
            .FirstOrDefaultAsync(o => o.Id == objectId);

        if (firstObject == null)
            return NotFound();
        
        var authorizationResult = _authorizationService
            .AuthorizeAsync(User, firstObject, PolicyNames.ResourceOwner);
        if (!authorizationResult.Result.Succeeded)
            return Forbid();
        
        return new ObjectDto(firstObject.Id, firstObject.Name, firstObject.Description, firstObject.Image,
            firstObject.Grade);
    }
    
    [HttpPost]
    public async Task<ActionResult<ObjectDto>> Create(int roomId, CreateObjectDto objectDto)
    {
        var validator = new CreateObjectDtoValidator();
        var result = await validator.ValidateAsync(objectDto);
        
        if (!result.IsValid)
            return UnprocessableEntity(result.Errors);

        var newObject = new Object(objectDto.Name, objectDto.Grade, objectDto.Description, objectDto.Image,
            roomId)
        {
            UserId = User.FindFirstValue(JwtRegisteredClaimNames.Sub)
        };

        _context.Objects.Add(newObject);
        await _context.SaveChangesAsync();
        
        return Created("",  new ObjectDto(newObject.Id, newObject.Name, newObject.Description,
            newObject.Image, newObject.Grade));
    }
    
    [HttpPut("{objectId}")]
    public async Task<ActionResult<ObjectDto>> Update(int apartmentId, int roomId, int objectId, UpdateObjectDto updateObjectDto)
    {
        var firstObject = await _context.Objects
            .Where(o => o.RoomId == roomId)
            .Where(o => o.Room.ApartmentId == apartmentId)
            .FirstOrDefaultAsync(o => o.Id == objectId);

        if (firstObject == null)
            return NotFound();
        
        var authorizationResult = _authorizationService
            .AuthorizeAsync(User, firstObject, PolicyNames.ResourceOwner);
        if (!authorizationResult.Result.Succeeded)
            return Forbid();
        
        firstObject.Description = updateObjectDto.Description;
        firstObject.Image = updateObjectDto.Image;
        firstObject.Grade = updateObjectDto.Grade;
        
        _context.Update(firstObject);
        await _context.SaveChangesAsync();
        
        return Ok(new ObjectDto(firstObject.Id, firstObject.Name, firstObject.Description,
            firstObject.Image, firstObject.Grade));
    }
    
    [HttpDelete("{objectId}")]
    public async Task<ActionResult> Delete(int apartmentId, int roomId, int objectId)
    {
        var firstObject = await _context.Objects
            .Where(o => o.RoomId == roomId)
            .Where(o => o.Room.ApartmentId == apartmentId)
            .FirstOrDefaultAsync(o => o.Id == objectId);
        
        if (firstObject == null)
            return NotFound();
        
        var authorizationResult = _authorizationService
            .AuthorizeAsync(User, firstObject, PolicyNames.ResourceOwner);
        if (!authorizationResult.Result.Succeeded)
            return Forbid();
        
        _context.Remove(firstObject);
        await _context.SaveChangesAsync();
        
        return NoContent();
    }
}