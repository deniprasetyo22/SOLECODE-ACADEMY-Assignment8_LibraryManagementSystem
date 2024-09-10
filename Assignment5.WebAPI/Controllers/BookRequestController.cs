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

        [Authorize(Roles = "Library User")]
        [HttpPost]
        public async Task<IActionResult> CreateBookRequest([FromBody] Bookrequest bookRequest)
        {
            if (bookRequest == null)
            {
                return BadRequest("Invalid request");
            }

            var addBookRequest = await _bookRequestService.AddBookRequestAsync(bookRequest);

            return Ok(addBookRequest);
        }

        [Authorize(Roles = "Librarian, Library Manager")]
        [HttpPut("approveOrReject/{processId}")]
        public async Task<IActionResult> approveOrRejectBookRequestAsync(int processId, [FromBody] Process process)
        {
            await _bookRequestService.ApproveOrRejectBookRequestAsync(processId, process);
            return Ok();
        }

    }
}
