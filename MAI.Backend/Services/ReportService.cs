using iTextSharp.text;
using iTextSharp.text.pdf;
using MAI.Backend.Models;

namespace MAI.Backend.Services;

public class ReportService : IReportService
{
    private readonly IDbService _dbService;
    
    public ReportService(IDbService dbService)
    {
        _dbService = dbService;
    }

    public async Task<bool> CreateReport(Report report)
    {
        var result =
            await _dbService.EditData(
                "INSERT INTO public.report (id, name, amount, price) VALUES (@Id, @Name, @Amount, @Price)",
                report);
        return true;
    }

    public async Task<byte[]> GetReportList()
    {
        var reportList = await _dbService.GetAll<Report>("SELECT * FROM report", new { });

        using (var memoryStream = new MemoryStream())
        {
            Document document = new Document();
            PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
            document.Open();

            // All Products
            document.Add(new Paragraph("All Products"));
            document.Add(CreateTable(reportList, new string[] { "Product Name", "Unit Price", "Units Sold", "Revenue" }));

            // Products Sorted by Unit Price
            var sortedByUnitPrice = reportList.OrderByDescending(r => r.Price).ToList();
            document.Add(new Paragraph("Products Sorted by Unit Price"));
            document.Add(CreateTable(sortedByUnitPrice, new string[] { "Product Name", "Unit Price", "Units Sold", "Revenue" }));

            // Products Sorted by Units Sold
            var sortedByAmount = reportList.OrderByDescending(r => r.Amount).ToList();
            document.Add(new Paragraph("Products Sorted by Units Sold"));
            document.Add(CreateTable(sortedByAmount, new string[] { "Product Name", "Unit Price", "Units Sold", "Revenue" }));

            // Products Sorted by Revenue
            var sortedByRevenue = reportList.OrderByDescending(r => r.Amount * r.Price).ToList();
            document.Add(new Paragraph("Products Sorted by Revenue"));
            document.Add(CreateTable(sortedByRevenue, new string[] { "Product Name", "Unit Price", "Units Sold", "Revenue" }));

            // Total Revenue and Total Units Sold
            var totalRevenue = reportList.Sum(r => r.Amount * r.Price);
            var totalUnitsSold = reportList.Sum(r => r.Amount);
            document.Add(new Paragraph($"Total Revenue: {totalRevenue}"));
            document.Add(new Paragraph($"Total Units Sold: {totalUnitsSold}"));

            document.Close();
            writer.Close();

            return memoryStream.ToArray();
        }
    }

    private PdfPTable CreateTable(List<Report> reports, string[] headers)
    {
        PdfPTable table = new PdfPTable(headers.Length);
        table.WidthPercentage = 100;

        // Add headers
        foreach (var header in headers)
        {
            table.AddCell(header);
        }

        // Add data
        foreach (var report in reports)
        {
            table.AddCell(report.Name);
            table.AddCell(report.Price.ToString());
            table.AddCell(report.Amount.ToString());
            table.AddCell((report.Amount * report.Price).ToString());
        }

        return table;
    }

    public async Task<byte[]> GetReport(int id)
    {
        var report = await _dbService.GetAsync<Report>("SELECT * FROM report WHERE id=@id", new { id });
        if (report == null)
        {
            return null;
        }

        using (var memoryStream = new MemoryStream())
        {
            Document document = new Document();
            PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
            document.Open();
            

            document.Add(new Paragraph($"Product name: {report.Name}"));
            document.Add(new Paragraph($"Unit price : {report.Price}"));
            document.Add(new Paragraph($"The number of units sold: {report.Amount}"));
            document.Add(new Paragraph($"Revenue: {report.Amount * report.Price}"));
            
            document.Close();
            writer.Close();

            return memoryStream.ToArray();
        }
    }

    public async Task<Report> UpdateReport(Report report)
    {
        var updateReport =
            await _dbService.EditData(
                "UPDATE report SET name=@Name, amount=@Amount, price=@Price WHERE id=@Id",
                report);
        return report;
    }

    public async Task<bool> DeleteReport(int id)
    {
        var deleteReport = await _dbService.EditData("DELETE FROM report WHERE id=@Id", new { id });
        return true;
    }
}