using Logic.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Logic.Utils;

public class AppDbContext : DbContext
{
    public DbSet<Customer> Customers => Set<Customer>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<ExpirationDate>()
            .HaveConversion<ExpirationDateConverter>();

        configurationBuilder.Properties<Dollars>()
            .HaveConversion<DollarsConverter>();
    }
}
