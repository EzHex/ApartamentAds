using BackendAPI.Dtos;
using BackendAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendAPI.Controllers;

[ApiController]
[Route("api/apartments/{apartmentId}/rooms")]
public class RoomController : ControllerBase
{
    private readonly ApartmentAdsDbContext _context;

    public RoomController(ApartmentAdsDbContext context)
    {
        this._context = context;
    }
    
    [HttpGet]
    public async Task<IEnumerable<RoomDto>> GetList(int apartmentId)
    {
        var rooms = await _context.Rooms.Where(r => r.ApartmentId == apartmentId).ToListAsync();

        return rooms.Select(r => new RoomDto(r.Id, r.Name, r.Grade));
    }

    [HttpGet("{roomId}")]
    public async Task<ActionResult<RoomDto>> Get(int apartmentId, int roomId)
    {
        var firstRoom = await _context.Rooms
            .Where(r => r.ApartmentId == apartmentId)
            .FirstOrDefaultAsync(r => r.Id == roomId);

        if (firstRoom == null)
            return NotFound();
        
        return new RoomDto(firstRoom.Id, firstRoom.Name, firstRoom.Grade);
    }
    
    [HttpPost]
    public async Task<ActionResult<RoomDto>> Create(int apartmentId, RoomDto roomDto)
    {
        var newRoom = new Room(roomDto.Name, roomDto.Grade)
        {
            ApartmentId = apartmentId
        };

        _context.Rooms.Add(newRoom);
        await _context.SaveChangesAsync();
        
        return Created("",  new RoomDto(newRoom.Id, newRoom.Name, newRoom.Grade));
    }
    
    [HttpPut("{roomId}")]
    public async Task<ActionResult<RoomDto>> Update(int apartmentId, int roomId, CreateRoomDto updateRoomDto)
    {
        var firstRoom = await _context.Rooms
            .Where(r => r.ApartmentId == apartmentId)
            .FirstOrDefaultAsync(r => r.Id == roomId);

        if (firstRoom == null)
            return NotFound();
        
        firstRoom.Name = updateRoomDto.Name;
        firstRoom.Grade = updateRoomDto.Grade;
        
        await _context.SaveChangesAsync();
        
        return new RoomDto(firstRoom.Id, firstRoom.Name, firstRoom.Grade);
    }
    
    [HttpDelete("{roomId}")]
    public async Task<ActionResult> Delete(int apartmentId, int roomId)
    {
        var firstRoom = await _context.Rooms
            .Where(r => r.ApartmentId == apartmentId)
            .FirstOrDefaultAsync(r => r.Id == roomId);

        if (firstRoom == null)
            return NotFound();
        
        _context.Remove(firstRoom);
        await _context.SaveChangesAsync();
        
        return NoContent();
    }
}