using GOtica.Domain.Repositories;
using GOtica.Domain.Repositories.OpticalStore;
using GOtica.Domain.Repositories.UserOpticalStore;

namespace GOtica.Application.UseCases.OpticalStores.Deactivate;

public class DeactivateOpticalStoreUseCase : IDeactivateOpticalStoreUseCase
{
    private readonly IOpticalStoreUpdateOnlyRepository _opticalStoreUpdateOnlyRepository;
    private readonly IUserOpticalStoreUpdateOnlyRepository _userOpticalStoreUpdateOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeactivateOpticalStoreUseCase(
        IOpticalStoreUpdateOnlyRepository opticalStoreUpdateOnlyRepository,
        IUserOpticalStoreUpdateOnlyRepository userOpticalStoreUpdateOnlyRepository,
        IUnitOfWork unitOfWork)
    {
        _opticalStoreUpdateOnlyRepository = opticalStoreUpdateOnlyRepository;
        _userOpticalStoreUpdateOnlyRepository = userOpticalStoreUpdateOnlyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(Guid opticalStoreId)
    {
        await _unitOfWork.ExecuteInTransaction(async () =>
        {
            await _opticalStoreUpdateOnlyRepository.DeactivateOpticalStore(opticalStoreId);

            await _userOpticalStoreUpdateOnlyRepository.DeactivateByOpticalStore(opticalStoreId);
        });
    }
}
