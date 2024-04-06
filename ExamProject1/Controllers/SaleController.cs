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

        public SaleController(SaleSevice saleService, UserService userService)
        {
            _saleService = saleService;
            _userService = userService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("all")]
        public async Task<IActionResult> All()
        {
            var sales = await _saleService.GetAllSalesAsync();
            return Ok(sales);
        }

        [Authorize(Roles = "Sales")]
        [HttpGet("my-sales")]
        public IActionResult GetSales()
        {
            var sales = _saleService.GetSalesForCurrentUser();
            return Ok(sales);
        }

        [Authorize(Roles = "Sales")]
        [HttpPost]
        public async Task<IActionResult> CreateSale(SaleCreateDto saleCreate)
        {
            var newSale = await _saleService.CreateSaleAsync(saleCreate);
            return Ok(newSale);
        }
    }
}
