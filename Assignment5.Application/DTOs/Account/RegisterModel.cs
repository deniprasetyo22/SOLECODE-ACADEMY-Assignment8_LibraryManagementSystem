using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment5.Application.DTOs.Account
{
    public class RegisterModel
    {
        [Required]
        public string? Username { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }

        [Required]
        public string? FirstName { get; set; }

        [Required]
        public string? LastName { get; set; }

        [Required]
        [RegularExpression("Library Manager|Librarian|Library User", ErrorMessage = "Invalid position. Valid values are 'Library Manager', 'Librarian', 'Library User'.")]
        public string? Position { get; set; }

        public string? Privilage { get; set; }
        public string? LibraryCardNumber { get; set; }
        public string? Notes { get; set; }
    }
}
