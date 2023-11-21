using System.Security.Claims;
using BackendAPI.Auth;
using BackendAPI.Dtos;
using BackendAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using NuGet.Protocol;

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
            return BadRequest(new ErrorDto("User with this username already exists"));
        
        
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
            return BadRequest(new ErrorDto("Username or password is invalid"));
        
        var passwordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);
        if (!passwordValid)
            return BadRequest(new ErrorDto("Username or password is invalid"));
        
        // valid user
        var roles = await _userManager.GetRolesAsync(user);
        var accessToken = _jwtTokenService.CreateAccessToken(user.UserName, user.Id, roles);
        var refreshToken = _jwtTokenService.CreateRefreshToken(user.Id);
        user.LastLogin = DateTime.Now;
        await _userManager.UpdateAsync(user);

        return Ok(new SuccessfulLoginDto(accessToken, refreshToken));
    }

    [HttpPost]
    [Route("accessToken")]
    public async Task<IActionResult> GetAccessToken(RefreshAccessTokenDto refreshAccessTokenDto)
    {
        if (!_jwtTokenService.TryParseRefreshToken(refreshAccessTokenDto.RefreshToken, out var claims))
            return Unauthorized();

        var userId = claims.FindFirstValue(JwtRegisteredClaimNames.Sub);
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return Unauthorized();

        var roles = await _userManager.GetRolesAsync(user);
        var accessToken = _jwtTokenService.CreateAccessToken(user.UserName, user.Id, roles);

        return Ok(new SuccessfulLoginDto(accessToken, refreshAccessTokenDto.RefreshToken));
    }
    
}