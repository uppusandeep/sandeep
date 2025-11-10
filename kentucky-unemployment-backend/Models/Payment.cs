namespace KentuckyUnemployment.Backend.Models;

public record Payment(
    Guid PaymentId,
    DateOnly IssueDate,
    decimal Amount,
    string Method,
    PaymentStatus Status,
    string Notes);
