namespace task9.Models;

public class PatientInfoDTO
{
    public int IdPatient { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime Birthdate { get; set; }
    public IEnumerable<PrescriptionDTO> Prescriptions { get; set; }
    
}