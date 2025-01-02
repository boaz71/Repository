using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RepositoryAPI.Models;
using RepositoryAPI.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[Route("api/[controller]")]
[ApiController]
[EnableCors("AllowLocalhost")]
public class AuthController : ControllerBase

    
{

    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService=authService;
    }


    [HttpGet("GetString")]
    public IActionResult GetString()
    {

        return Ok("Hello World!");
                
    }


    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginData request)
    {


        if (request.Username == "admin" && request.Password=="12345" ) 
        {
            var token = _authService.GenerateJwtToken(request.Username);
            return Ok(new { Token = token });
        }

        return Unauthorized("Invalid credentials");
    }



}

