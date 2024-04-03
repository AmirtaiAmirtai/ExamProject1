using ExamProject1.Models;
using ExamProject1.Dto;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
namespace ExamProject1.Services;


public class SaleSevice {
    private readonly AppDbContext _dbContext;
    private readonly HttpContext _httpContext; 

}