namespace KentuckyUnemployment.Backend.Dtos;

public record CreateClaimRequest(
    ClaimantProfile Claimant,
    EmploymentHistory EmploymentHistory,
    decimal AverageWeeklyWage,
    decimal RequestedWeeklyBenefitAmount,
    string PreferredLanguage,
    string PreferredContactMethod);

public record ClaimantProfile(
    string FirstName,
    string LastName,
    DateOnly DateOfBirth,
    string Email,
    string PhoneNumber,
    string County,
    string MailingAddress);

public record EmploymentHistory(
    string EmployerName,
    DateOnly StartDate,
    DateOnly LastDayWorked,
    string SeparationReason);
