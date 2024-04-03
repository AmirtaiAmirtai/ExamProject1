using ExamProject1.Dto;
using ExamProject1.Models;
using ExamProject1.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExamProject1.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{

    private readonly AppDbContext _context;
    private readonly UserService _userService;
    private readonly AuthService _authService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserController(AppDbContext context, AuthService authService, UserService userService, IHttpContextAccessor httpContextAccessor)
    {
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        _context = context;
        _authService = authService;
        _httpContextAccessor = httpContextAccessor;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(string email, string password)
    {
        try
        {
            var user = await _authService.LoginWithHttpContext(email, password);
            return NoContent();
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized("Invalid email or password");
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        var userDtos = await _userService.GetAllAsync().ConfigureAwait(false);

        return Ok(userDtos);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateUser(UserCreateDto userCreate)
    {
        var newUser = await _userService.CreateUserAsync(userCreate);

        return Ok("Added");
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        try
        {
            var deletedUserName = await _userService.DeleteUserAsync(id);
            return Ok($"User {deletedUserName} was deleted");
        }
        catch (UnauthorizedAccessException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }
}

public class UserLoginRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}
