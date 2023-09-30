using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Logic.Customers;

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

        builder.OwnsOne(p => p.Status, nav =>
        {
            nav.Property(p => p.Type).HasColumnName("Status");
            nav.Property(p => p.ExpirationDate).HasColumnName("StatusExpirationDate");
            // necessary for ExpirationDateConverter
            nav.Property(p => p.ExpirationDate).IsRequired(false);
        });
    }
}
