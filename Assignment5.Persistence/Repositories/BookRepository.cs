using Assignment5.Application.DTOs;
using Assignment5.Application.Interfaces.IRepositories;
using Assignment5.Domain.Models;
using Assignment5.Persistence.Context;
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

        public async Task<IEnumerable<Book>> GetAllBooks()
        {
            return await _context.Books.ToListAsync();
        }

        public async Task<Book?> GetBookById(int bookId)
        {
            var existingBook = await _context.Books.FirstOrDefaultAsync(cek => cek.bookId == bookId);
            
            if (existingBook == null)
            {
                return null;
            }

            return await _context.Books.FindAsync(bookId);
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

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteBook(int bookId, string reason)
        {
            var deleteBook = await _context.Books.FindAsync(bookId);
            if (deleteBook == null)
            {
                return false;
            }

            deleteBook.status = "Deleted at" + DateTime.UtcNow;
            deleteBook.reason = reason;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Book>> Search(SearchDto query, paginationDto pagination)
        {
            var search = _context.Books.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.ISBN))
            {
                search = search.Where(b => b.ISBN.Contains(query.ISBN));
            }

            if (!string.IsNullOrWhiteSpace(query.category))
            {
                search = search.Where(b => b.category.Contains(query.category));
            }

            if (!string.IsNullOrWhiteSpace(query.title))
            {
                search = search.Where(b => b.title.Contains(query.title));
            }

            if (!string.IsNullOrWhiteSpace(query.author))
            {
                search = search.Where(b => b.author.Contains(query.author));
            }
            
            if (!string.IsNullOrWhiteSpace(query.sortBy))
            {
                if(query.sortBy.Equals("title", StringComparison.OrdinalIgnoreCase))
                {
                    search = query.IsDescending ? search.OrderByDescending(o => o.title) : search.OrderBy(o => o.title);
                }
            }

            var skipNumber = (pagination.pageNumber - 1) * pagination.pageSize;

            return await search.Skip(skipNumber).Take(pagination.pageSize).ToListAsync();
        }

    }
}
