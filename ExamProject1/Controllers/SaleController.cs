using ExamProject1.Dto;
using ExamProject1.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExamProject1.Controllers
{
    [ApiController]
    [Route("api/Sale")]
    public class SaleController : ControllerBase
    {
        public readonly SaleSevice _saleService;
        private readonly UserService _userService;

        // Constructor for the SaleController class
        public SaleController(SaleSevice saleService, UserService userService)
        {
            _saleService = saleService;
            _userService = userService;
        }

        // Retrieves all sales (accessible to Admin only)
        [Authorize(Roles = "Admin")]
        [HttpGet("all")]
        public async Task<IActionResult> All()
        {
            // Retrieve all sales asynchronously
            var sales = await _saleService.GetAllSalesAsync();
            return Ok(sales);
        }

        // Retrieves sales assigned to the current sales user
        [Authorize(Roles = "Sales")]
        [HttpGet("my-sales")]
        public IActionResult GetSales()
        {
            // Retrieve sales assigned to the current sales user
            var sales = _saleService.GetSalesForCurrentUser();
            return Ok(sales);
        }

        // Creates a new sale
        [Authorize(Roles = "Sales")]
        [HttpPost]
        public async Task<IActionResult> CreateSale(SaleCreateDto saleCreate)
        {
            // Create a new sale asynchronously
            var newSale = await _saleService.CreateSaleAsync(saleCreate);
            return Ok(newSale);
        }

    }
}
