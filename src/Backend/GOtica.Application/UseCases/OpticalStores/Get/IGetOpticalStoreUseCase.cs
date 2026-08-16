using GOtica.Communication.Response.OpticalStore;

namespace GOtica.Application.UseCases.OpticalStores.Get;

public interface IGetOpticalStoreUseCase
{
    Task<ResponseGetOpticalStore> Execute(Guid opticalStoreId);
}
