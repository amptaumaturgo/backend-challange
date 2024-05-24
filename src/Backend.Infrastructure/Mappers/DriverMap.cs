using Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Infrastructure.Mappers;

public class DriverMap : IEntityTypeConfiguration<Driver>
{
    public void Configure(EntityTypeBuilder<Driver> builder)
    {
        builder.ToTable("Drivers");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.Name)
            .IsRequired();

        builder.Property(x => x.UserId)
            .IsRequired();

        builder.Property(x => x.BirthDate)
            .IsRequired();

        builder.OwnsOne(x => x.Cnpj, cnpj =>
        {
            cnpj.Property(c => c.Value)
                .HasColumnName("Cnpj")
                .HasMaxLength(14)
                .IsRequired();

            cnpj.Ignore(x => x.Type);
        });
         
        builder.OwnsOne(x => x.DriverLicense, license =>
        {
            license.Property(l => l.Value)
                .HasColumnName("DriverLicense")
                .IsRequired();
             
            license.Ignore(x => x.Type);
        });

    }
}