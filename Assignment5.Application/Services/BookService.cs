using Assignment5.Application.DTOs;
using Assignment5.Application.Interfaces.IRepositories;
using Assignment5.Application.Interfaces.IService;
using Assignment5.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
