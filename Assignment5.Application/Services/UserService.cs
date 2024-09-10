using Assignment5.Application.DTOs;
using Assignment5.Application.Interfaces.IRepositories;
using Assignment5.Application.Interfaces.IService;
using Assignment5.Domain.Models;
using PdfSharpCore.Pdf;
using PdfSharpCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheArtOfDev.HtmlRenderer.Core;
using TheArtOfDev.HtmlRenderer.PdfSharp;
using Assignment7.Application.Interfaces.IRepositories;

namespace Assignment5.Application.Services
{
    public class UserService:IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IBorrowRepository _borrowRepository;
        public UserService(IUserRepository userRepository, IBorrowRepository borrowRepository)
        {
            _userRepository = userRepository;
            _borrowRepository = borrowRepository;
        }

        public async Task<User> AddUser(User user)
        {
            return await _userRepository.AddUser(user);
        }

        public async Task<IEnumerable<User>> GetAllUsers(paginationDto pagination)
        {
            return await _userRepository.GetAllUsers(pagination);
        }

        public async Task<User> GetUserById(int userId)
        {
            return await _userRepository.GetUserById(userId);
        }

        public async Task<bool> UpdateUser(int userId, User user)
        {
            return await _userRepository.UpdateUser(userId, user);
        }

        public async Task<bool> DeleteUser(int userId)
        {
            return await _userRepository.DeleteUser(userId);
        }

        public async Task<byte[]> GenerateReportPdf()
        {
            var userList = await _borrowRepository.GetAllBorrows();

            var filteredList = userList
                .Where(b => b.Dateofreturn == null || !string.IsNullOrEmpty(b.Penalty))
                .ToList();

            var today = DateOnly.FromDateTime(DateTime.Today);

            string htmlContent = string.Empty;

            htmlContent += "<table>";
            htmlContent += "<tr> <td>Id</td> <td>User Name</td> <td>Book Id</td> <td>Date Of Borrow</td> <td>Dateline Return</td> <td>Date Of Return</td> <td>Penalty</td> </tr>";

            filteredList.ToList().ForEach(item => {
                htmlContent += "<tr style='border:1px solid #ccc;>";
                htmlContent += "<td>" + item.Userid + "</td>";
                htmlContent += "<td>" + item.User.UserName + "</td>";
                htmlContent += "<td>" + item.Bookid + "</td>";
                htmlContent += "<td>" + item.Dateofborrow + "</td>";
                htmlContent += "<td>" + item.Deadlinereturn + "</td>";
                htmlContent += "<td>" + item.Dateofreturn + "</td>";
                htmlContent += "<td>" + item.Penalty + "</td>";
                htmlContent += "</tr>";
            });
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
