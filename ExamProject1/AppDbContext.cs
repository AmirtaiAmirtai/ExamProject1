using ExamProject.Models;
using ExamProject1.Models;
using Microsoft.EntityFrameworkCore;

namespace ExamProject1;

public class AppDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Sale> Sales => Set<Sale>();
    public DbSet<Lead> Leads => Set<Lead>();
    public DbSet<Contact> Contacts => Set<Contact>();

    public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) { }

}