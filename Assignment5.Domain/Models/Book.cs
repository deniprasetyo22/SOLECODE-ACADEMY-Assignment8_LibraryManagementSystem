using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment5.Domain.Models
{
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int bookId { get; set; }

        [Required]
        [StringLength(255)]
        public string? category { get; set; }

        [Required]
        [StringLength(255)]
        public string? title {  get; set; }

        [Required]
        [StringLength(255)]
        public string? ISBN {  get; set; }

        [Required]
        [StringLength(255)]
        public string? author {  get; set; }

        [Required]
        [StringLength(255)]
        public string? publisher {  get; set; }

        [Required]
        [StringLength(255)]
        public string? description { get; set; }

        [Required]
        [StringLength(255)]
        public string? location {  get; set; }

        [Required]
        public DateTime? purchaseDate {  get; set; } = DateTime.UtcNow;

        [Required]
        public double? price {  get; set; }

        [Required]
        public int totalBook {  get; set; }

        [StringLength(255)]
        public string? status { get; set; }

        [StringLength(255)]
        public string? reason { get; set; }

        [StringLength(255)]
        public string? language {  get; set; }
    }
}
