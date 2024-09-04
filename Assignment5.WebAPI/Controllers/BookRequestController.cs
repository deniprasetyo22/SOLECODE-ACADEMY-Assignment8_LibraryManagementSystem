using Assignment5.Domain.Models;
using Assignment7.Application.Interfaces.IService;
using Assignment7.Persistence.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Assignment7.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookRequestController : ControllerBase
    {
        private readonly IBookRequestService _bookRequestService;
        public BookRequestController(IBookRequestService bookRequestService)
        {
            _bookRequestService = bookRequestService;
        }

        [Authorize(Roles = "Library Manager")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Bookrequest bookRequest)
        {
            if (bookRequest == null)
            {
                return BadRequest("Invalid request");
            }

            var addBookRequest = await _bookRequestService.AddBookRequestAsync(bookRequest);

            return Ok(addBookRequest);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBookRequest()
        {
            var requests = await _bookRequestService.GetAllBookRequestAsync();

            return Ok(requests);
        }
    }
}
