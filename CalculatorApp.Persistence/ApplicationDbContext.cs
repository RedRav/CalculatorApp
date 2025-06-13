using Microsoft.EntityFrameworkCore;
using CalculatorApp.Domain.Entities;

namespace CalculatorApp.Persistence;

public class ApplicationDbContext : DbContext
{
    public DbSet<CalculationLog> CalculationLogs { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}