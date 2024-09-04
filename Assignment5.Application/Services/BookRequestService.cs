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
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AppUser> _userManager;

        public BookRequestService(IBookRequestRepository bookRequestRepository, IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager)
        {
            _bookRequestRepository = bookRequestRepository;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }
        public async Task<Bookrequest> AddBookRequestAsync(Bookrequest bookRequest)
        {
            // Get current user ID from HttpContext
            var userName = _httpContextAccessor.HttpContext!.User.Identity!.Name;

            var user = await _userManager.FindByNameAsync(userName!);

            var userId = user!.Id;

            bookRequest.Process.Requesterid = userId;

            return await _bookRequestRepository.AddBookRequestAsync(bookRequest);
        }

        public async Task<IEnumerable<Bookrequest>> GetAllBookRequestAsync()
        {
            return await _bookRequestRepository.GetAllBookRequestAsync();
        }
    }
}
