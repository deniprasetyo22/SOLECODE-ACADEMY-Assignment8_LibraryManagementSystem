using Assignment7.Application.DTOs;
using Assignment7.Persistence.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment7.Application.Interfaces.IService
{
    public interface IDashboardService
    {
        //Task<byte[]> DashboardReportPdf();
        //Task<int> GetTotalBook();
        //Task<List<string>> GetMostActiveMembers();
        //Task<IEnumerable<Borrow>> GetOverdueBooks();
        //Task<IEnumerable<NumberOfBooksPerCategory>> NumberOfBooksPerCategory();
        //Task<int> NumberOfProcessAsync();
        Task<List<ProcessDto>> GetAllProcessesAsync();
        Task<DashboardDto> GetDashboardReportAsync();
    }
}
