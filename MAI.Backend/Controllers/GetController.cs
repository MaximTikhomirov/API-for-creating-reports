using Microsoft.AspNetCore.Mvc;

namespace MAI.Backend.Controllers;


public partial class ReportController
{
    [HttpGet(nameof(GetFullReport))]
    public async Task<IActionResult> GetFullReport()
    {
        var pdfBytes = await _reportService.GetReportList();
        return File(pdfBytes, "application/pdf", "ReportList.pdf");
    }

    [HttpGet(nameof(GetReport))]
    public async Task<IActionResult> GetReport(int id)
    {
        var pdfBytes = await _reportService.GetReport(id);
        if (pdfBytes == null)
        {
            return NotFound();
        }

        return File(pdfBytes, "application/pdf", $"Report_{id}.pdf");
    }
}