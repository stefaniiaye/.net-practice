using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using task9.Entities;

namespace task9.Configs;

public class DoctorConfiguration: IEntityTypeConfiguration<Doctor>
{
    public void Configure(EntityTypeBuilder<Doctor> modelBuilder)
    {
        modelBuilder
            .HasKey(x => x.IdDoctor);
        modelBuilder
            .Property(x => x.IdDoctor)
            .ValueGeneratedOnAdd()
            .IsRequired();
        modelBuilder
            .Property(x => x.FirstName)
            .HasMaxLength(100)
            .IsRequired();
        modelBuilder
            .Property(x => x.LastName)
            .HasMaxLength(100)
            .IsRequired();
        modelBuilder
            .Property(x => x.Email)
            .HasMaxLength(100)
            .IsRequired();
        modelBuilder
            .ToTable("Doctor");

        // modelBuilder
        //     .HasMany(x => x.Prescriptions)
        //     .WithOne(x => x.Doctor)
        //     .HasForeignKey(x => x.IdDoctor);
    }
}