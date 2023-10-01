using BackendAPI.Dtos;
using BackendAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendAPI.Controllers;

[ApiController]
[Route("api/apartments")]
public class ApartamentController : ControllerBase
{
    private readonly ApartmentAdsDbContext _context;

    public ApartamentController(ApartmentAdsDbContext context)
    {
        this._context = context;
    }
    
    // api/apartments GET List 200
    // api/apartments/{id} GET Read 200
    // api/apartments POST Create 201
    // api/apartments/{id} PUT Update 200
    // api/apartments/{id} DELETE Delete 200
    
    [HttpGet]
    public async Task<IEnumerable<ApartmentDto>> GetList()
    {
        var apartments = await _context.Apartments.ToListAsync();

        return apartments.Select(a => new ApartmentDto(a.Id, a.Address, a.Floor, a.Number, a.Area, a.Rating));
    }
    
    [HttpGet("{apartmentId}")]
    public async Task<ActionResult<ApartmentDto>> Get(int apartmentId)
    {
        var firstApartment = await _context.Apartments.FirstOrDefaultAsync(a => a.Id == apartmentId);

        if (firstApartment == null)
            return NotFound();

        return new ApartmentDto(firstApartment.Id, firstApartment.Address, firstApartment.Floor, firstApartment.Number, firstApartment.Area, firstApartment.Rating);
    }
    
    [HttpPost]
    public async Task<ActionResult<ApartmentDto>> Create(CreateApartmentDto createApartmentDto)
    {
        var newApartment = new Apartment(createApartmentDto.Address, createApartmentDto.Floor, createApartmentDto.Number, createApartmentDto.Area, createApartmentDto.Rating);

        _context.Apartments.Add(newApartment);
        await _context.SaveChangesAsync();
        
        return Created("",  new ApartmentDto(newApartment.Id, newApartment.Address, newApartment.Floor, newApartment.Number, newApartment.Area, newApartment.Rating));
    }
    
    [HttpPut("{apartmentId}")]
    public async Task<ActionResult<ApartmentDto>> Update(int apartmentId, UpdateApartmentDto updateApartmentDto)
    {
        var firstApartment = await _context.Apartments.FirstOrDefaultAsync(a => a.Id == apartmentId);

        if (firstApartment == null)
            return NotFound();
        
        firstApartment.Rating = updateApartmentDto.Rating;
        await _context.SaveChangesAsync();

        return new ApartmentDto(firstApartment.Id, firstApartment.Address, firstApartment.Floor, firstApartment.Number, firstApartment.Area, firstApartment.Rating);
    }
    
    [HttpDelete("{apartmentId}")]
    public async Task<ActionResult> Delete(int apartmentId)
    {
        var firstApartment = await _context.Apartments.FirstOrDefaultAsync(a => a.Id == apartmentId);

        if (firstApartment == null)
            return NotFound();
        
        _context.Apartments.Remove(firstApartment);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}