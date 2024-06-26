﻿using AutoMapper;
using ExamProject1.Dto;
using ExamProject1.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ExamProject1.Services;

public class UserService
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(AppDbContext dbContext, IHttpContextAccessor accessor, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<List<UserGetDto>> GetAllAsync()
    {
        var users = await _dbContext.Users.ToListAsync();
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

    public async Task<User> GetUserByIdAsync(string userId)
    {
        if (!int.TryParse(userId, out int userIdInt))
        {
            throw new InvalidOperationException("User not found");
        }
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userIdInt);

        return user;
    }

    public async Task<User> ChangeRoleAsync(string userId, int role)
    {
        if (!int.TryParse(userId, out int userIdInt))
        {
            throw new InvalidOperationException("User not found");
        }
        Enums.Role newRole = (Enums.Role)role;

        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userIdInt);
        if (user != null)
        {
            user.Role = newRole;
            await _dbContext.SaveChangesAsync();
        }

        return user;
    }

    public async Task<User> ChangePasswordAsync(string userId, string password)
    {
        if (!int.TryParse(userId, out int userIdInt))
        {
            throw new InvalidOperationException("User not found");
        }

        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userIdInt);

        if (user != null)
        {
            user.Password = password;
            await _dbContext.SaveChangesAsync();
        }

        return user;
    }

    public async Task<User> ChangeBanDateAsync(string userId)
    {
        if (!int.TryParse(userId, out int userIdInt))
        {
            throw new InvalidOperationException("User not found");
        }
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userIdInt);

        if (user != null)
        {
            user.BanDate = DateTime.Now;
            await _dbContext.SaveChangesAsync();
        }

        return user;
    }
}