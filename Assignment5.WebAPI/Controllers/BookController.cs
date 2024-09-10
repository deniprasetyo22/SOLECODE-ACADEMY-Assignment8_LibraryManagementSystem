﻿using Asp.Versioning;
using Assignment5.Application.DTOs;
using Assignment5.Application.Interfaces.IService;
using Assignment5.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Assignment5.WebAPI.Controllers
{
    [Authorize]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[Controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        /// <summary>
        /// Adds a new book to the system.
        /// </summary>
        /// <remarks>
        /// Ensure that the book data is not null and that the book details are valid.
        /// Validate that no book with the same ISBN or Title already exists.
        ///
        /// Sample request:
        ///
        ///     POST /api/Book
        ///     {
        ///         "catergory":"Education",
        ///         "title":"Biology",
        ///         "ISBN":"AAA11111",
        ///         "publisher":"Gramedia",
        ///         "description":"All about biology",
        ///         "location":"A1",
        ///         "price":30000,
        ///         "totalBook":5
        ///     }
        /// </remarks>
        /// <param name="book">The book to be added.</param>
        /// <returns>
        /// If the book is successfully added, returns a 200 OK response with a success message and the added book details.
        /// If a book with the same ISBN or Title already exists, returns a 400 Bad Request response with an error message.
        /// If the input data is invalid, returns a 400 Bad Request response with an error message.
        /// </returns>
        [Authorize(Roles = "Library Manager")]
        [HttpPost]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> AddBook([FromBody] Book book)
        {
            if (book == null)
            {
                return BadRequest("Invalid input data. Please check the book details.");
            }

            var addedBook = await _bookService.AddBook(book);

            if(addedBook == null)
            {
                return BadRequest("A book with the same ISBN or Title already exists.");
            }

            return Ok(new { Message = "Book added successfully.", Book = addedBook });
        }


        /// <summary>
        /// Retrieves a list of all books in the system.
        /// </summary>
        /// <remarks>
        /// This endpoint retrieves all books available in the system.
        ///
        /// Sample request:
        ///
        ///     GET /api/Book
        /// </remarks>
        /// <returns>A list of books.</returns>
        [Authorize(Roles = "Library Manager, Librarian, Library User")]
        [HttpGet]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<IEnumerable<Book>>> GetAllBooks([FromQuery] paginationDto pagination)
        {
            var books = await _bookService.GetAllBooks(pagination); 
            return Ok(books);
        }

        /// <summary>
        /// Retrieves a book by its ID.
        /// </summary>
        /// <remarks>
        /// Ensure that the provided book ID is valid (greater than zero).
        ///
        /// Sample request:
        ///
        ///     GET /api/Book/5
        /// </remarks>
        /// <param name="bookId">The ID of the book to be retrieved.</param>
        /// <returns>Book details if found, otherwise an error message.</returns>
        [Authorize(Roles = "Library Manager, Librarian, Library User")]
        [HttpGet("{bookId}")]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<Book>> GetBookById(int bookId)
        {
            if (bookId <= 0)
            {
                return BadRequest("Invalid ID. The ID must be greater than zero.");
            }

            var book = await _bookService.GetBookById(bookId);
            if (book == null)
            {
                return NotFound($"Book with ID {bookId} was not found.");
            }

            return Ok(book);
        }

        /// <summary>
        /// Updates an existing book by its ID.
        /// </summary>
        /// <remarks>
        /// Ensure that the book data is not null and that all required fields are provided.
        /// Validate that the book's name and ISBN are unique.
        ///
        /// Sample request:
        ///
        ///     PUT /api/Book/5
        ///     {
        ///        "Title": "Updated Book Title",
        ///        "Author": "Updated Author",
        ///        "PublicationYear": 1925
        ///        "ISBN": "9780743273565",
        ///     }
        /// </remarks>
        /// <param name="bookId">The ID of the book to be updated.</param>
        /// <param name="book">The updated book details.</param>
        /// <returns>Success message if the book is updated successfully or an error message if validation fails.</returns>
        [Authorize(Roles = "Library Manager")]
        [HttpPut("{bookId}")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UpdateBook(int bookId, [FromBody] Book book)
        {
            if (book == null)
            {
                return BadRequest("Invalid input data. Please check the book details.");
            }

            var success = await _bookService.UpdateBook(bookId, book);
            if (!success)
            {
                return BadRequest("Unable to update book. Title or ISBN might already exist.");
            }

            return Ok("Book updated successfully.");
        }

        /// <summary>
        /// Deletes a book by its ID.
        /// </summary>
        /// <remarks>
        /// Ensure that the book ID provided is valid.
        ///
        /// Sample request:
        ///
        ///     DELETE /api/Book/5
        /// </remarks>
        /// <param name="bookId">The ID of the book to be deleted.</param>
        /// <returns>Success message if the book is deleted successfully or an error message if the book is not found.</returns>
        [Authorize(Roles = "Library Manager")]
        [HttpDelete("{bookId}")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> DeleteBook(int bookId,[FromBody] string reason)
        {
            // Memeriksa apakah reason adalah null atau kosong
            if (string.IsNullOrEmpty(reason))
            {
                return BadRequest("The reason should not be empty.");
            }

            var success = await _bookService.DeleteBook(bookId, reason);
            if (!success)
            {
                return NotFound("Book not found.");
            }

            return Ok("Book deleted successfully.");
        }

        /// <summary>
        /// Searches for books based on the provided search criteria and pagination.
        /// </summary>
        /// <remarks>
        /// This endpoint allows you to search for books by various criteria and paginate the results. Ensure the query parameters are provided correctly.
        ///
        /// Sample request:
        /// 
        ///     GET /api/Book/search
        /// </remarks>
        /// <param name="query">Search criteria including title, author, ISBN, and category. These parameters are optional.</param>
        /// <param name="pagination">Pagination details including page number and page size.</param>
        /// <returns>A list of books that match the search criteria, along with pagination details.</returns>
        [Authorize(Roles = "Library Manager, Librarian, Library User")]
        [HttpGet("search")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Search([FromQuery] SearchDto query, [FromQuery] paginationDto pagination)
        {
            var search = await _bookService.Search(query, pagination);

            var searchDto = search.Select(a => a.ToString()).ToList();

            return Ok(search);
        }

        [Authorize]
        [HttpGet("report")]
        public async Task<IActionResult> Report()
        {
            var Filename = "BookReport.pdf";
            var file = await _bookService.GenerateReportPdf();
            return File(file, "application/pdf", Filename);
        }

    }
}
