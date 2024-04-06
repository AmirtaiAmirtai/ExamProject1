using AutoMapper;
using ExamProject.Models;
using ExamProject1.Dto;
using ExamProject1.Enums;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ExamProject1.Services
{
    public class LeadService
    {
        private readonly AppDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public LeadService(AppDbContext dbContext, IHttpContextAccessor httpContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContext;
            _mapper = mapper;
        }

        public async Task<List<Lead>> GetLeadsForCurrentUser()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

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

        public async Task<Lead> CreateLeadAsync(LeadCreateDto leadDto)
        {
            var newLead = _mapper.Map<Lead>(leadDto);

            var existingContact = await _dbContext.Contacts.FindAsync(newLead.ContactId);
            if (existingContact == null)
            {
                throw new InvalidOperationException("Contact not found");
            }

            if (existingContact.ContactStatus != ContactStatus.Lead)
            {
                throw new InvalidOperationException("Contact status is not Lead");
            }

            var existingLead = await _dbContext.Leads.FirstOrDefaultAsync(l => l.ContactId == newLead.ContactId);
            if (existingLead != null)
            {
                throw new InvalidOperationException("Lead with the same ContactId already exists");
            }

            _dbContext.Leads.Add(newLead);
            await _dbContext.SaveChangesAsync();

            return newLead;
        }
    }
}
