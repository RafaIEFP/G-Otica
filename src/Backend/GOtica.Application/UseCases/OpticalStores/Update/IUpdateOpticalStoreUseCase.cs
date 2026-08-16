using GOtica.Communication.Requests;

namespace GOtica.Application.UseCases.OpticalStores.Update;

public interface IUpdateOpticalStoreUseCase
{
    Task Execute(Guid opticalStoreId, RequestOpticalStore request);
}
