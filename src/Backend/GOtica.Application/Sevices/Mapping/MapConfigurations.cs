using GOtica.Communication.Requests.User;
using GOtica.Communication.Response.Sale;
using GOtica.Domain.Dtos;
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

        TypeAdapterConfig<SaleDto, ResponseGetSale>
            .NewConfig()
            .Map(
                dest => dest.Client,
                src => new ResponseGetSaleClient
                {
                    Id = src.ClientId,
                    Name = src.ClientName
                })
            .Map(
                dest => dest.RegisteredBy,
                src => new ResponseGetSaleUser
                {
                    Id = src.UserId,
                    Name = src.UserName
                });

        TypeAdapterConfig<PaymentDto, ResponseGetSalePayment>
            .NewConfig()
            .Map(
                dest => dest.ReceivedBy,
                src => src.ReceivedByUserId.HasValue
                    ? new ResponseGetSaleUser
                    {
                        Id = src.ReceivedByUserId.Value,
                        Name = src.ReceivedByUserName!
                    }
                    : null);
    }
}
