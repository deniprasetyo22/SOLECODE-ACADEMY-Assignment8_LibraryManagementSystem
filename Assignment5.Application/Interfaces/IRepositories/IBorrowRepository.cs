﻿using Assignment7.Application.DTOs;
using Assignment7.Persistence.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment7.Application.Interfaces.IRepositories
{
    public interface IBorrowRepository
    {
        Task<(bool IsSuccess, string Message)> AddBorrow(Borrow borrow);
        Task<IEnumerable<Borrow>> GetAllBorrows();
        Task<Borrow> GetBorrowById(int borrowId);
        Task<bool> ReturnBook(int borrowId);
        Task<(bool IsSuccess, string Message)> UpdateBorrow(int borrowId, Borrow updatedBorrow);
        Task<(bool IsSuccess, string Message)> DeleteBorrow(int borrowId);
        Task<List<MostActiveMemberDto>> GetMostActiveMembers();
        Task<IEnumerable<OverdueDto>> GetOverdueBooks();
    }
}
