using GOtica.Application.Sevices.Auth;
using GOtica.Application.Sevices.Invite;
using GOtica.Application.Sevices.Mapping;
using GOtica.Application.UseCases.Client.Deactivate;
using GOtica.Application.UseCases.Client.Get;
using GOtica.Application.UseCases.Client.GetAll;
using GOtica.Application.UseCases.Client.Reactivate;
using GOtica.Application.UseCases.Client.Register;
using GOtica.Application.UseCases.Client.Update;
using GOtica.Application.UseCases.Invite.Accept;
using GOtica.Application.UseCases.Login.DoLogin;
using GOtica.Application.UseCases.Login.DoLogout;
using GOtica.Application.UseCases.OpticalStores.Deactivate;
using GOtica.Application.UseCases.OpticalStores.Get;
using GOtica.Application.UseCases.OpticalStores.GetAll;
using GOtica.Application.UseCases.OpticalStores.Register;
using GOtica.Application.UseCases.OpticalStores.TransferOwnership;
using GOtica.Application.UseCases.OpticalStores.Update;
using GOtica.Application.UseCases.Prescription.Get;
using GOtica.Application.UseCases.Prescription.Register;
using GOtica.Application.UseCases.Product.AdjustStock;
using GOtica.Application.UseCases.Product.Deactivate;
using GOtica.Application.UseCases.Product.Get;
using GOtica.Application.UseCases.Product.GetAll;
using GOtica.Application.UseCases.Product.Reactivate;
using GOtica.Application.UseCases.Product.Register;
using GOtica.Application.UseCases.Product.Update;
using GOtica.Application.UseCases.Purchase.Get;
using GOtica.Application.UseCases.Purchase.GetAll;
using GOtica.Application.UseCases.Purchase.Register;
using GOtica.Application.UseCases.StockMovement.GetAll;
using GOtica.Application.UseCases.Supplier.Deactivate;
using GOtica.Application.UseCases.Supplier.Get;
using GOtica.Application.UseCases.Supplier.GetAll;
using GOtica.Application.UseCases.Supplier.Reactivate;
using GOtica.Application.UseCases.Supplier.Register;
using GOtica.Application.UseCases.Supplier.Update;
using GOtica.Application.UseCases.Token.RefreshToken;
using GOtica.Application.UseCases.User.ChangePassword;
using GOtica.Application.UseCases.User.DeleteAccount;
using GOtica.Application.UseCases.User.Profile;
using GOtica.Application.UseCases.User.Reactivate;
using GOtica.Application.UseCases.User.Register;
using GOtica.Application.UseCases.User.Update;
using GOtica.Application.UseCases.UserOpticalStore.Invite.Create;
using GOtica.Application.UseCases.UserOpticalStore.Invite.Validade;
using GOtica.Application.UseCases.UserOpticalStores.ChangeRole;
using GOtica.Application.UseCases.UserOpticalStores.Deactivate;
using GOtica.Application.UseCases.UserOpticalStores.GetAll;
using GOtica.Application.UseCases.UserOpticalStores.Reactivate;
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
        #region Auth
        services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
        services.AddScoped<IDoLoginUseCase, DoLoginUseCase>();
        services.AddScoped<IRefreshTokenUseCase, RefreshTokenUseCase>();
        services.AddScoped<IDoLogoutUseCase, DoLogoutUseCase>();
        #endregion

        #region User
        services.AddScoped<IChangePasswordUseCase, ChangePasswordUseCase>();
        services.AddScoped<IGetUserProfileUseCase, GetUserProfileUseCase>();
        services.AddScoped<IUpdateUserUseCase, UpdateUserUseCase>();
        services.AddScoped<IDeleteAccountUseCase, DeleteAccountUseCase>();
        services.AddScoped<IReactivateUserUseCase, ReactivateUserUseCase>();
        #endregion

        #region OpticalStore
        services.AddScoped<ITransferOpticalStoreOwnershipUseCase, TransferOpticalStoreOwnershipUseCase>();
        services.AddScoped<IRegisterOpticalStoreUseCase, RegisterOpticalStoreUseCase>();
        services.AddScoped<IDeactivateOpticalStoreUseCase, DeactivateOpticalStoreUseCase>();
        services.AddScoped<IUpdateOpticalStoreUseCase, UpdateOpticalStoreUseCase>();
        services.AddScoped<IGetOpticalStoreUseCase, GetOpticalStoreUseCase>();
        services.AddScoped<IGetAllOpticalStoresUseCase, GetAllOpticalStoresUseCase>();
        #endregion

        #region Invite
        services.AddScoped<ICreateInviteUseCase, CreateInviteUseCase>();
        services.AddScoped<IValidateInviteUseCase, ValidateInviteUseCase>();
        services.AddScoped<IAcceptInviteUseCase, AcceptInviteUseCase>();
        #endregion

        #region UserOpticalStore
        services.AddScoped<IGetAllOpticalStoreUsersUseCase, GetAllOpticalStoreUsersUseCase>();
        services.AddScoped<IChangeRoleUseCase, ChangeRoleUseCase>();
        services.AddScoped<IDeactivateUserOpticalStoreUseCase, DeactivateUserOpticalStoreUseCase>();
        services.AddScoped<IReactivateUserOpticalStoreUseCase, ReactivateUserOpticalStoreUseCase>();
        #endregion

        #region Client
        services.AddScoped<IRegisterClientUseCase, RegisterClientUseCase>();
        services.AddScoped<IGetClientUseCase, GetClientUseCase>();
        services.AddScoped<IGetAllClientsUseCase, GetAllClientsUseCase>();
        services.AddScoped<IUpdateClientUseCase, UpdateClientUseCase>();
        services.AddScoped<IDeactivateClientUseCase, DeactivateClientUseCase>();
        services.AddScoped<IReactivateClientUseCase, ReactivateClientUseCase>();
        #endregion

        #region Product
        services.AddScoped<IRegisterProductUseCase, RegisterProductUseCase>();
        services.AddScoped<IGetProductUseCase, GetProductUseCase>();
        services.AddScoped<IGetAllProductsUseCase, GetAllProductsUseCase>();
        services.AddScoped<IUpdateProductUseCase, UpdateProductUseCase>();
        services.AddScoped<IDeactivateProductUseCase, DeactivateProductUseCase>();
        services.AddScoped<IReactivateProductUseCase, ReactivateProductUseCase>();
        services.AddScoped<IAdjustProductStockUseCase, AdjustProductStockUseCase>();
        #endregion

        #region StockMovement
        services.AddScoped<IGetAllStockMovementsUseCase, GetAllStockMovementsUseCase>();
        #endregion

        #region Supplier
        services.AddScoped<IRegisterSupplierUseCase, RegisterSupplierUseCase>();
        services.AddScoped<IGetSupplierUseCase, GetSupplierUseCase>();
        services.AddScoped<IGetAllSuppliersUseCase, GetAllSuppliersUseCase>();
        services.AddScoped<IUpdateSupplierUseCase, UpdateSupplierUseCase>();
        services.AddScoped<IDeactivateSupplierUseCase, DeactivateSupplierUseCase>();
        services.AddScoped<IReactivateSupplierUseCase, ReactivateSupplierUseCase>();
        #endregion

        #region Purchase
        services.AddScoped<IRegisterPurchaseUseCase, RegisterPurchaseUseCase>();
        services.AddScoped<IGetPurchaseUseCase, GetPurchaseUseCase>();
        services.AddScoped<IGetAllPurchasesUseCase, GetAllPurchasesUseCase>();
        #endregion

        #region Prescription
        services.AddScoped<IRegisterPrescriptionUseCase, RegisterPrescriptionUseCase>();
        services.AddScoped<IGetPrescriptionUseCase, GetPrescriptionUseCase>();
        #endregion
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
