using Microsoft.AspNetCore.Mvc;

namespace MAI.Backend.Controllers;

public partial class ReportController
{
    [HttpDelete(nameof(DeleteReport))]
    public async Task<IActionResult> DeleteReport(int id)
    {
        var result = await _reportService.DeleteReport(id);

        return Ok(result);
    }
}