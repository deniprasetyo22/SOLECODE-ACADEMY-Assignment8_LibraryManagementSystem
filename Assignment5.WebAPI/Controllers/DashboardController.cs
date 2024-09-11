using Assignment7.Application.Interfaces.IService;
using Assignment7.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Assignment7.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;
        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetDashboardReport()
        {
            try
            {
                var report = await _dashboardService.GetDashboardReportAsync();
                return Ok(report);
            }
            catch (Exception ex)
            {
                // Log exception and return error response
                return StatusCode(500, "Internal server error");
            }
        }

        //[HttpGet("get-total-books")]
        //public async Task<IActionResult> GetTotalBook()
        //{
        //    var totalBook = await _dashboardService.GetTotalBook();
        //    return Ok(totalBook);
        //}

        //[HttpGet("most-active-members")]
        //public async Task<IActionResult> GetMostActiveMembers()
        //{
        //    var mostActiveMembers = await _dashboardService.GetMostActiveMembers();
        //    return Ok(mostActiveMembers);
        //}

        //[HttpGet("overdue-books")]
        //public async Task<IActionResult> GetOverdueBooks()
        //{
        //    var overdueBooks = await _dashboardService.GetOverdueBooks();
        //    return Ok(overdueBooks);
        //}

        //[HttpGet("NumberOfBooksPerCategory")]
        //public async Task<IActionResult> NumberOfBooksPerCategory()
        //{
        //    var result = await _dashboardService.NumberOfBooksPerCategory();
        //    if (result == null || !result.Any())
        //    {
        //        return NotFound("Tidak ada kategori buku yang ditemukan.");
        //    }

        //    return Ok(result);
        //}

        //[HttpGet("number-of-process")]
        //public async Task<IActionResult> GetNumberOfProcess()
        //{
        //    try
        //    {
        //        var count = await _dashboardService.NumberOfProcessAsync();
        //        return Ok(count);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Tangani kesalahan
        //        return StatusCode(500, $"Terjadi kesalahan: {ex.Message}");
        //    }
        //}

        //[HttpGet("get-processes")]
        //public async Task<IActionResult> GetAllProcesses()
        //{
        //    try
        //    {
        //        var processes = await _dashboardService.GetAllProcessesAsync();
        //        return Ok(processes);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Tangani kesalahan
        //        return StatusCode(500, $"Terjadi kesalahan: {ex.Message}");
        //    }
        //}

        //[HttpGet("report")]
        //public async Task<IActionResult> DashboardReport()
        //{
        //    var Filename = "DashboardReport.pdf";
        //    var file = await _dashboardService.DashboardReportPdf();
        //    return File(file, "application/pdf", Filename);
        //}
    }
}
