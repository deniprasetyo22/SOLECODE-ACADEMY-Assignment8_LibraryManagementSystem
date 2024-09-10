using Assignment5.Application.DTOs;
using Assignment5.Application.Interfaces.IRepositories;
using Assignment5.Application.Interfaces.IService;
using Assignment5.Domain.Models;
using Assignment7.Application.DTOs;
using Microsoft.EntityFrameworkCore;
using PdfSharpCore;
using PdfSharpCore.Pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheArtOfDev.HtmlRenderer.Core;
using TheArtOfDev.HtmlRenderer.PdfSharp;

namespace Assignment5.Application.Services
{
    public class BookService:IBookService
    {
        private readonly IBookRepository _bookRepository;
        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository; 
        }

        public async Task<Book> AddBook(Book book)
        {
            return await _bookRepository.AddBook(book);
        }

        public async Task<IEnumerable<BookDto>> GetAllBooks(paginationDto pagination)
        {
            return await _bookRepository.GetAllBooks(pagination);
        }

        public async Task<BookDto> GetBookById(int bookId)
        {
            return await _bookRepository.GetBookById(bookId);
        }

        public async Task<bool> UpdateBook(int bookId, Book book)
        {
            return await _bookRepository.UpdateBook(bookId, book);
        }

        public async Task<bool> DeleteBook(int bookId, string reason)
        {
            return await _bookRepository.DeleteBook(bookId, reason);
        }

        public async Task<IEnumerable<BookDto>> Search(SearchDto query, paginationDto pagination)
        {
            return await _bookRepository.Search(query, pagination);
        }

        public async Task<byte[]> GenerateReportPdf()
        {
            var bookList = await _bookRepository.GetBookReportDtos();

            var categoryTotals = bookList
                .GroupBy(b => b.category)
                .Select(g => new
                {
                    Category = g.Key,
                    TotalPrice = g.Sum(b => b.price.HasValue ? b.price.Value : 0)
                })
                .ToList();

            string htmlContent = string.Empty;

            htmlContent += "<table>";
            htmlContent += "<tr> <td>Book Id</td> <td>Title</td> <td>Category</td> <td>Author</td> <td>Publisher</td> <td>Price</td> <td>Book Total</td></tr>";

            bookList.ToList().ForEach(item => {
                htmlContent += "<tr style='border:1px solid #ccc;>";
                htmlContent += "<td>" + item.Id + "</td>";
                htmlContent += "<td>" + item.title + "</td>";
                htmlContent += "<td>" + item.category + "</td>";
                htmlContent += "<td>" + item.author + "</td>";
                htmlContent += "<td>" + item.publisher + "</td>";
                htmlContent += "<td>" + "Rp." + (item.price.HasValue ? item.price.Value.ToString("F2") : "N/A") + "</td>";
                htmlContent += "<td>" + item.totalBook + "</td>";
                htmlContent += "</tr>";
            });

            htmlContent += "<tr><td colspan='7'</td></tr>";
            htmlContent += "<tr><td colspan='7' style='text-align: Left;'>Total Price per Category:</td><td></td></tr>";
            foreach (var categoryTotal in categoryTotals)
            {
                htmlContent += "<tr style='border:1px solid #ccc;'>";
                htmlContent += "<td style='text-align: left;'>Total for " + categoryTotal.Category + ":</td>";
                htmlContent += "<td colspan='6'>" + "Rp." + categoryTotal.TotalPrice.ToString("F2") + "</td>";
                htmlContent += "</tr>";
            }

            htmlContent += "</table>";

            var document = new PdfDocument();

            var config = new PdfGenerateConfig();
            config.PageOrientation = PageOrientation.Landscape;
            config.PageSize = PageSize.A4;

            string cssStr = File.ReadAllText(@"./Style/ReportStyle.css");
            CssData css = PdfGenerator.ParseStyleSheet(cssStr);

            PdfGenerator.AddPdfPages(document, htmlContent, config, css);

            MemoryStream stream = new MemoryStream();
            document.Save(stream, false);
            byte[] bytes = stream.ToArray();

            return bytes;
        }
    }
}
