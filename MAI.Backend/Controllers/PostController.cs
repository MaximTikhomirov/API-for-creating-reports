using MAI.Backend.Models;
using MAI.Backend.Services;

using Microsoft.AspNetCore.Mvc;

namespace MAI.Backend.Controllers;

[ApiController]
[Route("[controller]")]
public partial class ReportController : Controller
{
    private readonly IReportService _reportService;
    
    public ReportController(IReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpPost(nameof(AddReport))]
    public async Task<IActionResult> AddReport(Report report)
    {
        var result =  await _reportService.CreateReport(report);

        return Ok(result);
    }
}