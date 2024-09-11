using Assignment5.Application.Interfaces.IRepositories;
using Assignment7.Application.DTOs;
using Assignment7.Application.Interfaces.IRepositories;
using Assignment7.Application.Interfaces.IService;
using Assignment7.Persistence.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PdfSharpCore.Pdf;
using PdfSharpCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheArtOfDev.HtmlRenderer.Core;
using TheArtOfDev.HtmlRenderer.PdfSharp;

namespace Assignment7.Application.Services
{
    public class DashboardService:IDashboardService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IBorrowRepository _borrowRepository;
        private readonly IBookRequestRepository _bookRequestRepository;
        public DashboardService(IBookRepository bookRepository, IBorrowRepository borrowRepository, IBookRequestRepository bookRequestRepository)
        {
            _bookRepository = bookRepository;
            _borrowRepository = borrowRepository;
            _bookRequestRepository = bookRequestRepository;
        }

        public async Task<DashboardDto> GetDashboardReportAsync()
        {
            var totalBooks = await _bookRepository.GetTotalBook();
            var mostActiveMembers = await _borrowRepository.GetMostActiveMembers();
            var overdueBooks = await _borrowRepository.GetOverdueBooks();
            var booksPerCategory = await _bookRepository.NumberOfBooksPerCategory();
            var numberOfProcesses = await _bookRequestRepository.NumberOfProcessAsync();

            return new DashboardDto
            {
                TotalBooks = totalBooks,
                MostActiveMembers = mostActiveMembers,
                OverdueBooks = overdueBooks,
                BooksPerCategory = booksPerCategory,
                NumberOfProcesses = numberOfProcesses
            };
        }

        public async Task<List<ProcessDto>> GetAllProcessesAsync()
        {
            return await _bookRequestRepository.GetAllProcessesAsync();
        }

        //public async Task<byte[]> DashboardReportPdf()
        //{
        //    var totalBooks = await _bookRepository.GetTotalBook();
        //    var mostActiveMembers = await _borrowRepository.GetMostActiveMembers();
        //    var overdueBooks = await _borrowRepository.GetOverdueBooks();
        //    var booksPerCategory = await _bookRepository.NumberOfBooksPerCategory();
        //    var allProcesses = await _bookRequestRepository.GetAllProcessesAsync();

        //    string htmlContent = "<h1 style='text-align: center;'>Dashboard Report</h1>";

        //    // Book Total Section
        //    htmlContent += "<h2>Total Books</h2><table>";
        //    htmlContent += "<tr><th>Total Books</th></tr>";
        //    htmlContent += $"<tr><td>{totalBooks}</td></tr>";
        //    htmlContent += "</table>";

        //    // Most Active Members Section
        //    htmlContent += "<h2>Most Active Members</h2><table>";
        //    htmlContent += "<tr><th>Name</th></tr>";
        //    foreach (var member in mostActiveMembers)
        //    {
        //        htmlContent += $"<tr><td>{member}</td></tr>";
        //    }
        //    htmlContent += "</table>";

        //    // Overdue Books Section
        //    htmlContent += "<h2>Overdue Books</h2><table>";
        //    htmlContent += "<tr><th>User Name</th><th>Book Id</th><th>Deadline Return</th><th>Date of Return</th></tr>";
        //    foreach (var book in overdueBooks)
        //    {
        //        htmlContent += $"<tr><td>{book.Userid}</td><td>{book.Bookid}</td><td>{book.Deadlinereturn}</td><td>{book.Dateofreturn}</td></tr>";
        //    }
        //    htmlContent += "</table>";

        //    // Books Per Category Section
        //    htmlContent += "<h2>Number of Books Per Category</h2><table>";
        //    htmlContent += "<tr><th>Category</th><th>Number of Books</th></tr>";
        //    foreach (var category in booksPerCategory)
        //    {
        //        htmlContent += $"<tr><td>{category.Category}</td><td>{category.TotalBooks}</td></tr>";
        //    }
        //    htmlContent += "</table>";

        //    // All Processes Section
        //    htmlContent += "<h2>All Processes</h2><table>";
        //    htmlContent += "<tr><th>Process Id</th><th>Workflow Id</th><th>Requster</th><th>Request Date</th><th>Status</th><th>Current Step</th></tr>";
        //    foreach (var process in allProcesses)
        //    {
        //        htmlContent += $"<tr><td>{process.Processid}</td><td>{process.Workflowid}</td><td>{process.RequesterUsername}</td><td>{process.Requestdate}</td><td>{process.Status}</td><td>{process.CurrentstepName}</td></tr>";
        //    }
        //    htmlContent += "</table>";

        //    // Generate PDF from HTML content
        //    var document = new PdfDocument();
        //    var config = new PdfGenerateConfig
        //    {
        //        PageOrientation = PageOrientation.Landscape,
        //        PageSize = PageSize.A4
        //    };

        //    // Menggunakan CSS untuk styling jika diperlukan
        //    string cssStr = File.ReadAllText(@"./Style/ReportStyle.css");
        //    CssData css = PdfGenerator.ParseStyleSheet(cssStr);

        //    PdfGenerator.AddPdfPages(document, htmlContent, config, css);

        //    using var stream = new MemoryStream();
        //    document.Save(stream, false);
        //    return stream.ToArray();
        //}

        //public async Task<int> GetTotalBook()
        //{
        //    return await _bookRepository.GetTotalBook();
        //}

        //public async Task<List<string>> GetMostActiveMembers()
        //{
        //    return await _borrowRepository.GetMostActiveMembers();
        //}

        //public async Task<IEnumerable<Borrow>> GetOverdueBooks()
        //{
        //    return await _borrowRepository.GetOverdueBooks();
        //}

        //public async Task<IEnumerable<NumberOfBooksPerCategory>> NumberOfBooksPerCategory()
        //{
        //    return await _bookRepository.NumberOfBooksPerCategory();
        //}

        //public async Task<int> NumberOfProcessAsync()
        //{
        //    return await _bookRequestRepository.NumberOfProcessAsync();
        //}

    }
}
