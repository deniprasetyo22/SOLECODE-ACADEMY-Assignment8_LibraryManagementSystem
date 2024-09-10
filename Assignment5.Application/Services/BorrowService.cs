using Assignment7.Application.Interfaces.IRepositories;
using Assignment7.Application.Interfaces.IService;
using Assignment7.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment7.Application.Services
{
    public class BorrowService : IBorrowService
    {
        private readonly IBorrowRepository _borrowRepository;
        public BorrowService(IBorrowRepository borrowRepository)
        {
            _borrowRepository = borrowRepository;
        }
        public async Task<(bool IsSuccess, string Message)> AddBorrow(Borrow borrow)
        {
            return await _borrowRepository.AddBorrow(borrow);
        }
        public async Task<IEnumerable<Borrow>> GetAllBorrows()
        {
            return await _borrowRepository.GetAllBorrows();
        }
        public async Task<Borrow> GetBorrowById(int borrowId)
        {
            return await _borrowRepository.GetBorrowById(borrowId);
        }
        public async Task<bool> ReturnBook(int borrowId)
        {
            return await _borrowRepository.ReturnBook(borrowId);
        }
        public async Task<(bool IsSuccess, string Message)> UpdateBorrow(int borrowId, Borrow updatedBorrow)
        {
            return await _borrowRepository.UpdateBorrow(borrowId, updatedBorrow);
        }
        public async Task<(bool IsSuccess, string Message)> DeleteBorrow(int borrowId)
        {
            return await _borrowRepository.DeleteBorrow(borrowId);
        }
    }
}
