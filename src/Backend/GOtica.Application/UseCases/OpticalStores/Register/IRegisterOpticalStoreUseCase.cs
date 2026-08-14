using GOtica.Communication.Requests;
using GOtica.Communication.Response;

namespace GOtica.Application.UseCases.OpticalStores.Register;

public interface IRegisterOpticalStoreUseCase
{
    Task<ResponseRegisterOpticalStore> Execute(RequestRegisterOpticalStore request);
}
