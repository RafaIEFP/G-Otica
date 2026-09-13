namespace GOtica.Application.UseCases.Treatment.Reactivate;

public interface IReactivateTreatmentUseCase
{
    Task Execute(Guid opticalStoreId, Guid treatmentId);
}
