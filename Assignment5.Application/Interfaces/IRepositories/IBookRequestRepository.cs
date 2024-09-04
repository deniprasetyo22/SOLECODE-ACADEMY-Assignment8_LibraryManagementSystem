﻿using Assignment7.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment7.Application.Interfaces.IRepositories
{
    public interface IBookRequestRepository
    {
        Task<Bookrequest> AddBookRequestAsync(Bookrequest bookRequest);
        Task<IEnumerable<Bookrequest>> GetAllBookRequestAsync();
    }
}
