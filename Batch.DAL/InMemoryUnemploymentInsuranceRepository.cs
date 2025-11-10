using Ui.Shared.Models;

namespace Batch.DAL;

public sealed class InMemoryUnemploymentInsuranceRepository : IUnemploymentInsuranceRepository
{
    private static readonly IReadOnlyList<UnemploymentInsuranceBenefitDto> BenefitSeed = new[]
    {
        new UnemploymentInsuranceBenefitDto("UI-BASIC", 1200, 1_850_000.32m, 175_430.11m, 82_390.44m),
        new UnemploymentInsuranceBenefitDto("UI-EXTENDED", 140, 312_500.00m, 98_450.00m, 12_340.00m)
    };

    private static readonly IReadOnlyList<UnemploymentInsuranceCommonDto> CommonSeed = new[]
    {
        new UnemploymentInsuranceCommonDto("PA", "Schedule C", 987_654_321.45m, new DateTime(2025, 10, 31, 18, 34, 00, DateTimeKind.Utc), "uitrust@labor.gov"),
        new UnemploymentInsuranceCommonDto("NJ", "Schedule B", 543_210_987.00m, new DateTime(2025, 10, 30, 12, 00, 00, DateTimeKind.Utc), "ui.admin@labor.nj.gov")
    };

    private static readonly IReadOnlyList<UnemploymentInsuranceRevenueDto> RevenueSeed = new[]
    {
        new UnemploymentInsuranceRevenueDto("E-1001", "Acme Manufacturing", 1_200_000.00m, 1_150_000.00m, 1_300_000.00m, 1_450_000.00m),
        new UnemploymentInsuranceRevenueDto("E-2042", "Global Tech", 950_000.00m, 1_005_000.00m, 1_025_000.00m, 1_100_000.00m)
    };

    public Task<IReadOnlyList<UnemploymentInsuranceBenefitDto>> GetBenefitSummariesAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult(BenefitSeed);

    public Task<IReadOnlyList<UnemploymentInsuranceCommonDto>> GetCommonSummariesAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult(CommonSeed);

    public Task<IReadOnlyList<UnemploymentInsuranceRevenueDto>> GetRevenueSummariesAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult(RevenueSeed);
}
