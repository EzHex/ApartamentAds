﻿using System.Security.Claims;
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
[Route("api/apartments/{apartmentId}/rooms")]
public class RoomController : ControllerBase
{
    private readonly ApartmentAdsDbContext _context;
    private readonly IAuthorizationService _authorizationService;

    public RoomController(ApartmentAdsDbContext context, IAuthorizationService authorizationService)
    {
        _context = context;
        _authorizationService = authorizationService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetList(int apartmentId)
    {
        var rooms = await _context.Rooms.Where(r => r.ApartmentId == apartmentId).ToListAsync();

        // if (rooms.Count == 0)
        //     return NotFound();
        
        rooms.ForEach(a =>
        {
            var authorizationResult = _authorizationService
                .AuthorizeAsync(User, a, PolicyNames.ResourceOwner);
            if (!authorizationResult.Result.Succeeded)
                Forbid();
        });
        
        return Ok(rooms.Select(r => new RoomDto(r.Id, r.Name, r.Grade)));
    }

    [HttpGet("{roomId}")]
    public async Task<ActionResult<RoomDto>> Get(int apartmentId, int roomId)
    {
        var firstRoom = await _context.Rooms
            .Where(r => r.ApartmentId == apartmentId)
            .FirstOrDefaultAsync(r => r.Id == roomId);

        if (firstRoom == null)
            return NotFound();
        
        var authorizationResult = _authorizationService
            .AuthorizeAsync(User, firstRoom, PolicyNames.ResourceOwner);
        if (!authorizationResult.Result.Succeeded)
            return Forbid();
        
        return new RoomDto(firstRoom.Id, firstRoom.Name, firstRoom.Grade);
    }
    
    [HttpPost]
    public async Task<ActionResult<RoomDto>> Create(int apartmentId, RoomDto roomDto)
    {
        var validator = new CreateRoomDtoValidator();
        var result = await validator.ValidateAsync(roomDto);
        
        if (!result.IsValid)
            return UnprocessableEntity(result.Errors);
        
        var newRoom = new Room(roomDto.Name, roomDto.Grade)
        {
            ApartmentId = apartmentId,
            UserId = User.FindFirstValue(JwtRegisteredClaimNames.Sub)
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
        
        var authorizationResult = _authorizationService
            .AuthorizeAsync(User, firstRoom, PolicyNames.ResourceOwner);
        if (!authorizationResult.Result.Succeeded)
            return Forbid();
        
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
        
        var authorizationResult = _authorizationService
            .AuthorizeAsync(User, firstRoom, PolicyNames.ResourceOwner);
        if (!authorizationResult.Result.Succeeded)
            return Forbid();
        
        _context.Remove(firstRoom);
        await _context.SaveChangesAsync();
        
        return NoContent();
    }
}