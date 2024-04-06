using ExamProject1.Dto;
using ExamProject1.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExamProject1.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{

    private readonly UserService _userService;
    private readonly AuthService _authService;

    public UserController(AuthService authService, UserService userService)
    {
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        _authService = authService;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(string email, string password)
    {
        var user = await _authService.LoginWithHttpContext(email, password);
        return Ok("You logged in!");
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        var userDtos = await _userService.GetAllAsync().ConfigureAwait(false);

        return Ok(userDtos);
    }

    [Authorize]
    [HttpGet("data")]
    public IActionResult GetUserData()
    {
        var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("no");
        }

        var userData = _userService.GetUserByIdAsync(userId);

        if (userData == null)
        {
            return NotFound();
        }

        return Ok(userData);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateUser(UserCreateDto userCreate)
    {
        var newUser = await _userService.CreateUserAsync(userCreate);

        return Ok("Added");
    }

    [Authorize(Roles = "Admin")]
    [HttpPatch("change-role")]
    public async Task<IActionResult> ChangeRole(string id, int role)
    {
        var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == id)
        {
            return BadRequest("You can't change your own role");
        }

        await _userService.ChangeRoleAsync(id, role);
        return Ok($"User role changed successfully to {role}");
    }

    [Authorize]
    [HttpPatch("change-password")]
    public async Task<IActionResult> ChangePassword(string password)
    {
        var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("no");
        }

        var userData = _userService.GetUserByIdAsync(userId);

        if (userData == null)
        {
            return NotFound();
        }

        await _userService.ChangePasswordAsync(userId, password);
        return Ok($"User password changed successfully to {password}");
    }

    [Authorize(Roles = "Admin")]
    [HttpPatch("ban")]
    public async Task<IActionResult> Ban(string id)
    {
        var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userData = await _userService.GetUserByIdAsync(userId);

        if (userId == id)
        {
            return BadRequest("You can't ban yourself");
        }

        var changedUser = await _userService.ChangeBanDateAsync(id);

        if (changedUser == null)
        {
            return NotFound($"User with ID {id} not found");
        }

        return Ok($"User {userData.FullName} has successfully received a new ban date: {DateTime.Now}");
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == id)
        {
            return BadRequest("You can't delete your own account");
        }
        int intId = int.Parse(id);
        try
        {
            var deletedUserName = await _userService.DeleteUserAsync(intId);
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