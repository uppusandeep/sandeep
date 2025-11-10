using Batch.BusinessLogic.Interfaces;
using Batch.DAL;
using Ui.Shared.Models;

namespace Batch.BusinessLogic.Services;

public sealed class UnemploymentInsuranceService(IUnemploymentInsuranceRepository repository) : IUnemploymentInsuranceService
{
    private readonly IUnemploymentInsuranceRepository _repository = repository;

    public Task<IReadOnlyList<UnemploymentInsuranceBenefitDto>> GetBenefitSummaryAsync(CancellationToken cancellationToken = default) =>
        _repository.GetBenefitSummariesAsync(cancellationToken);

    public Task<IReadOnlyList<UnemploymentInsuranceCommonDto>> GetCommonSummaryAsync(CancellationToken cancellationToken = default) =>
        _repository.GetCommonSummariesAsync(cancellationToken);

    public Task<IReadOnlyList<UnemploymentInsuranceRevenueDto>> GetRevenueSummaryAsync(CancellationToken cancellationToken = default) =>
        _repository.GetRevenueSummariesAsync(cancellationToken);
}
