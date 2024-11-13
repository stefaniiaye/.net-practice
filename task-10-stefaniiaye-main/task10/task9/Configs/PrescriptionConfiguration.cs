using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using task9.Entities;

namespace task9.Configs;

public class PrescriptionConfiguration: IEntityTypeConfiguration<Prescription>
{
    public void Configure(EntityTypeBuilder<Prescription> modelBuilder)
    {
        modelBuilder
            .HasKey(x => x.IdPrescription);
        modelBuilder
            .Property(x => x.IdPrescription)
            .ValueGeneratedOnAdd()
            .IsRequired();
        modelBuilder
            .ToTable("Prescription");

        modelBuilder
            .HasOne(x => x.Doctor)
            .WithMany(x => x.Prescriptions)
            .HasForeignKey(x => x.IdDoctor);
        
        modelBuilder
            .HasOne(x => x.Patient)
            .WithMany(x => x.Prescriptions)
            .HasForeignKey(x => x.IdPatient);
    }
}