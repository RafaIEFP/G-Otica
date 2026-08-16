using GOtica.Communication.Response.OpticalStore;

namespace GOtica.Application.UseCases.OpticalStores.GetAll;

public interface IGetAllOpticalStoresUseCase
{
    Task<IReadOnlyCollection<ResponseGetAllOpticalStores>> Execute();
}
