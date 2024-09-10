﻿using Assignment5.Application.DTOs;
using Assignment5.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment5.Application.Interfaces.IService
{
    public interface IUserService
    {
        Task<User> AddUser(User user);
        Task<IEnumerable<User>> GetAllUsers(paginationDto pagination);
        Task<User> GetUserById(int userId);
        Task<bool> UpdateUser(int userId, User user);
        Task<bool> DeleteUser(int userId);
        Task<byte[]> GenerateReportPdf();
    }
}
