using AutoMapper;
using ExamProject.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ExamProject1.Services
{
    public class LeadService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LeadService(AppDbContext dbContext, IHttpContextAccessor httpContext)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContext;
        }

        public async Task<List<Lead>> GetLeadsForCurrentUser()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return new List<Lead>();
            }

            int intId = int.Parse(userId);
            var leads = await _dbContext.Leads
                .Where(l => l.SellerId == intId)
                .ToListAsync();

            return leads;
        }

        public async Task<Lead> ChangeStatusAsync(string leadId, int status)
        {
            if (!int.TryParse(leadId, out int leadIdInt))
            {
                throw new InvalidOperationException("Lead ID is invalid.");
            }

            Enums.LeadStatus newStatus = (Enums.LeadStatus)status;

            var lead = await _dbContext.Leads.FirstOrDefaultAsync(u => u.Id == leadIdInt);
            if (lead == null)
            {
                throw new InvalidOperationException("Lead not found.");
            }

            lead.LeadStatus = newStatus;
            await _dbContext.SaveChangesAsync();

            return lead;
        }
    }
}
