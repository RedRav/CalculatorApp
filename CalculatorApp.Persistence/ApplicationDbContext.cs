using CalculatorApp.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CalculatorApp.Persistence;
public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<CalculationLog> CalculationLogs { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }
}