using Assignment5.Persistence.Context;
using Assignment7.Application.Interfaces.IRepositories;
using Assignment7.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment7.Persistence.Repositories
{
    public class BookRequestRepository:IBookRequestRepository
    {
        private readonly LibraryContext _context;
        public BookRequestRepository(LibraryContext context)
        {
            _context = context; 
        }

        public async Task<Bookrequest> AddBookRequestAsync(Bookrequest bookRequest)
        {
            _context.BookRequests.Add(bookRequest);
            await _context.SaveChangesAsync();
            return bookRequest;
        }

        public async Task<IEnumerable<Bookrequest>> GetAllBookRequestAsync()
        {
            return await _context.BookRequests.ToListAsync();
        }


    }
}
