namespace GOtica.Application.UseCases.Treatment.Deactivate;

public interface IDeactivateTreatmentUseCase
{
    Task Execute(Guid opticalStoreId, Guid treatmentId);
}
