using GOtica.Communication.Requests;
using GOtica.Communication.Response;
using GOtica.Domain.Entities;
using Mapster;

namespace GOtica.Application.Sevices.Mapping;

public static class MapConfigurations
{
    public static void Configure()
    {
        TypeAdapterConfig<RequestRegisterUser, User>
            .NewConfig()
            .Ignore(dest => dest.Password);

        TypeAdapterConfig<UserOpticalStore, ResponseOpticalStoreProfile>
            .NewConfig()
            .Map(dest => dest.Id, src => src.OpticalStoreId)
            .Map(dest => dest.Name, src => src.OpticalStore.Name);
    }
}
