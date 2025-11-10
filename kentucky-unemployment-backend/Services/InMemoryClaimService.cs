using System.Collections.Immutable;
using KentuckyUnemployment.Backend.Dtos;
using KentuckyUnemployment.Backend.Models;

namespace KentuckyUnemployment.Backend.Services;

public class InMemoryClaimService : IClaimService
{
    private readonly List<UnemploymentClaim> _claims = new();
    private readonly object _lock = new();
    private int _claimSequence = 2000;

    public InMemoryClaimService()
    {
        SeedClaims();
    }

    public IEnumerable<UnemploymentClaim> GetClaims()
    {
        lock (_lock)
        {
            return _claims
                .Select(claim => claim with
                {
                    WeeklyCertifications = claim.WeeklyCertifications.ToList(),
                    Payments = claim.Payments.ToList(),
                    Determinations = claim.Determinations.ToList(),
                    ProgramFlags = claim.ProgramFlags.ToList()
                })
                .ToList();
        }
    }

    public UnemploymentClaim? GetClaim(Guid claimId)
    {
        lock (_lock)
        {
            return _claims.FirstOrDefault(c => c.ClaimId == claimId);
        }
    }

    public UnemploymentClaim CreateClaim(CreateClaimRequest request)
    {
        var filedDate = DateOnly.FromDateTime(DateTime.UtcNow.Date);
        var benefitYearEnd = filedDate.AddYears(1).AddDays(-1);
        var weeklyBenefitAmount = CalculateWeeklyBenefit(request.AverageWeeklyWage, request.RequestedWeeklyBenefitAmount);

        var newClaim = new UnemploymentClaim(
            ClaimId: Guid.NewGuid(),
            ClaimNumber: GenerateClaimNumber(),
            Claimant: new Claimant(
                ClaimantId: Guid.NewGuid(),
                FirstName: request.Claimant.FirstName,
                LastName: request.Claimant.LastName,
                DateOfBirth: request.Claimant.DateOfBirth,
                Email: request.Claimant.Email,
                PhoneNumber: request.Claimant.PhoneNumber,
                County: request.Claimant.County,
                MailingAddress: request.Claimant.MailingAddress,
                LastEmployer: request.EmploymentHistory.EmployerName,
                LastDayWorked: request.EmploymentHistory.LastDayWorked),
            FiledDate: filedDate,
            BenefitYearEnd: benefitYearEnd,
            WeeklyBenefitAmount: weeklyBenefitAmount,
            Status: ClaimStatus.PendingEligibilityReview,
            ProgramFlags: ImmutableList.Create("State UI"),
            Determinations: ImmutableList<Determination>.Empty,
            WeeklyCertifications: ImmutableList<WeeklyCertification>.Empty,
            Payments: ImmutableList<Payment>.Empty);

        lock (_lock)
        {
            _claims.Add(newClaim);
        }

        return newClaim;
    }

    public WeeklyCertification? AddWeeklyCertification(Guid claimId, SubmitWeeklyCertificationRequest request)
    {
        lock (_lock)
        {
            var index = _claims.FindIndex(c => c.ClaimId == claimId);
            if (index < 0)
            {
                return null;
            }

            var claim = _claims[index];
            var certificationStatus = DetermineCertificationStatus(request, claim);
            var certification = new WeeklyCertification(
                CertificationId: Guid.NewGuid(),
                WeekEnding: request.WeekEnding,
                AbleAndAvailable: request.AbleAndAvailable,
                ActivelySeekingWork: request.ActivelySeekingWork,
                GrossEarnings: request.GrossEarnings,
                WorkSearchActivities: request.WorkSearchActivities.ToList(),
                Status: certificationStatus);

            var updatedCertifications = claim.WeeklyCertifications.ToList();
            updatedCertifications.Add(certification);

            var updatedPayments = claim.Payments.ToList();
            var updatedFlags = claim.ProgramFlags.ToList();
            var updatedStatus = claim.Status;

            if (certificationStatus == WeeklyCertificationStatus.Approved)
            {
                updatedPayments.Add(new Payment(
                    PaymentId: Guid.NewGuid(),
                    IssueDate: DateOnly.FromDateTime(DateTime.UtcNow.Date),
                    Amount: claim.WeeklyBenefitAmount,
                    Method: "Direct Deposit",
                    Status: PaymentStatus.Scheduled,
                    Notes: "Certification approved for payment."));

                if (!updatedFlags.Contains("Direct Deposit Active"))
                {
                    updatedFlags.Add("Direct Deposit Active");
                }

                updatedStatus = ClaimStatus.Active;
            }
            else
            {
                updatedStatus = ClaimStatus.DeterminationPending;
            }

            var updatedClaim = claim with
            {
                WeeklyCertifications = updatedCertifications,
                Payments = updatedPayments,
                ProgramFlags = updatedFlags,
                Status = updatedStatus
            };

            _claims[index] = updatedClaim;

            return certification;
        }
    }

    public IEnumerable<Payment> GetPayments(Guid claimId)
    {
        lock (_lock)
        {
            return _claims.FirstOrDefault(c => c.ClaimId == claimId)?.Payments
                   ?? Enumerable.Empty<Payment>();
        }
    }

    private void SeedClaims()
    {
        var filedDate = new DateOnly(2025, 1, 12);
        var claimant = new Claimant(
            ClaimantId: Guid.NewGuid(),
            FirstName: "Alexis",
            LastName: "Henderson",
            DateOfBirth: new DateOnly(1992, 4, 3),
            Email: "alexis.henderson@example.com",
            PhoneNumber: "(502) 555-0112",
            County: "Jefferson",
            MailingAddress: "123 Market Street, Louisville, KY 40202",
            LastEmployer: "Bluegrass Hospitality Group",
            LastDayWorked: new DateOnly(2024, 12, 28));

        var certifications = new List<WeeklyCertification>
        {
            new(
                CertificationId: Guid.NewGuid(),
                WeekEnding: new DateOnly(2025, 1, 18),
                AbleAndAvailable: true,
                ActivelySeekingWork: true,
                GrossEarnings: 0m,
                WorkSearchActivities: new[] { "Applied via KCC portal", "Attended Kentucky Career Center workshop" },
                Status: WeeklyCertificationStatus.Approved),
            new(
                CertificationId: Guid.NewGuid(),
                WeekEnding: new DateOnly(2025, 1, 25),
                AbleAndAvailable: true,
                ActivelySeekingWork: true,
                GrossEarnings: 110m,
                WorkSearchActivities: new[] { "Interviewed with Derby Logistics", "Applied on KYJobs program" },
                Status: WeeklyCertificationStatus.Approved)
        };

        var payments = new List<Payment>
        {
            new(
                PaymentId: Guid.NewGuid(),
                IssueDate: new DateOnly(2025, 1, 20),
                Amount: 498.50m,
                Method: "Direct Deposit",
                Status: PaymentStatus.Paid,
                Notes: "Issued via direct deposit ending -4123"),
            new(
                PaymentId: Guid.NewGuid(),
                IssueDate: new DateOnly(2025, 1, 27),
                Amount: 388.05m,
                Method: "Direct Deposit",
                Status: PaymentStatus.Paid,
                Notes: "Earnings deducted: $110.00")
        };

        var determinations = new List<Determination>
        {
            new(
                DeterminationId: Guid.NewGuid(),
                IssuedOn: new DateOnly(2025, 1, 22),
                Type: "Monetary",
                Outcome: "Approved",
                Summary: "Weekly benefit amount established at $498.50. Benefit year ends 01/11/2026."),
            new(
                DeterminationId: Guid.NewGuid(),
                IssuedOn: new DateOnly(2025, 1, 24),
                Type: "Separation",
                Outcome: "No Issue",
                Summary: "Employer confirmed layoff due to seasonal slowdown.")
        };

        var seededClaim = new UnemploymentClaim(
            ClaimId: Guid.NewGuid(),
            ClaimNumber: GenerateClaimNumber(),
            Claimant: claimant,
            FiledDate: filedDate,
            BenefitYearEnd: filedDate.AddYears(1).AddDays(-1),
            WeeklyBenefitAmount: 498.50m,
            Status: ClaimStatus.Active,
            ProgramFlags: new[] { "State UI", "Direct Deposit Active" },
            Determinations: determinations,
            WeeklyCertifications: certifications,
            Payments: payments);

        _claims.Add(seededClaim);
    }

    private string GenerateClaimNumber()
    {
        var sequence = Interlocked.Increment(ref _claimSequence);
        return $"KY-{DateTime.UtcNow:yyyy}-{sequence}";
    }

    private static decimal CalculateWeeklyBenefit(decimal averageWeeklyWage, decimal requestedAmount)
    {
        const decimal maxBenefit = 650m;
        var calculated = Math.Round(averageWeeklyWage * 0.45m, 2, MidpointRounding.AwayFromZero);
        var requested = Math.Round(requestedAmount, 2, MidpointRounding.AwayFromZero);
        return Math.Min(Math.Max(calculated, 50m), Math.Min(requested, maxBenefit));
    }

    private static WeeklyCertificationStatus DetermineCertificationStatus(SubmitWeeklyCertificationRequest request, UnemploymentClaim claim)
    {
        if (!request.AbleAndAvailable || !request.ActivelySeekingWork)
        {
            return WeeklyCertificationStatus.Denied;
        }

        if (request.GrossEarnings >= claim.WeeklyBenefitAmount * 1.5m)
        {
            return WeeklyCertificationStatus.Denied;
        }

        return WeeklyCertificationStatus.Approved;
    }
}
