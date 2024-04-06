using AutoMapper;
using ExamProject1.Dto;
using ExamProject1.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ExamProject1.Services;


public class SaleSevice
{
    private readonly AppDbContext _dbContext;
    private readonly HttpContext _httpContext;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SaleSevice(AppDbContext dbContext, IHttpContextAccessor httpContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContext;
        _mapper = mapper;
    }

    public async Task<List<Sale>> GetAllSalesAsync()
    {
        var sales = await _dbContext.Sales.ToListAsync();
        return sales;
    }
    public async Task<List<Sale>> GetSalesForCurrentUser()
    {
        var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        int intId = int.Parse(userId);
        var sales = await _dbContext.Sales
            .Where(l => l.SellerId == intId)
            .ToListAsync();

        return sales;
    }
    public async Task<Sale> CreateSaleAsync(SaleCreateDto saleCreate)
    {
        var newSale = _mapper.Map<Sale>(saleCreate);
        try
        {
            _dbContext.Sales.Add(newSale);
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            throw new InvalidOperationException("One of the ID's is not found", ex);
        }
        return newSale;
    }
}