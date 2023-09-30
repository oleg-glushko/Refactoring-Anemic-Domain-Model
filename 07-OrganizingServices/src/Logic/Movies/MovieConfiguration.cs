using Logic.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Logic.Movies;

public class MovieConfiguration : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder.ToTable(nameof(Movie));
        builder.Property(p => p.Id).HasColumnName(nameof(Movie) + "ID");
        builder.HasDiscriminator<LicensingModel>(nameof(LicensingModel))
            .HasValue<TwoDaysMovie>(LicensingModel.TwoDays)
            .HasValue<LifeLongMovie>(LicensingModel.LifeLong);
    }
}