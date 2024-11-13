using task9.Models;

namespace task9.Services;

public interface IPrescriptionsService
{
    Task AddNewPrescriptionAsync(NewPrescriptionRequestDTO dto);
}