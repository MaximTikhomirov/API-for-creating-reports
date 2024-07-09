using MAI.Backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace MAI.Backend.Controllers;

public partial class ReportController
{
    [HttpPut(nameof(UpdateReport))]
    public async Task<IActionResult> UpdateReport(Report report)
    {
        var result = await _reportService.UpdateReport(report);

        return Ok(result);
    }
}