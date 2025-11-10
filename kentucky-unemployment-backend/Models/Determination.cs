namespace KentuckyUnemployment.Backend.Models;

public record Determination(
    Guid DeterminationId,
    DateOnly IssuedOn,
    string Type,
    string Outcome,
    string Summary);
