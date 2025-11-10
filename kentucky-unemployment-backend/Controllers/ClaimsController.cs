using KentuckyUnemployment.Backend.Dtos;
using KentuckyUnemployment.Backend.Models;
using KentuckyUnemployment.Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace KentuckyUnemployment.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClaimsController(IClaimService claimService) : ControllerBase
{
    private readonly IClaimService _claimService = claimService;

    [HttpGet]
    public ActionResult<IEnumerable<ClaimSummaryDto>> GetClaims()
    {
        var claims = _claimService
            .GetClaims()
            .Select(MapToSummary)
            .OrderByDescending(c => c.FiledDate);

        return Ok(claims);
    }

    [HttpGet("{claimId:guid}")]
    public ActionResult<UnemploymentClaim> GetClaim(Guid claimId)
    {
        var claim = _claimService.GetClaim(claimId);
        return claim is null ? NotFound() : Ok(claim);
    }

    [HttpPost]
    public ActionResult<UnemploymentClaim> CreateClaim([FromBody] CreateClaimRequest request)
    {
        if (request is null)
        {
            return BadRequest("Request body is required.");
        }

        var claim = _claimService.CreateClaim(request);

        return CreatedAtAction(nameof(GetClaim), new { claimId = claim.ClaimId }, claim);
    }

    [HttpPost("{claimId:guid}/weekly-certifications")]
    public ActionResult<WeeklyCertification> SubmitWeeklyCertification(Guid claimId, [FromBody] SubmitWeeklyCertificationRequest request)
    {
        if (request is null)
        {
            return BadRequest("Request body is required.");
        }

        var certification = _claimService.AddWeeklyCertification(claimId, request);
        return certification is null ? NotFound() : Ok(certification);
    }

    [HttpGet("{claimId:guid}/payments")]
    public ActionResult<IEnumerable<Payment>> GetPayments(Guid claimId)
    {
        var claim = _claimService.GetClaim(claimId);
        if (claim is null)
        {
            return NotFound();
        }

        return Ok(claim.Payments);
    }

    private static ClaimSummaryDto MapToSummary(UnemploymentClaim claim)
    {
        var weeksPaid = claim.Payments.Count(p => p.Status == PaymentStatus.Paid);

        return new ClaimSummaryDto(
            ClaimId: claim.ClaimId,
            ClaimNumber: claim.ClaimNumber,
            ClaimantName: $"{claim.Claimant.FirstName} {claim.Claimant.LastName}",
            FiledDate: claim.FiledDate,
            BenefitYearEnd: claim.BenefitYearEnd,
            Status: claim.Status,
            WeeklyBenefitAmount: claim.WeeklyBenefitAmount,
            WeeksCertified: claim.WeeklyCertifications.Count,
            WeeksPaid: weeksPaid);
    }
}
