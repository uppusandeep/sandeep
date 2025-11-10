using Batch.BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ui.Shared.Models;

namespace Batch.Service.Controllers.Revenue;

[ApiController]
[Route("api/revenue/unemployment-insurance")]
public sealed class UnemploymentInsuranceController(IUnemploymentInsuranceService unemploymentInsuranceService) : ControllerBase
{
    /// <summary>
    ///     Returns contribution revenue broken down by quarter for unemployment insurance.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<UnemploymentInsuranceRevenueDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRevenueAsync(CancellationToken cancellationToken)
    {
        var revenue = await unemploymentInsuranceService.GetRevenueSummaryAsync(cancellationToken);
        return Ok(revenue);
    }
}
