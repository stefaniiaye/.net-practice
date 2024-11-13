using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using task9.Entities;

namespace task9.Configs;

public class PrescriptionMedicamentConfiguration : IEntityTypeConfiguration<Prescription_Medicament>
{
    public void Configure(EntityTypeBuilder<Prescription_Medicament> modelBuilder)
    {
        modelBuilder
            .HasKey(x => new { x.IdPrescription, x.IdMedicament });
        
        modelBuilder
            .Property(x => x.Details)
            .HasMaxLength(100)
            .IsRequired();
        modelBuilder
            .Property(x => x.Dose)
            .IsRequired(false);
        modelBuilder
            .ToTable("Prescription_Medicament");

        modelBuilder
            .HasOne(x => x.Medicament)
            .WithMany(x => x.Prescriptions)
            .HasForeignKey(x => x.IdMedicament);
        
        modelBuilder
            .HasOne(x => x.Prescription)
            .WithMany(x => x.Medicaments)
            .HasForeignKey(x => x.IdPrescription);
    }
}