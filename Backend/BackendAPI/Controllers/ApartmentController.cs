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
[Route("api/apartments")]
public class ApartmentController : ControllerBase
{
    private readonly ApartmentAdsDbContext _context;
    private readonly IAuthorizationService _authorizationService;

    public ApartmentController(ApartmentAdsDbContext context, IAuthorizationService authorizationService)
    {
        _context = context;
        _authorizationService = authorizationService;
    }
    
    [HttpGet]
    [Authorize(Roles = Roles.User)]
    public async Task<IActionResult> GetList()
    {
        var apartments = await _context.Apartments
            .Where(a => a.UserId == User.FindFirstValue(JwtRegisteredClaimNames.Sub))
            .ToListAsync();
        
        apartments.ForEach(a =>
        {
            var authorizationResult = _authorizationService
                .AuthorizeAsync(User, a, PolicyNames.ResourceOwner);
            if (!authorizationResult.Result.Succeeded)
                Forbid();
        });
        
        return Ok(apartments.Select(a => new ApartmentDto(a.Id, a.Address, a.Floor, a.Number, a.Area, a.Rating)));
    }
    
    [HttpGet("{apartmentId}")]
    [Authorize(Roles = Roles.User)]
    public async Task<ActionResult<ApartmentDto>> Get(int apartmentId)
    {
        var firstApartment = await _context.Apartments.FirstOrDefaultAsync(a => a.Id == apartmentId);
        
        if (firstApartment == null)
            return NotFound();

        var authorizationResult = _authorizationService
            .AuthorizeAsync(User, firstApartment, PolicyNames.ResourceOwner);
        if (!authorizationResult.Result.Succeeded)
            return Forbid();
        
        return new ApartmentDto(firstApartment.Id, firstApartment.Address, firstApartment.Floor,
            firstApartment.Number, firstApartment.Area, firstApartment.Rating);
    }
    
    [HttpPost]
    [Authorize(Roles = Roles.User)]
    public async Task<ActionResult<ApartmentDto>> Create(CreateApartmentDto createApartmentDto)
    {
        var validator = new CreateApartmentDtoValidator();
        var result = await validator.ValidateAsync(createApartmentDto);
        
        if (!result.IsValid)
            return UnprocessableEntity(result.Errors);

        var newApartment = new Apartment(createApartmentDto.Address, createApartmentDto.Floor,
            createApartmentDto.Number, createApartmentDto.Area, createApartmentDto.Rating)
        {
            UserId = User.FindFirstValue(JwtRegisteredClaimNames.Sub)
        };

        _context.Apartments.Add(newApartment);
        await _context.SaveChangesAsync();
        
        return Created("",  new ApartmentDto(newApartment.Id, newApartment.Address, newApartment.Floor,
            newApartment.Number, newApartment.Area, newApartment.Rating));
    }
    
    [HttpPut("{apartmentId}")]
    [Authorize(Roles = Roles.User)]
    public async Task<ActionResult<ApartmentDto>> Update(int apartmentId, UpdateApartmentDto updateApartmentDto)
    {
        var validator = new UpdateApartmentDtoValidator();
        var result = await validator.ValidateAsync(updateApartmentDto);
        
        if (!result.IsValid)
            return UnprocessableEntity(result.Errors);
        
        var firstApartment = await _context.Apartments.FirstOrDefaultAsync(a => a.Id == apartmentId);

        if (firstApartment == null)
            return NotFound();

        var authorizationResult = _authorizationService
            .AuthorizeAsync(User, firstApartment, PolicyNames.ResourceOwner);
        if (!authorizationResult.Result.Succeeded)
            return Forbid();
        
        firstApartment.Rating = updateApartmentDto.Rating;
        await _context.SaveChangesAsync();

        return new ApartmentDto(firstApartment.Id, firstApartment.Address, firstApartment.Floor,
            firstApartment.Number, firstApartment.Area, firstApartment.Rating);
    }
    
    [HttpDelete("{apartmentId}")]
    [Authorize(Roles = Roles.User)]
    public async Task<ActionResult> Delete(int apartmentId)
    {
        var firstApartment = await _context.Apartments.FirstOrDefaultAsync(a => a.Id == apartmentId);

        if (firstApartment == null)
            return NotFound();
        
        var authorizationResult = _authorizationService
            .AuthorizeAsync(User, firstApartment, PolicyNames.ResourceOwner);
        if (!authorizationResult.Result.Succeeded && !User.IsInRole(Roles.Admin))
            return Forbid();
        
        _context.Apartments.Remove(firstApartment);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}