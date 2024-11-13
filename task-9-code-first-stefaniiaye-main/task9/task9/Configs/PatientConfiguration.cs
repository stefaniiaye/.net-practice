using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using task9.Entities;

namespace task9.Configs;

public class PatientConfiguration: IEntityTypeConfiguration<Patient>
{
    public void Configure(EntityTypeBuilder<Patient> modelBuilder)
    {
        modelBuilder
            .HasKey(x => x.IdPatient);
        modelBuilder
            .Property(x => x.IdPatient)
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
            .Property(x => x.Birthdate)
            .IsRequired();
        modelBuilder
            .ToTable("Patient");
    }
}