using Assignment5.Application.DTOs;
using Assignment5.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment5.Application.Interfaces.IService
{
    public interface IBookService
    {
        Task<Book> AddBook(Book book);
        Task<IEnumerable<BookDto>> GetAllBooks(paginationDto pagination);
        Task<BookDto> GetBookById(int bookId);
        Task<bool> UpdateBook(int bookId, Book book);
        Task<bool> DeleteBook(int bookId, string reason);
        Task<IEnumerable<BookDto>> Search(SearchDto query, paginationDto pagination);
    }
}
