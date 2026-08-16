using GOtica.Communication.Requests;
using GOtica.Communication.Response.OpticalStore;

namespace GOtica.Application.UseCases.OpticalStores.Register;

public interface IRegisterOpticalStoreUseCase
{
    Task<ResponseRegisterOpticalStore> Execute(RequestOpticalStore request);
}
