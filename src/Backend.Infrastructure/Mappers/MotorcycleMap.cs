using Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Infrastructure.Mappers;

public class MotorcycleMap : IEntityTypeConfiguration<Motorcycle>
{
    public void Configure(EntityTypeBuilder<Motorcycle> builder)
    {
        builder.ToTable("Motorcycles");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Year)
            .IsRequired();

        builder.Property(x => x.Model)
            .IsRequired();

        builder.Property(x => x.Plate)
            .HasMaxLength(7)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.HasIndex(x => x.Year);

        builder.HasIndex(x => x.Plate);
    }
}

public class PlanMap : IEntityTypeConfiguration<Plan>
{
    public void Configure(EntityTypeBuilder<Plan> builder)
    {
        builder.ToTable("Plans");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.Days)
            .IsRequired();

        builder.Property(x => x.PricePerDay)
            .HasColumnType("DECIMAL(18,2)")
            .IsRequired();
    }
}

public class RentMap : IEntityTypeConfiguration<Rent>
{
    public void Configure(EntityTypeBuilder<Rent> builder)
    {
        builder.ToTable("Rents");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.StartDate)
            .IsRequired();

        builder.Property(x => x.ExpectedEndDate)
            .IsRequired();

        builder.Property(x => x.EndDate)
            .IsRequired(false);

        builder.HasOne(x => x.Plan)
            .WithMany()
            .HasForeignKey(x => x.PlanId);

        builder.HasOne(x => x.Driver)
            .WithMany()
            .HasForeignKey(x => x.DriverId);

        builder.HasOne(x => x.Motorcycle)
            .WithMany()
            .HasForeignKey(x => x.MotorcycleId);
    }
}

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