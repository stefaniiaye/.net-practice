namespace task9.Entities;

public class Prescription
{
    public int IdPrescription { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public int IdDoctor { get; set; }
    public int IdPatient { get; set; }
    
    public virtual Doctor Doctor { get; }
    public virtual Patient Patient { get; }
    public virtual ICollection<Prescription_Medicament> Medicaments { get; set; }
}