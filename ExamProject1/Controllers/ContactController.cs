using ExamProject1.Dto;
using ExamProject1.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExamProject1.Controllers;

[ApiController]
[Route("api/Contact")]
public class ContactController : ControllerBase
{
    private readonly ContactService _contactService;
    public ContactController(ContactService contactService)
    {
        _contactService = contactService;
    }


    // Shows all existing contacts
    [HttpGet("all")]
    [Authorize(Roles = "Admin, Marketing")]
    public async Task<IActionResult> GetAllAsync()
    {
        // Retrieve all contacts asynchronously
        var contacts = await _contactService.GetAllAsync().ConfigureAwait(false);
        return Ok(contacts);
    }

    // Shows current user's leads
    [HttpGet("leads")]
    [Authorize(Roles = "Marketing")]
    public async Task<IActionResult> GetMyLeadsAsync()
    {
        // Retrieve leads only asynchronously
        var leads = await _contactService.GetLeadsAsync().ConfigureAwait(false);
        return Ok(leads);
    }

    // Creates a new contact
    [HttpPost]
    [Authorize(Roles = "Marketing")]
    public async Task<IActionResult> CreateContactAsync(ContactCreateDto contactDto)
    {
        // Create a new contact asynchronously
        var newContact = await _contactService.CreateContactAsync(contactDto);
        return Ok("Added\n" + newContact);
    }

    // Changes the status of a contact
    [HttpPatch("change-contact-status")]
    [Authorize(Roles = "Marketing")]
    public async Task<IActionResult> ChangeContactStatusAsync(string id, int newStatus)
    {
        // Change the status of a contact asynchronously
        var changedContact = await _contactService.ChangeStatusAsync(id, newStatus);
        if (changedContact == null)
        {
            return NotFound();
        }
        return Ok(changedContact);
    }

    // Updates an existing contact
    [HttpPut("change-contact")]
    [Authorize(Roles = "Marketing, Sales")]
    public async Task<IActionResult> ChangeContactAsync(int id, ContactChangeDto contactDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Update the contact asynchronously
        var result = await _contactService.UpdateContactAsync(id, contactDto);

        return result;
    }
}