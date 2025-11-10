namespace KentuckyUnemployment.Backend.Models;

public record WeeklyCertification(
    Guid CertificationId,
    DateOnly WeekEnding,
    bool AbleAndAvailable,
    bool ActivelySeekingWork,
    decimal GrossEarnings,
    IReadOnlyCollection<string> WorkSearchActivities,
    WeeklyCertificationStatus Status);
