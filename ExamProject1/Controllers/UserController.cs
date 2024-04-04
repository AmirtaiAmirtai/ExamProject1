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

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateUser(UserCreateDto userCreate)
    {
        var newUser = await _userService.CreateUserAsync(userCreate);

        return Ok("Added");
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("change-role")]
    public async Task<IActionResult> ChangeRole(string id, int role)
    {
        var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == id)
        {
            return BadRequest("You can't change your own role");
        }

        var changedUser = _userService.ChangeRoleAsync(id, role);

        if (changedUser == null)
        {
            return NotFound($"User with ID {id} not found");
        }

        return Ok($"User role changed successfully to {role}");
    }
    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword(string password)
    {
        var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("no");
        }

        // Здесь вы можете использовать идентификатор пользователя для получения данных из базы данных или другого источника данных
        var userData = _userService.GetUserById(userId);

        if (userData == null)
        {
            return NotFound();
        }
        _userService.ChangePasswordAsync(userId, password);
        return Ok($"User password changed successfully to {password}");
    }

    [Authorize]
    [HttpPost("ban-someone")]
    public async Task<IActionResult> Ban(string id)
    {
        var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userData = _userService.GetUserById(userId);

        if (userId == id)
        {
            return BadRequest("You can't change your own role");
        }

        var changedUser = _userService.ChangeBanDateAsync(id);

        if (changedUser == null)
        {
            return NotFound($"User with ID {id} not found");
        }

        return Ok($"User {userData.FullName} has successfully recieved a new ban date: {DateTime.Now}");
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

    [Authorize]
    [HttpGet("data")]
    public IActionResult GetUserData()
    {
        // Получаем идентификатор пользователя из куки
        var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("no");
        }

        // Здесь вы можете использовать идентификатор пользователя для получения данных из базы данных или другого источника данных
        var userData = _userService.GetUserById(userId);

        if (userData == null)
        {
            return NotFound();
        }

        return Ok(userData);
    }

}

public class UserLoginRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}
