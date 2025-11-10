namespace KentuckyUnemployment.Backend.Dtos;

public record SubmitWeeklyCertificationRequest(
    DateOnly WeekEnding,
    bool AbleAndAvailable,
    bool ActivelySeekingWork,
    decimal GrossEarnings,
    IReadOnlyCollection<string> WorkSearchActivities);
