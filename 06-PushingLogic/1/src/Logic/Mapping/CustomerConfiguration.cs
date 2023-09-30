using Logic.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Logic.Mapping;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable(nameof(Customer));
        builder.Property(p => p.Id).HasColumnName(nameof(Customer) + "ID");

        builder.Property(p => p.Name).HasMaxLength(100);
        builder.Property(p => p.Name).HasConversion(
            v => (string)v,
            v => (CustomerName)v);

        builder.Property(p => p.Email).HasConversion(
            v => (string)v,
            v => (Email)v);

        // necessary for ExpirationDateConverter
        builder.Property(p => p.StatusExpirationDate).IsRequired(false);
    }
}
