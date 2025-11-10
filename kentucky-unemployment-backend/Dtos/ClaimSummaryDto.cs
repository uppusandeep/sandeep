using KentuckyUnemployment.Backend.Models;

namespace KentuckyUnemployment.Backend.Dtos;

public record ClaimSummaryDto(
    Guid ClaimId,
    string ClaimNumber,
    string ClaimantName,
    DateOnly FiledDate,
    DateOnly BenefitYearEnd,
    ClaimStatus Status,
    decimal WeeklyBenefitAmount,
    int WeeksCertified,
    int WeeksPaid);
