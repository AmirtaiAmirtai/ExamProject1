using ExamProject1.Models;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace ExamProject1.Services
{
    public class AuthService
    {
        private readonly AppDbContext _dbContext;
        private readonly HttpContext _httpContext;
        public AuthService(AppDbContext dbContext, IHttpContextAccessor accessor)
        {
            _dbContext = dbContext;

            if (accessor.HttpContext is null)
            {
                throw new ArgumentException(nameof(accessor.HttpContext));
            }

            _httpContext = accessor.HttpContext;
        }

        public async Task<User> LoginWithHttpContext(string email, string password)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == email) ?? null;
            if (user.BanDate != null) 
            { 
                throw new Exception("This account is banned. Cannot log in"); 
            }
            if (user != null && user.Password == password)
            {
                var claims = new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role.ToString()),
                };

                var identity = new ClaimsIdentity(claims, "Cookies");
                var principal = new ClaimsPrincipal(identity);

                await _httpContext.SignInAsync(principal);

                return user;
            }

            throw new UnauthorizedAccessException("Invalid email or password");
        }
    }
}
