using ExamProject1.Models;
using ExamProject1.Dto;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExamProject1.Services;

public class UserService {
    private readonly AppDbContext _dbContext;
    private readonly HttpContext _httpContext;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(AppDbContext dbContext, IHttpContextAccessor accessor, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;

        if (accessor.HttpContext is null)
        {
            throw new ArgumentException(nameof(accessor.HttpContext));
        }

        _httpContext = accessor.HttpContext;
    }

    public async Task<List<UserGetDto>> GetAllAsync()
    {
        var users = _dbContext.Users.ToList();

        var usersDtos = _mapper.Map<List<UserGetDto>>(users);

        return usersDtos;
    }
    public async Task<User> CreateUserAsync(UserCreateDto userDto)
    {
        if (await _dbContext.Users.AnyAsync(u => u.Email == userDto.Email))
            throw new InvalidOperationException("User with this email already exists");

        var newUser = _mapper.Map<User>(userDto);

        _dbContext.Users.Add(newUser);
        await _dbContext.SaveChangesAsync();

        return newUser;
    }
    public async Task<string> DeleteUserAsync(int id)
    {
        var user = await _dbContext.Users.FindAsync(id);
        if (user == null)
            throw new InvalidOperationException("User not found");

        if (_httpContextAccessor.HttpContext.User.IsInRole("Admin") && user.Id.ToString() == _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value)
        {
            throw new UnauthorizedAccessException("Admin cannot delete own account.");
        }

        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();
        var deletedUserName = user.FullName;
        return deletedUserName;
    }
}