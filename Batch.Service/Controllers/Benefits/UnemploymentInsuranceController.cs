using Batch.BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ui.Shared.Models;

namespace Batch.Service.Controllers.Benefits;

[ApiController]
[Route("api/benefits/unemployment-insurance")]
public sealed class UnemploymentInsuranceController(IUnemploymentInsuranceService unemploymentInsuranceService) : ControllerBase
{
    /// <summary>
    ///     Retrieves aggregate benefit metrics for unemployment insurance programs.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<UnemploymentInsuranceBenefitDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBenefitsAsync(CancellationToken cancellationToken)
    {
        var benefits = await unemploymentInsuranceService.GetBenefitSummaryAsync(cancellationToken);
        return Ok(benefits);
    }
}
