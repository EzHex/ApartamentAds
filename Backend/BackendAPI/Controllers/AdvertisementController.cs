using BackendAPI.Dtos;
using BackendAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendAPI.Controllers;

[ApiController]
[Route("api/ads")]
public class AdvertisementController : ControllerBase
{
    private readonly ApartmentAdsDbContext _context;
    
    public AdvertisementController(ApartmentAdsDbContext context)
    {
        this._context = context;
    }
    
    [HttpGet]
    public async Task<IEnumerable<AdvertisementDto>> GetList()
    {
        var advertisements = await _context.Advertisements.ToListAsync();

        return advertisements.Select(a => new AdvertisementDto(a.Id, a.Title, a.Description, a.Price,
            a.Date));
    }
    
    [HttpGet("{adId}")]
    public async Task<ActionResult<AdvertisementDto>> Get(int adId)
    {
        var firstAdvertisement = await _context.Advertisements.FirstOrDefaultAsync(a => a.Id == adId);
        
        if (firstAdvertisement == null)
            return NotFound();

        return new AdvertisementDto(firstAdvertisement.Id, firstAdvertisement.Title, firstAdvertisement.Description,
            firstAdvertisement.Price, firstAdvertisement.Date);
    }
    
    [HttpGet("{adId}/registered-users")]
    public async Task<ActionResult<AdvertisementWithOwnerDataDto>> GetWithOwnerData(int adId)
    {
        var firstAdvertisement = await _context.Advertisements.FirstOrDefaultAsync(a => a.Id == adId);
        
        if (firstAdvertisement == null)
            return NotFound();
        
        var apartment = await _context.Apartments
            .FirstOrDefaultAsync(a => a.Id == firstAdvertisement.ApartmentId);
        
        if (apartment == null)
            return NotFound();
        
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == apartment.UserId);
        
        if (user == null)
            return NotFound();

        return new AdvertisementWithOwnerDataDto(firstAdvertisement.Title, firstAdvertisement.Description,
            firstAdvertisement.Price, firstAdvertisement.Date, user.Name, user.Email);
    }
    
    [HttpPost]
    public async Task<ActionResult<AdvertisementDto>> Create(CreateAdvertisementDto advertisementDto)
    {
        var validator = new CreateAdvertisementDtoValidator();
        var result = await validator.ValidateAsync(advertisementDto);
        
        if (!result.IsValid)
            return UnprocessableEntity(result.Errors);

        var newAdvertisement = new Advertisement(advertisementDto.Title, advertisementDto.Description,
            DateTime.Now, advertisementDto.Price, advertisementDto.ApartmentId);
        
        _context.Advertisements.Add(newAdvertisement);
        await _context.SaveChangesAsync();
        
        return Created("",  new AdvertisementDto(newAdvertisement.Id, newAdvertisement.Title,
            newAdvertisement.Description, newAdvertisement.Price, newAdvertisement.Date));
    }
    
    [HttpDelete]
    public async Task<ActionResult> Delete(int adId)
    {
        var firstAdvertisement = await _context.Advertisements.FirstOrDefaultAsync(a => a.Id == adId);
        
        if (firstAdvertisement == null)
            return NotFound();
        
        _context.Advertisements.Remove(firstAdvertisement);
        await _context.SaveChangesAsync();
        
        return NoContent();
    }

}