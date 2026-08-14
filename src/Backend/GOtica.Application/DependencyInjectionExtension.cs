using GOtica.Application.Sevices.Auth;
using GOtica.Application.Sevices.Mapping;
using GOtica.Application.UseCases.Login.DoLogin;
using GOtica.Application.UseCases.OpticalStores.Register;
using GOtica.Application.UseCases.OpticalStores.TransferOwnership;
using GOtica.Application.UseCases.Token.RefreshToken;
using GOtica.Application.UseCases.User.ChangePassword;
using GOtica.Application.UseCases.User.DeleteAccount;
using GOtica.Application.UseCases.User.Profile;
using GOtica.Application.UseCases.User.Register;
using GOtica.Application.UseCases.User.Update;
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
    }

    private static void AddMapperConfigurations() => MapConfigurations.Configure();

    private static void AddTokenService(IServiceCollection services)
    {
        services.AddOptions<TokenSettings>().BindConfiguration("Settings:RefreshToken");
        services.AddScoped<ITokenService, TokenService>();
    }
}
