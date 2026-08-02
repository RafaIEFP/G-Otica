using GOtica.Communication.Requests;
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
