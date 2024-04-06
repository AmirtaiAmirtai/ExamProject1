using ExamProject1.Dto;
using ExamProject1.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExamProject1.Controllers;

[ApiController]
[Route("api/Lead")]
public class LeadController : ControllerBase
{
    private readonly LeadService _leadService;

    public LeadController(LeadService leadService)
    {
        _leadService = leadService ?? throw new ArgumentNullException(nameof(leadService));
    }
    // Retrieves leads assigned to the current sales user
    [Authorize(Roles = "Sales")]
    [HttpGet("my-leads")]
    public async Task<IActionResult> GetMyLeads()
    {
        // Get leads assigned to the current sales user asynchronously
        var leads = await _leadService.GetLeadsForCurrentUser();

        return Ok(leads);
    }

    // Creates a new lead
    [Authorize(Roles = "Sales")]
    [HttpPost]
    public async Task<IActionResult> CreateLead(LeadCreateDto leadCreate)
    {
        // Create a new lead asynchronously
        await _leadService.CreateLeadAsync(leadCreate);
        return Ok(leadCreate);
    }

    // Changes the status of a lead
    [Authorize(Roles = "Admin")]
    [HttpPatch("role")]
    public async Task<IActionResult> ChangeLeadStatus(string id, int status)
    {
        // Change the status of a lead asynchronously
        await _leadService.ChangeStatusAsync(id, status);
        return Ok($"Lead status changed successfully to {status}");
    }
}
