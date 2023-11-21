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
[Route("api/ads")]
public class AdvertisementController : ControllerBase
{
    private readonly ApartmentAdsDbContext _context;
    private readonly IAuthorizationService _authorizationService;
    
    public AdvertisementController(ApartmentAdsDbContext context, IAuthorizationService authorizationService)
    {
        _context = context;
        _authorizationService = authorizationService;
    }
    
    [AllowAnonymous]
    [HttpGet]
    public async Task<IEnumerable<AdvertisementDto>> GetList()
    {
        var advertisements = await _context.Advertisements.ToListAsync();

        return advertisements.Select(a => new AdvertisementDto(a.Id, a.Title, a.Description, a.Price,
            a.Date));
    }
    
    [AllowAnonymous]
    [HttpGet("{adId}")]
    public async Task<IActionResult> Get(int adId)
    {
        var firstAdvertisement = await _context.Advertisements.FirstOrDefaultAsync(a => a.Id == adId);
        
        if (firstAdvertisement == null)
            return NotFound();

        if (!User.Identity.IsAuthenticated)
            return Ok(new AdvertisementWithoutOwnerDataDto(firstAdvertisement.Title, firstAdvertisement.Description,
                firstAdvertisement.Price, firstAdvertisement.Date));
        
        var apartment = await _context.Apartments
            .FirstOrDefaultAsync(a => a.Id == firstAdvertisement.ApartmentId);
        
        if (apartment == null)
            return NotFound();
        
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == apartment.UserId);
        
        if (user == null)
            return NotFound();

        return Ok(new AdvertisementWithOwnerDataDto(firstAdvertisement.Title, firstAdvertisement.Description,
            firstAdvertisement.Price, firstAdvertisement.Date, user.Email));
    }
    
    [HttpPost]
    public async Task<ActionResult<AdvertisementDto>> Create(CreateAdvertisementDto advertisementDto)
    {
        var validator = new CreateAdvertisementDtoValidator();
        var result = await validator.ValidateAsync(advertisementDto);
        
        if (!result.IsValid)
            return UnprocessableEntity(result.Errors);
        
        var apartment = await _context.Apartments
            .FirstOrDefaultAsync(a => a.Id == advertisementDto.ApartmentId);
        if (apartment == null)
            return UnprocessableEntity();
        
        var ad = await _context.Advertisements
            .FirstOrDefaultAsync(a => a.ApartmentId == advertisementDto.ApartmentId);
        if (ad != null)
            return Conflict(new ErrorDto("Advertisement for this apartment already exists"));

        var newAdvertisement = new Advertisement(advertisementDto.Title, advertisementDto.Description,
            DateTime.Now, advertisementDto.Price, advertisementDto.ApartmentId)
        {
            UserId = User.FindFirstValue(JwtRegisteredClaimNames.Sub)
        };
        
        _context.Advertisements.Add(newAdvertisement);
        await _context.SaveChangesAsync();
        
        return Created("",  new AdvertisementDto(newAdvertisement.Id, newAdvertisement.Title,
            newAdvertisement.Description, newAdvertisement.Price, newAdvertisement.Date));
    }
    
    [HttpDelete("{adId}")]
    public async Task<ActionResult> Delete(int adId)
    {
        var firstAdvertisement = await _context.Advertisements.FirstOrDefaultAsync(a => a.Id == adId);
        
        if (firstAdvertisement == null)
            return NotFound();
        
        var authorizationResult = _authorizationService
            .AuthorizeAsync(User, firstAdvertisement, PolicyNames.ResourceOwner);
        if (!authorizationResult.Result.Succeeded)
            return Forbid();
        
        _context.Advertisements.Remove(firstAdvertisement);
        await _context.SaveChangesAsync();
        
        return NoContent();
    }

}