namespace task9.Entities;

public class Prescription_Medicament
{
    public int IdPrescription { get; set; }
    public int IdMedicament { get; set; }
    public int? Dose { get; set; }
    public string Details { get; set; }
    
    public virtual Medicament Medicament { get; }
    public virtual Prescription Prescription { get; }
}