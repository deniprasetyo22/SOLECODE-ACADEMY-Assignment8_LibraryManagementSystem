using Assignment7.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment7.Application.Interfaces.IService
{
    public interface IBookRequestService
    {
        Task<Bookrequest> AddBookRequestAsync(Bookrequest bookRequest);
        Task ApproveOrRejectBookRequestAsync(int processId, Process process);
    }
}
