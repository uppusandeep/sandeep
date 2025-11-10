using Batch.BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ui.Shared.Models;

namespace Batch.Service.Controllers.Common;

[ApiController]
[Route("api/common/unemployment-insurance")]
public sealed class UnemploymentInsuranceController(IUnemploymentInsuranceService unemploymentInsuranceService) : ControllerBase
{
    /// <summary>
    ///     Provides access to shared unemployment insurance operational metadata.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<UnemploymentInsuranceCommonDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCommonOperationalDataAsync(CancellationToken cancellationToken)
    {
        var response = await unemploymentInsuranceService.GetCommonSummaryAsync(cancellationToken);
        return Ok(response);
    }
}
