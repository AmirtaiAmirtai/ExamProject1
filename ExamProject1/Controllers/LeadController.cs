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
    [Authorize(Roles = "Sales")]
    [HttpGet("user-leads")]
    public async Task<IActionResult> GetMyLeads()
    {
        var leads = await _leadService.GetLeadsForCurrentUser();

        return Ok(leads);
    }

    [Authorize(Roles = "Admin")]
    [HttpPatch("change-role")]
    public async Task<IActionResult> ChangeLeadStatus(string id, int status)
    {
        await _leadService.ChangeStatusAsync(id, status);
        return Ok($"Lead status changed successfully to {status}");
    }
}
