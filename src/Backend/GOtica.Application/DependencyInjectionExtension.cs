using GOtica.Application.Sevices.Auth;
using GOtica.Application.Sevices.Invite;
using GOtica.Application.Sevices.Mapping;
using GOtica.Application.UseCases.Login.DoLogin;
using GOtica.Application.UseCases.OpticalStores.Deactivate;
using GOtica.Application.UseCases.OpticalStores.Get;
using GOtica.Application.UseCases.OpticalStores.GetAll;
using GOtica.Application.UseCases.OpticalStores.Register;
using GOtica.Application.UseCases.OpticalStores.TransferOwnership;
using GOtica.Application.UseCases.OpticalStores.Update;
using GOtica.Application.UseCases.Token.RefreshToken;
using GOtica.Application.UseCases.User.ChangePassword;
using GOtica.Application.UseCases.User.DeleteAccount;
using GOtica.Application.UseCases.User.Profile;
using GOtica.Application.UseCases.User.Register;
using GOtica.Application.UseCases.User.Update;
using GOtica.Application.UseCases.UserOpticalStore.Invite;
using Microsoft.Extensions.DependencyInjection;

namespace GOtica.Application;

public static class DependencyInjectionExtension
{
    extension(IServiceCollection services)
    {
        public void AddApplication()
        {
            AddUseCases(services);
            AddMapperConfigurations();
            AddTokenService(services);
            AddInviteTokenService(services);
        }
    }

    private static void AddUseCases(IServiceCollection services)
    {
        services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
        services.AddScoped<IDoLoginUseCase, DoLoginUseCase>();
        services.AddScoped<IRefreshTokenUseCase, RefreshTokenUseCase>();

        services.AddScoped<IChangePasswordUseCase, ChangePasswordUseCase>();
        services.AddScoped<IGetUserProfileUseCase, GetUserProfileUseCase>();
        services.AddScoped<IUpdateUserUseCase, UpdateUserUseCase>();
        services.AddScoped<IDeleteAccountUseCase, DeleteAccountUseCase>();

        services.AddScoped<ITransferOpticalStoreOwnershipUseCase, TransferOpticalStoreOwnershipUseCase>();
        services.AddScoped<IRegisterOpticalStoreUseCase, RegisterOpticalStoreUseCase>();
        services.AddScoped<IDeactivateOpticalStoreUseCase, DeactivateOpticalStoreUseCase>();
        services.AddScoped<IUpdateOpticalStoreUseCase, UpdateOpticalStoreUseCase>();
        services.AddScoped<IGetOpticalStoreUseCase, GetOpticalStoreUseCase>();
        services.AddScoped<IGetAllOpticalStoresUseCase, GetAllOpticalStoresUseCase>();

        services.AddScoped<ICreateInviteUseCase, CreateInviteUseCase>();
    }

    private static void AddMapperConfigurations() => MapConfigurations.Configure();

    private static void AddTokenService(IServiceCollection services)
    {
        services.AddOptions<TokenSettings>().BindConfiguration("Settings:RefreshToken");
        services.AddScoped<ITokenService, TokenService>();
    }

    private static void AddInviteTokenService(IServiceCollection services)
    {
        services.AddOptions<InviteTokenSettings>().BindConfiguration("Settings:InviteToken");
        services.AddScoped<IInviteTokenService, InviteTokenService>();
    }
}
