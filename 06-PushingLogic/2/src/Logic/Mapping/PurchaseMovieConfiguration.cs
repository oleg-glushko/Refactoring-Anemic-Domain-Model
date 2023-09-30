using Logic.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Logic.Mapping;

public class PurchaseMovieConfiguration : IEntityTypeConfiguration<PurchasedMovie>
{
    public void Configure(EntityTypeBuilder<PurchasedMovie> builder)
    {
        builder.ToTable(nameof(PurchasedMovie));
        builder.Property(p => p.Id).HasColumnName(nameof(PurchasedMovie) + "ID");

        // necessary for ExpirationDateConverter
        builder.Property(p => p.ExpirationDate).IsRequired(false);
    }
}
