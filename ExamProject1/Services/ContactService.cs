using AutoMapper;
using ExamProject1.Dto;
using ExamProject1.Enums;
using ExamProject1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExamProject1.Services
{
    public class ContactService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public ContactService(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<List<Contact>> GetAllAsync()
        {
            var contacts = _dbContext.Contacts.ToList();
            return contacts;
        }
        public async Task<List<Contact>> GetLeadsAsync()
        {
            var leadContacts = _dbContext.Contacts.Where(x => x.ContactStatus == (ContactStatus)2).ToList();
            return leadContacts;
        }
        public async Task<Contact> CreateContactAsync(ContactCreateDto contactDto)
        {
            _dbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Contacts ON");
            if (await _dbContext.Contacts.AnyAsync(u => u.Email == contactDto.Email))
                throw new InvalidOperationException("Contact with this email already exists");

            var newContact = _mapper.Map<Contact>(contactDto);

            _dbContext.Contacts.Add(newContact);
            await _dbContext.SaveChangesAsync();
            _dbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Contacts OFF");
            return newContact;
        }
        public async Task<Contact> ChangeStatusAsync(string contactId, int status)
        {
            if (!int.TryParse(contactId, out int contactIdInt))
            {
                throw new InvalidOperationException("User not found");
            }
            Enums.ContactStatus newStatus = (Enums.ContactStatus)status;

            var contact = await _dbContext.Contacts.FirstOrDefaultAsync(u => u.Id == contactIdInt); 

            if (contact != null)
            {
                
                contact.ContactStatus = newStatus;
                await _dbContext.SaveChangesAsync();
            }

            return contact;
        }

        public async Task<IActionResult> UpdateContact(int contactId, ContactChangeDto contactDto)
        {
            var existingContact = await _dbContext.Contacts.FindAsync(contactId);

            if (existingContact == null)
            {
                return new NotFoundResult();
            }

            _mapper.Map(contactDto, existingContact);

            await _dbContext.SaveChangesAsync();

            return new OkResult();
        }
    }
}
