using Assignment7.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment7.Application.Interfaces.IService
{
    public interface IBorrowService
    {
        Task<(bool IsSuccess, string Message)> AddBorrow(Borrow borrow);
        Task<IEnumerable<Borrow>> GetAllBorrows();
        Task<Borrow> GetBorrowById(int borrowId);
        Task<bool> ReturnBook(int borrowId);
        Task<(bool IsSuccess, string Message)> UpdateBorrow(int borrowId, Borrow updatedBorrow);
        Task<(bool IsSuccess, string Message)> DeleteBorrow(int borrowId);
    }
}
