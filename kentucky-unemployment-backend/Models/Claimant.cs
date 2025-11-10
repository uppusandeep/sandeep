namespace KentuckyUnemployment.Backend.Models;

public record Claimant(
    Guid ClaimantId,
    string FirstName,
    string LastName,
    DateOnly DateOfBirth,
    string Email,
    string PhoneNumber,
    string County,
    string MailingAddress,
    string LastEmployer,
    DateOnly LastDayWorked);
