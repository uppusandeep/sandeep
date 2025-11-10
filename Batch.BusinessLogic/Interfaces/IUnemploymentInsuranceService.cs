using Ui.Shared.Models;

namespace Batch.BusinessLogic.Interfaces;

public interface IUnemploymentInsuranceService
{
    Task<IReadOnlyList<UnemploymentInsuranceBenefitDto>> GetBenefitSummaryAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<UnemploymentInsuranceCommonDto>> GetCommonSummaryAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<UnemploymentInsuranceRevenueDto>> GetRevenueSummaryAsync(CancellationToken cancellationToken = default);
}
