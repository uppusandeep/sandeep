namespace Ui.Shared.Models;

/// <summary>
///     Shared DTO describing the benefit accruals for unemployment insurance programs.
/// </summary>
public sealed record UnemploymentInsuranceBenefitDto(
    string ProgramCode,
    int ClaimCount,
    decimal TotalWeeklyBenefitAmount,
    decimal TotalExtendedBenefitAmount,
    decimal TotalDependentBenefitAmount);

/// <summary>
///     Shared DTO covering common operational attributes for unemployment insurance.
/// </summary>
public sealed record UnemploymentInsuranceCommonDto(
    string Jurisdiction,
    string ContributionRateSchedule,
    decimal TrustFundBalance,
    DateTime LastReconciledOnUtc,
    string ContactEmail);

/// <summary>
///     Shared DTO summarising revenue flowing into the unemployment insurance trust fund.
/// </summary>
public sealed record UnemploymentInsuranceRevenueDto(
    string EmployerId,
    string EmployerName,
    decimal Quarter1Contributions,
    decimal Quarter2Contributions,
    decimal Quarter3Contributions,
    decimal Quarter4Contributions);
