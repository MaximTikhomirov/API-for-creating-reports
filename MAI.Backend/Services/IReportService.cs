using MAI.Backend.Models;

namespace MAI.Backend.Services;

public interface IReportService
{
    Task<bool> CreateReport(Report report);
    Task<byte[]>  GetReport(int id);
    Task<byte[]> GetReportList();
    Task<Report> UpdateReport(Report report);
    Task<bool> DeleteReport(int key);
}