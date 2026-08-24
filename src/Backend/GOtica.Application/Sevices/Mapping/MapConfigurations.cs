using GOtica.Communication.Requests.User;
using GOtica.Communication.Response.OpticalStore;
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
    }
}
