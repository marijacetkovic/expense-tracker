using ExpenseTracker.Models;
using ExpenseTracker.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserApiController : ControllerBase
{
    private readonly UserService _userService;

    public UserApiController(UserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        bool success = await _userService.CreateUserAsync(model.Username, model.Email, model.Password);
        
        if (!success)
            return Conflict(new { message = "Username or Email already exists." }); // 409 Conflict

        return StatusCode(201, new { message = "User created successfully" }); // 201 Created
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginViewModel model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var user = await _userService.GetUser(model.Username);
        
        if (user == null)
            return NotFound(new { message = "User is not registered." }); // 404 Not Found

        if (!PasswordService.VerifyPassword(model.Password, user.PasswordHash, user.PasswordSalt))
            return Unauthorized(new { message = "Incorrect password." }); // 401 Unauthorized

        HttpContext.Session.SetInt32("UserId", user.Id);
        HttpContext.Session.SetString("Username", user.Username); 

        return Ok(new { message = "Login successful" }); // 200 OK
    }
}