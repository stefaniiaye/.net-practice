namespace task9.Models;

public class PrescriptionDTO
{
    public int IdPrescription { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public IEnumerable<MedicamentDTOforPatient> Medicaments { get; set; }
    public DoctorDTO Doctor { get; set; }
}