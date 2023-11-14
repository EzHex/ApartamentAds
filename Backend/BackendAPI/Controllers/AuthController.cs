using BackendAPI.Auth;
using BackendAPI.Dtos;
using BackendAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BackendAPI.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApartmentUser> _userManager;
    private readonly IJwtTokenService _jwtTokenService;

    public AuthController(UserManager<ApartmentUser> userManager, IJwtTokenService jwtTokenService)
    {
        _userManager = userManager;
        _jwtTokenService = jwtTokenService;
    }


    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register(RegisterUserDto registerDto)
    {
        var user = await _userManager.FindByNameAsync(registerDto.UserName);
        if (user != null)
            return BadRequest("User with this username already exists");
        
        
        var newUser = new ApartmentUser(registerDto.UserName, registerDto.Email);
        
        var createdUserResult = await _userManager.CreateAsync(newUser, registerDto.Password);
        
        if (!createdUserResult.Succeeded)
            return BadRequest(createdUserResult.Errors);
        
        
        await _userManager.AddToRoleAsync(newUser, Roles.User);

        return CreatedAtAction(nameof(Register), new UserDto(newUser.Id, newUser.UserName, newUser.Email));
    }
    
    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login(LoginUserDto loginDto)
    {
        var user = await _userManager.FindByNameAsync(loginDto.UserName);
        if (user == null)
            return BadRequest("Username or password is invalid");
        
        var passwordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);
        
        if (!passwordValid)
            return BadRequest("Username or password is invalid");
        
        // valid user
        var roles = await _userManager.GetRolesAsync(user);
        var accessToken = _jwtTokenService.CreateAccessToken(user.UserName, user.Id, roles);
        user.LastLogin = DateTime.Now;
        await _userManager.UpdateAsync(user);

        return Ok(new SuccessfulLoginDto(accessToken));
    }
    
}