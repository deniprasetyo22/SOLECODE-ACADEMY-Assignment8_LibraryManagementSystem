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
    public class UserRepository:IUserRepository
    {
        private readonly LibraryContext _context;
        public UserRepository(LibraryContext context)
        {
            _context = context;
        }

        public async Task<User> AddUser(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }
        public async Task<IEnumerable<User>> GetAllUsers(paginationDto pagination)
        {
            var skipNumber = (pagination.pageNumber - 1) * pagination.pageSize;
            return await _context.Users.Skip(skipNumber).Take(pagination.pageSize).ToListAsync();
        }
        public async Task<User> GetUserById(int userId)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(cek => cek.userId == userId);
            if (existingUser == null)
            
            {
                return null;
            }

            return await _context.Users.FindAsync(userId);
        }
        public async Task<bool> UpdateUser(int userId, User user)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(cek => cek.userId == userId);
            
            if (existingUser == null)
            {
                return false;
            }

            existingUser.firstName = user.firstName;
            existingUser.lastName = user.lastName;
            existingUser.position = user.position;
            existingUser.privilage = user.privilage;
            existingUser.libraryCardNumber = user.libraryCardNumber;
            existingUser.notes = user.notes;
            
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteUser(int userId)
        {
            var deleteUser = await _context.Users.FirstOrDefaultAsync(cek => cek.userId == userId);
            if (deleteUser == null)
            
            {
                return false;
            }

            _context.Users.Remove(deleteUser);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
