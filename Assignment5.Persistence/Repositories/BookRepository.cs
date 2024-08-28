using Assignment5.Application.DTOs;
using Assignment5.Application.Interfaces.IRepositories;
using Assignment5.Domain.Models;
using Assignment5.Persistence.Context;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment5.Persistence.Repositories
{
    public class BookRepository:IBookRepository
    {
        private readonly LibraryContext _context;

        public BookRepository(LibraryContext context)
        {
            _context = context;
        }

        public async Task<Book> AddBook(Book book)
        {
            var existingBook = await _context.Books.FirstOrDefaultAsync(cek => cek.ISBN == book.ISBN || cek.title == book.title);
            
            if (existingBook != null)
            {
                return null;
            }

            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task<IEnumerable<BookDto>> GetAllBooks(paginationDto pagination)
        {
            var books = _context.Books
                .Where(cek => !cek.status.Contains("Deleted"));

            var skipNumber = (pagination.pageNumber - 1) * pagination.pageSize;

            return await books
                .Skip(skipNumber)
                .Take(pagination.pageSize)
                .Select(b => new BookDto
                {
                    Id = b.bookId,
                    category = b.category,
                    title = b.title,
                    ISBN = b.ISBN,
                    author = b.author,
                    publisher = b.publisher,
                    description = b.description,
                    location = b.location,
                    totalBook = b.totalBook,
                    language = b.language
                })
                .OrderBy(b => b.title)
                .ToListAsync();
        }

        public async Task<BookDto> GetBookById(int bookId)
        {
            var existingBook = await _context.Books
                .Where(cek => cek.bookId == bookId)
                .Select(b => new BookDto
                {
                    Id = b.bookId,
                    category = b.category,
                    title = b.title,
                    ISBN = b.ISBN,
                    author = b.author,
                    publisher = b.publisher,
                    description = b.description,
                    location = b.location,
                    totalBook = b.totalBook,
                    language = b.language
                })
                .FirstOrDefaultAsync();

            if (existingBook == null)
            {
                return null;
            }

            return existingBook;
        }

        public async Task<bool> UpdateBook(int bookId, Book book)
        {
            var existingBook = await _context.Books.FindAsync(bookId);
            
            if (existingBook == null)
            {
                return false;
            }

            var duplicateISBN = await _context.Books.AnyAsync(b => b.ISBN == book.ISBN && b.bookId != bookId);
            if (duplicateISBN)
            
            {
                return false;
            }

            var duplicateTitle = await _context.Books.AnyAsync(b => b.title == book.title && b.bookId != bookId);
            if (duplicateTitle)
            
            {
                return false;
            }

            existingBook.category = book.category;
            existingBook.title = book.title;
            existingBook.ISBN = book.ISBN;
            existingBook.publisher = book.publisher;
            existingBook.author = book.author;
            existingBook.description = book.description;
            existingBook.location = book.location;
            existingBook.price = book.price;
            existingBook.totalBook = book.totalBook;
            existingBook.language = book.language;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteBook(int bookId, string reason)
        {
            // Memeriksa apakah reason adalah null atau kosong
            if (string.IsNullOrEmpty(reason))
            {
                return false;
            }

            var deleteBook = await _context.Books.FindAsync(bookId);
            if (deleteBook == null)
            {
                return false;
            }

            deleteBook.status = "Deleted at " + DateTime.UtcNow;
            deleteBook.reason = reason;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<BookDto>> Search(SearchDto query, paginationDto pagination)
        {
            var search = _context.Books.AsQueryable();

            // Logika pencarian berdasarkan OR
            if (query.logicOperator.Equals("OR", StringComparison.OrdinalIgnoreCase))
            {
                search = search.Where(b =>
                    (!string.IsNullOrEmpty(query.title) && b.title.ToLower().Contains(query.title.ToLower())) ||
                    (!string.IsNullOrEmpty(query.author) && b.author.ToLower().Contains(query.author.ToLower())) ||
                    (!string.IsNullOrEmpty(query.ISBN) && b.ISBN.ToLower().Contains(query.ISBN.ToLower())) ||
                    (!string.IsNullOrEmpty(query.category) && b.category.ToLower().Contains(query.category.ToLower())) ||
                    (!string.IsNullOrEmpty(query.language) && b.language.ToLower().Contains(query.language.ToLower()))
                );
            }   
            else // Default to AND logic
            {
                if (!string.IsNullOrEmpty(query.title))
                    search = search.Where(b => b.title.ToLower().Contains(query.title.ToLower()));

                if (!string.IsNullOrEmpty(query.author))
                    search = search.Where(b => b.author.ToLower().Contains(query.author.ToLower()));

                if (!string.IsNullOrEmpty(query.ISBN))
                    search = search.Where(b => b.ISBN.ToLower().Contains(query.ISBN.ToLower()));

                if (!string.IsNullOrEmpty(query.category))
                    search = search.Where(b => b.category.ToLower().Contains(query.category.ToLower()));
                
                if (!string.IsNullOrEmpty(query.language))
                    search = search.Where(b => b.language.ToLower().Contains(query.language.ToLower()));
            }

            var skipNumber = (pagination.pageNumber - 1) * pagination.pageSize;

            return await search
                .Skip(skipNumber)
                .Take(pagination.pageSize)
                .Select(b => new BookDto
                {
                    Id = b.bookId,
                    category = b.category,
                    title = b.title,
                    ISBN = b.ISBN,
                    author = b.author,
                    publisher = b.publisher,
                    description = b.description,
                    location = b.location,
                    totalBook = b.totalBook,
                    language = b.language
                })
                .OrderBy(b => b.title)
                .ToListAsync();
        }


    }
}
