using KentuckyUnemployment.Backend.Dtos;
using KentuckyUnemployment.Backend.Models;

namespace KentuckyUnemployment.Backend.Services;

public interface IClaimService
{
    IEnumerable<UnemploymentClaim> GetClaims();
    UnemploymentClaim? GetClaim(Guid claimId);
    UnemploymentClaim CreateClaim(CreateClaimRequest request);
    WeeklyCertification? AddWeeklyCertification(Guid claimId, SubmitWeeklyCertificationRequest request);
    IEnumerable<Payment> GetPayments(Guid claimId);
}
