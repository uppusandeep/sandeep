namespace KentuckyUnemployment.Backend.Models;

public record UnemploymentClaim(
    Guid ClaimId,
    string ClaimNumber,
    Claimant Claimant,
    DateOnly FiledDate,
    DateOnly BenefitYearEnd,
    decimal WeeklyBenefitAmount,
    ClaimStatus Status,
    IReadOnlyCollection<string> ProgramFlags,
    IReadOnlyCollection<Determination> Determinations,
    IReadOnlyCollection<WeeklyCertification> WeeklyCertifications,
    IReadOnlyCollection<Payment> Payments);
