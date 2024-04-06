using ExamProject1.Dto;
using ExamProject1.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ExamProject1.Controllers;

[ApiController]
[Route("api/Contact")]
public class ContactController : ControllerBase
{
    private readonly AppDbContext _dbContext;
    private readonly ContactService _contactService;
    public ContactController(AppDbContext context, ContactService contactService)
    {
        _dbContext = context;
        _contactService = contactService;
    }

    [HttpGet("all")]
    [Authorize(Roles = "Admin, Marketing")]
    public async Task<IActionResult> GetAllAsync()
    {
        var contacts = await _contactService.GetAllAsync().ConfigureAwait(false);
        return Ok(contacts);
    }

    [HttpGet("leads")]
    [Authorize(Roles = "Marketing")]
    public async Task<IActionResult> GetLeadsAsync()
    {
        var leads = await _contactService.GetLeadsAsync().ConfigureAwait(false);
        return Ok(leads);
    }

    [HttpPost]
    [Authorize(Roles = "Marketing")]
    public async Task<IActionResult> CreateContactAsync(ContactCreateDto contactDto)
    {
        var newContact = await _contactService.CreateContactAsync(contactDto);
        return Ok("Added");
    }

    [HttpPatch("change-contact-status")]
    [Authorize(Roles = "Marketing")]
    public async Task<IActionResult> ChangeContactStatusAsync(string id, int newStatus)
    {
        var changedContact = await _contactService.ChangeStatusAsync(id, newStatus);
        if (changedContact == null)
        {
            return NotFound(); 
        }
        return Ok(changedContact);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Marketing, Sales")]
    public async Task<IActionResult> ChangeContactAsync(int id, ContactChangeDto contactDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _contactService.UpdateContactAsync(id, contactDto);

        return result;
    }
}