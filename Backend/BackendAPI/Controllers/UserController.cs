using BackendAPI.Dtos;
using BackendAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendAPI.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly ApartmentAdsDbContext _context;

    public UserController(ApartmentAdsDbContext context)
    {
        this._context = context;
    }
    
    [HttpGet]
    public async Task<IEnumerable<UserDto>> GetList()
    {
        var users = await _context.Users.ToListAsync();

        return users.Select(u => new UserDto(u.Id, u.Login, u.Email, u.Password, u.Name, u.LastLogin));
    }
    
    [HttpGet("{userId}")]
    public async Task<ActionResult<UserDto>> Get(int userId)
    {
        var firstUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        
        if (firstUser == null)
            return NotFound();

        return new UserDto(firstUser.Id, firstUser.Login, firstUser.Email, firstUser.Password, firstUser.Name,
            firstUser.LastLogin);
    }
    
    [HttpPost]
    public async Task<ActionResult<UserDto>> Create(CreateUserDto createUserDto)
    {
        var validator = new CreateUserDtoValidator();
        var result = await validator.ValidateAsync(createUserDto);
        
        if (!result.IsValid)
            return UnprocessableEntity(result.Errors);

        var newUser = new User(createUserDto.Login, createUserDto.Email, createUserDto.Password, createUserDto.Name,
            DateTime.Now);
        
        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();
        
        return Created("",  new UserDto(newUser.Id, newUser.Login, newUser.Email, newUser.Password,
            newUser.Name, newUser.LastLogin));
    }
    
    [HttpPut("{userId}")]
    public async Task<ActionResult<UserDto>> Update(int userId, UpdateUserDto updateUserDto)
    {
        var validator = new UpdateUserDtoValidator();
        var result = await validator.ValidateAsync(updateUserDto);
        
        if (!result.IsValid)
            return UnprocessableEntity(result.Errors);
        
        var firstUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        
        if (firstUser == null)
            return NotFound();

        firstUser.Password = updateUserDto.Password;
        firstUser.Name = updateUserDto.Name;
        
        await _context.SaveChangesAsync();
        
        return new UserDto(firstUser.Id, firstUser.Login, firstUser.Email, firstUser.Password, firstUser.Name,
            firstUser.LastLogin);
    }
    
    [HttpDelete("{userId}")]
    public async Task<ActionResult> Delete(int userId)
    {
        var firstUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

        if (firstUser == null)
            return NotFound();
        
        _context.Users.Remove(firstUser);
        await _context.SaveChangesAsync();
        
        return NoContent();
    }
    
}