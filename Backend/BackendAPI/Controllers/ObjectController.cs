using BackendAPI.Dtos;
using BackendAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Object = BackendAPI.Models.Object;

namespace BackendAPI.Controllers;

[ApiController]
[Route("api/apartments/{apartmentId}/rooms/{roomsId}/objects")]
public class ObjectController : ControllerBase
{
    private readonly ApartmentAdsDbContext _context;

    public ObjectController(ApartmentAdsDbContext context)
    {
        this._context = context;
    }
    
    [HttpGet]
    public async Task<IEnumerable<ObjectDto>> GetList(int apartmentId, int roomId)
    {
        var objects = await _context.Objects
            .Where(o => o.RoomId == roomId)
            .Where(o => o.Room.ApartmentId == apartmentId)
            .ToListAsync();

        return objects.Select(o => new ObjectDto(o.Id, o.Name, o.Description, o.Image, o.Grade));
    }
    
    [HttpGet("{objectId}")]
    public async Task<ActionResult<CreateObjectDto>> Get(int apartmentId, int roomId, int objectId)
    {
        var firstObject = await _context.Objects
            .Where(o => o.RoomId == roomId)
            .Where(o => o.Room.ApartmentId == apartmentId)
            .FirstOrDefaultAsync(o => o.Id == objectId);

        if (firstObject == null)
            return NotFound();
        
        return new CreateObjectDto(firstObject.Name, firstObject.Description, firstObject.Image, firstObject.Grade);
    }
    
    [HttpPost]
    public async Task<ActionResult<CreatedResult>> Create(int roomId, ObjectDto objectDto)
    {
        var newObject = new Object(objectDto.Name, objectDto.Grade, objectDto.Description, objectDto.Image)
         {
             RoomId = roomId
         };

        _context.Objects.Add(newObject);
        await _context.SaveChangesAsync();
        
        return Created("",  new ObjectDto(newObject.Id, newObject.Name, newObject.Description, newObject.Image, newObject.Grade));
    }
    
    [HttpPut("{objectId}")]
    public async Task<ActionResult<ObjectDto>> Update(int objectId, UpdateObjectDto updateObjectDto)
    {
        var firstObject = await _context.Objects.FirstOrDefaultAsync(o => o.Id == objectId);

        if (firstObject == null)
            return NotFound();
        
        firstObject.Description = updateObjectDto.Description;
        firstObject.Image = updateObjectDto.Image;
        firstObject.Grade = updateObjectDto.Grade;
        
        _context.Update(firstObject);
        await _context.SaveChangesAsync();
        
        return Ok(new ObjectDto(firstObject.Id, firstObject.Name, firstObject.Description, firstObject.Image, firstObject.Grade));
    }
    
    [HttpDelete("{objectId}")]
    public async Task<ActionResult> Delete(int objectId)
    {
        var firstObject = await _context.Objects.FirstOrDefaultAsync(o => o.Id == objectId);
        
        if (firstObject == null)
            return NotFound();
        
        _context.Remove(firstObject);
        await _context.SaveChangesAsync();
        
        return NoContent();
    }
}