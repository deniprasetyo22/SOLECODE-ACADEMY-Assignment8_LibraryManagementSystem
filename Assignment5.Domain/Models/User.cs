using Assignment7.Persistence.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment5.Domain.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int userId {  get; set; }

        [Required]
        [StringLength(255)]
        public string? firstName {  get; set; }

        [Required]
        [StringLength(255)]
        public string? lastName { get; set; }

        [Required]
        [StringLength(255)]
        [RegularExpression("Library Manager|Librarian|Library User", ErrorMessage = "Invalid position. Valid values are 'Library Manager', 'Librarian', 'Library User'.")]
        public string? position {  get; set; }

        [StringLength(255)]
        public string? privilage {  get; set; }

        [StringLength(255)]
        public string? libraryCardNumber { get; set; }

        [StringLength(255)]
        public string? notes { get; set; }

        public string? AppUserId { get; set; }

        [ForeignKey("AppUserId")]
        [InverseProperty("Users")]
        public virtual AspNetUser? AppUser { get; set; }
    }
}
