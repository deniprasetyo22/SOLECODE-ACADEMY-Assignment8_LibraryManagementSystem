using Assignment5.Persistence.Context;
using Assignment7.Application.Interfaces.IRepositories;
using Assignment7.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment7.Persistence.Repositories
{
    public class BorrowRepository : IBorrowRepository
    {
        private readonly LibraryContext _context;
        private const int MaxBookBorrowed = 5; // Ganti dengan nilai sesuai kebutuhan
        private const int DurationBookLoans = 14; // Ganti dengan nilai sesuai kebutuhan

        public BorrowRepository(LibraryContext context)
        {
            _context = context;
        }

        // Adds a new borrow record
        public async Task<(bool IsSuccess, string Message)> AddBorrow(Borrow borrow)
        {
            // Check if the borrow object is null
            if (borrow == null)
            {
                return (false, "Invalid borrow data.");
            }

            // Check if the book is already borrowed and not returned
            var existingBorrowedBook = await _context.Borrows
                .Where(b => b.Bookid == borrow.Bookid && b.Dateofreturn == null)
                .FirstOrDefaultAsync();

            if (existingBorrowedBook != null)
            {
                return (false, "The book is already borrowed and not returned yet.");
            }

            // Validate the number of books currently borrowed by the user
            var borrowedBooksCount = await _context.Borrows
                .Where(b => b.Userid == borrow.Userid && b.Dateofreturn == null)
                .CountAsync();

            if (borrowedBooksCount >= MaxBookBorrowed)
            {
                return (false, $"User has already borrowed the maximum number of books ({MaxBookBorrowed}).");
            }

            // Ensure the DateOfBorrow is provided
            if (!borrow.Dateofborrow.HasValue)
            {
                return (false, "The borrow date must be provided.");
            }

            // Calculate and set the deadline for returning the book
            borrow.Deadlinereturn = borrow.Dateofborrow.Value.AddDays(DurationBookLoans);

            // Add the borrow record to the database
            await _context.Borrows.AddAsync(borrow);
            await _context.SaveChangesAsync();

            return (true, "Borrow added successfully.");
        }

        // Retrieves all borrow records
        public async Task<IEnumerable<Borrow>> GetAllBorrows()
        {
            return await _context.Borrows
                .Include(b => b.Book)
                .Include(b => b.User)
                .ToListAsync();
        }

        // Retrieves a borrow record by its ID
        public async Task<Borrow> GetBorrowById(int borrowId)
        {
            var borrow = await _context.Borrows
                .Include(b => b.Book)
                .Include(b => b.User)
                .FirstOrDefaultAsync(b => b.Borrowid == borrowId);

            return borrow;
        }

        // Marks a book as returned
        public async Task<bool> ReturnBook(int borrowId)
        {
            var existingBorrow = await _context.Borrows
                .FirstOrDefaultAsync(b => b.Borrowid == borrowId);

            if (existingBorrow == null)
            {
                return false;
            }

            existingBorrow.Dateofreturn = DateOnly.FromDateTime(DateTime.UtcNow);

            if (existingBorrow.Dateofreturn > existingBorrow.Deadlinereturn)
            {
                // Calculate the number of days late
                var daysLate = existingBorrow.Dateofreturn.Value.DayNumber - existingBorrow.Deadlinereturn.Value.DayNumber;

                // Set penalty as the number of days late, you can modify this calculation as needed
                existingBorrow.Penalty = daysLate.ToString() + "000"; // Example: Set penalty as a string of days late
            }
            else
            {
                existingBorrow.Penalty = null; // No penalty if returned on or before the deadline
            }

            await _context.SaveChangesAsync();
            return true;
        }

        // Updates a borrow record
        public async Task<(bool IsSuccess, string Message)> UpdateBorrow(int borrowId, Borrow updatedBorrow)
        {
            var existingBorrow = await _context.Borrows
                .FirstOrDefaultAsync(b => b.Borrowid == borrowId);

            if (existingBorrow == null)
            {
                return (false, "Borrow record not found.");
            }

            // Validate that the updated borrow record meets the requirements
            var borrowedBooksCount = await _context.Borrows
                .Where(b => b.Userid == updatedBorrow.Userid && b.Dateofreturn == null && b.Borrowid != borrowId)
                .CountAsync();

            if (borrowedBooksCount >= MaxBookBorrowed)
            {
                return (false, "User has already borrowed the maximum number of books.");
            }

            // Update borrow details
            existingBorrow.Userid = updatedBorrow.Userid;
            existingBorrow.Bookid = updatedBorrow.Bookid;
            existingBorrow.Dateofborrow = updatedBorrow.Dateofborrow;
            existingBorrow.Deadlinereturn = updatedBorrow.Dateofborrow?.AddDays(DurationBookLoans);
            existingBorrow.Dateofreturn = updatedBorrow.Dateofreturn;

            await _context.SaveChangesAsync();
            return (true, "Borrow record updated successfully.");
        }

        // Deletes a borrow record
        public async Task<(bool IsSuccess, string Message)> DeleteBorrow(int borrowId)
        {
            var existingBorrow = await _context.Borrows
                .FirstOrDefaultAsync(b => b.Borrowid == borrowId);

            if (existingBorrow == null)
            {
                return (false, "Borrow record not found.");
            }

            _context.Borrows.Remove(existingBorrow);
            await _context.SaveChangesAsync();
            return (true, "Borrow record deleted successfully.");
        }
    }

}
