using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment7.Application.DTOs
{
    public class OverdueDto
    {
        public int Borrowid { get; set; }
        public string? Userid { get; set; }
        public int Bookid { get; set; }
        public DateOnly? Deadlinereturn { get; set; }
        public DateOnly? Dateofreturn { get; set; }
    }
}
