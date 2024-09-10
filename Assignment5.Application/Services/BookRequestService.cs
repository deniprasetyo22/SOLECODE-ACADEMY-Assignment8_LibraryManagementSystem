using Assignment5.Domain.Models;
using Assignment7.Application.Interfaces.IRepositories;
using Assignment7.Application.Interfaces.IService;
using Assignment7.Persistence.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment7.Application.Services
{
    public class BookRequestService:IBookRequestService
    {
        private readonly IBookRequestRepository _bookRequestRepository;

        public BookRequestService(IBookRequestRepository bookRequestRepository)
        {
            _bookRequestRepository = bookRequestRepository;
        }
        public async Task<Bookrequest> AddBookRequestAsync(Bookrequest bookRequest)
        {
            return await _bookRequestRepository.AddBookRequestAsync(bookRequest);
        }

        public async Task ApproveOrRejectBookRequestAsync(int processId, Process process)
        {
            await _bookRequestRepository.ApproveOrRejectBookRequestAsync(processId, process);
        }
    }
}
