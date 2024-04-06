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

    // Logs in a user
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(string email, string password)
    {
        // Attempt to log in with provided email and password
        var user = await _authService.LoginWithHttpContext(email, password);
        return Ok("You logged in!");
    }

    // Retrieves all users (accessible to Admin only)
    [Authorize(Roles = "Admin")]
    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        // Retrieve all users asynchronously
        var userDtos = await _userService.GetAllAsync().ConfigureAwait(false);

        return Ok(userDtos);
    }

    // Retrieves data of the currently authenticated user
    [Authorize]
    [HttpGet("my-data")]
    public IActionResult GetUserData()
    {
        // Retrieve user data for the current user
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

    // Creates a new user (accessible to Admin only)
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateUser(UserCreateDto userCreate)
    {
        // Create a new user asynchronously
        var newUser = await _userService.CreateUserAsync(userCreate);

        return Ok("Added");
    }

    // Changes the role of a user (accessible to Admin only)
    [Authorize(Roles = "Admin")]
    [HttpPatch("role")]
    public async Task<IActionResult> ChangeRole(string id, int role)
    {
        // Change the role of a user asynchronously
        var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == id)
        {
            return BadRequest("You can't change your own role");
        }

        await _userService.ChangeRoleAsync(id, role);
        return Ok($"User role changed successfully to {role}");
    }

    // Changes the password of the currently authenticated user
    [Authorize]
    [HttpPatch("my-password")]
    public async Task<IActionResult> ChangePassword(string password)
    {
        // Change the password of the current user asynchronously
        var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        await _userService.ChangePasswordAsync(userId, password);

        return Ok($"User password changed successfully to {password}");
    }

    // Bans a user (accessible to Admin only)
    [Authorize(Roles = "Admin")]
    [HttpPatch("ban")]
    public async Task<IActionResult> Ban(string id)
    {
        // Ban a user asynchronously
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

    // Deletes a user (accessible to Admin only)
    [Authorize(Roles = "Admin")]
    [HttpDelete]
    public async Task<IActionResult> DeleteUser(string id)
    {
        // Delete a user asynchronously
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