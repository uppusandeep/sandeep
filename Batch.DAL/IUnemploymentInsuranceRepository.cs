using Ui.Shared.Models;

namespace Batch.DAL;

public interface IUnemploymentInsuranceRepository
{
    Task<IReadOnlyList<UnemploymentInsuranceBenefitDto>> GetBenefitSummariesAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<UnemploymentInsuranceCommonDto>> GetCommonSummariesAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<UnemploymentInsuranceRevenueDto>> GetRevenueSummariesAsync(CancellationToken cancellationToken = default);
}
