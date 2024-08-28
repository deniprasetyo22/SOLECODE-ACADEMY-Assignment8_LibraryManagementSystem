using Asp.Versioning;
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
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Adds a new user to the system.
        /// </summary>
        /// <remarks>
        /// Ensure that the user data is not null and that all required fields are provided.
        ///
        /// Sample request:
        ///
        ///     POST /api/User
        ///     {
        ///        "firstName": "Deni",
        ///        "lastName": "Prasetyo",
        ///        "position": "Library Manager",
        ///        "privilage": "Can Add User",
        ///        "libraryCardNumber": "L-11111",
        ///        "notes":"admin"
        ///     }
        /// </remarks>
        /// <param name="user">The user to be added.</param>
        /// <returns>Success message if the user is added successfully, or an error message if validation fails.</returns>
        [Authorize(Roles = "Library Manager")]
        [HttpPost]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<User>> AddUser([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest("Invalid input data. Please check the user data.");
            }

            await _userService.AddUser(user);
            return Ok("User added successfully.");
        }

        /// <summary>
        /// Retrieves a list of all users in the system.
        /// </summary>
        /// <remarks>
        /// This endpoint retrieves all user records available in the system.
        ///
        /// Sample request:
        ///
        ///     GET /api/User
        /// </remarks>
        /// <returns>A list of users.</returns>
        [Authorize(Roles = "Library Manager")]
        [HttpGet]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers([FromQuery] paginationDto pagination)
        {
            var users = await _userService.GetAllUsers(pagination);
            return Ok(users);
        }

        /// <summary>
        /// Retrieves a user by its ID.
        /// </summary>
        /// <remarks>
        /// Ensure that the provided user ID is valid (greater than zero).
        ///
        /// Sample request:
        ///
        ///     GET /api/User/1
        /// </remarks>
        /// <param name="userId">The ID of the user to be retrieved.</param>
        /// <returns>User details if found, otherwise an error message.</returns>
        [Authorize(Roles = "Library Manager")]
        [HttpGet("{userId}")]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<User>> GetUserById(int userId)
        {
            if (userId <= 0)
            {
                return BadRequest("Invalid ID. The ID must be greater than zero.");
            }
            var user = await _userService.GetUserById(userId);
            if (user == null)
            {
                return NotFound($"User with ID {userId} not found.");
            }
            return Ok(user);
        }

        /// <summary>
        /// Updates an existing user record.
        /// </summary>
        /// <remarks>
        /// Ensure that the user ID is valid and that the user data is not null.
        ///
        /// Sample request:
        ///
        ///     PUT /api/User/1
        ///     {
        ///        "firstName": "Deni",
        ///        "lastName": "Prasetyo",
        ///        "position": "Library Manager",
        ///        "privilage": "Can Add User",
        ///        "libraryCardNumber": "L-11111",
        ///        "notes":"admin"
        ///     }
        /// </remarks>
        /// <param name="userId">The ID of the user to be updated.</param>
        /// <param name="user">The updated user data.</param>
        /// <returns>Success message if the user is updated successfully, or an error message if validation fails.</returns>
        [Authorize(Roles = "Library Manager")]
        [HttpPut("{userId}")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UpdateUser(int userId, [FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest("Invalid input data. Please check the user data.");
            }
            var success = await _userService.UpdateUser(userId, user);
            if (!success)
            {
                return NotFound("User not found.");
            }
            return Ok("User updated successfully.");
        }

        /// <summary>
        /// Deletes a user by its ID.
        /// </summary>
        /// <remarks>
        /// Ensure that the provided user ID is valid.
        ///
        /// Sample request:
        ///
        ///     DELETE /api/User/1
        /// </remarks>
        /// <param name="userId">The ID of the user to be deleted.</param>
        /// <returns>Success message if the user is deleted successfully, or an error message if the user is not found.</returns>
        [Authorize(Roles = "Library Manager")]
        [HttpDelete("{userId}")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var success = await _userService.DeleteUser(userId);
            if (!success)
            {
                return NotFound("User not found.");
            }

            return Ok("User deleted successfully.");
        }
    }
}
