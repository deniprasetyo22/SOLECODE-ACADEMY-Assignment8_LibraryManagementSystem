using Assignment7.Application.Interfaces.IService;
using Assignment7.Persistence.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Assignment7.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowController : ControllerBase
    {
        private readonly IBorrowService _borrowService;
        public BorrowController(IBorrowService borrowService)
        {
            _borrowService = borrowService;
        }

        /// <summary>
        /// Adds a new borrow record to the system.
        /// </summary>
        /// <remarks>
        /// Ensure that the borrow data is not null and that all required fields are provided.
        ///
        /// Sample request:
        ///
        ///     POST /api/Borrow
        ///     {
        ///        "UserId": 1,
        ///        "BookId": 1,
        ///        "BorrowDate": "2024-08-16"
        ///     }
        /// </remarks>
        /// <param name="borrow">The borrow record to be added.</param>
        /// <returns>Success message with borrow details if the record is added successfully, or an error message if validation fails.</returns>
        [HttpPost]
        public async Task<ActionResult> AddBorrow([FromBody] Borrow borrow)
        {
            if (borrow == null)
            {
                return BadRequest("Invalid input data. Please check the borrow data.");
            }

            var (isSuccess, message) = await _borrowService.AddBorrow(borrow);

            if (isSuccess)
            {
                return Ok(new
                {
                    Message = message,
                    BorrowDetails = borrow
                });
            }

            return BadRequest(message);
        }

        /// <summary>
        /// Retrieves a list of all borrow records in the system.
        /// </summary>
        /// <remarks>
        /// This endpoint retrieves all borrow records available in the system.
        ///
        /// Sample request:
        ///
        ///     GET /api/Borrow
        /// </remarks>
        /// <returns>A list of borrow records.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Borrow>>> GetAllBorrows()
        {
            var borrows = await _borrowService.GetAllBorrows();
            return Ok(borrows);
        }

        /// <summary>
        /// Retrieves a borrow record by its ID.
        /// </summary>
        /// <remarks>
        /// Ensure that the provided borrow ID is valid (greater than zero).
        ///
        /// Sample request:
        ///
        ///     GET /api/Borrow/5
        /// </remarks>
        /// <param name="borrowId">The ID of the borrow record to be retrieved.</param>
        /// <returns>Borrow details if found, otherwise an error message.</returns>
        [HttpGet("{borrowId}")]
        public async Task<ActionResult<Borrow>> GetBorrowById(int borrowId)
        {
            if (borrowId <= 0)
            {
                return BadRequest("Invalid borrow ID. The ID must be greater than zero.");
            }

            var borrow = await _borrowService.GetBorrowById(borrowId);
            if (borrow == null)
            {
                return NotFound($"Borrow record with ID {borrowId} not found.");
            }

            return Ok(borrow);
        }

        /// <summary>
        /// Returns a borrowed book.
        /// </summary>
        /// <remarks>
        /// Ensure that the provided borrow ID is valid (greater than zero).
        ///
        /// Sample request:
        ///
        ///     PUT /api/Borrow/return/5
        /// </remarks>
        /// <param name="borrowId">The ID of the borrow record to be updated.</param>
        /// <returns>Success message if the book is returned successfully, or an error message if the borrow record is not found.</returns>
        [HttpPut("return/{borrowId}")]
        public async Task<ActionResult> ReturnBook(int borrowId)
        {
            if (borrowId <= 0)
            {
                return BadRequest("Invalid borrow ID.");
            }

            bool result = await _borrowService.ReturnBook(borrowId);
            if (result)
            {
                return Ok("Book returned successfully.");
            }
            else
            {
                return NotFound("Borrow record not found.");
            }
        }

        /// <summary>
        /// Updates an existing borrow record.
        /// </summary>
        /// <remarks>
        /// Ensure that the borrow ID is valid and that the borrow data is not null.
        ///
        /// Sample request:
        ///
        ///     PUT /api/Borrow/5
        ///     {
        ///        "BookId": 1,
        ///        "UserId": 1,
        ///        "BorrowDate": "2024-08-16"
        ///     }
        /// </remarks>
        /// <param name="borrowId">The ID of the borrow record to be updated.</param>
        /// <param name="updatedBorrow">The updated borrow record data.</param>
        /// <returns>Success message with updated borrow details if the record is updated successfully, or an error message if validation fails.</returns>
        [HttpPut("{borrowId}")]
        public async Task<ActionResult> UpdateBorrow(int borrowId, [FromBody] Borrow updatedBorrow)
        {
            if (borrowId <= 0)
            {
                return BadRequest("Invalid borrow ID.");
            }

            if (updatedBorrow == null)
            {
                return BadRequest("Invalid borrow data.");
            }

            var (isSuccess, message) = await _borrowService.UpdateBorrow(borrowId, updatedBorrow);

            if (isSuccess)
            {
                return Ok(new
                {
                    Message = message,
                    BorrowDetails = updatedBorrow
                });
            }

            return BadRequest(message);
        }

        /// <summary>
        /// Deletes a borrow record by its ID.
        /// </summary>
        /// <remarks>
        /// Ensure that the provided borrow ID is valid.
        ///
        /// Sample request:
        ///
        ///     DELETE /api/Borrow/5
        /// </remarks>
        /// <param name="borrowId">The ID of the borrow record to be deleted.</param>
        /// <returns>Success message if the borrow record is deleted successfully, or an error message if the record is not found.</returns>
        [HttpDelete("{borrowId}")]
        public async Task<ActionResult> DeleteBorrow(int borrowId)
        {
            if (borrowId <= 0)
            {
                return BadRequest("Invalid borrow ID.");
            }

            var (isSuccess, message) = await _borrowService.DeleteBorrow(borrowId);

            if (isSuccess)
            {
                return Ok(message);
            }

            return NotFound(message);
        }
    }
}
