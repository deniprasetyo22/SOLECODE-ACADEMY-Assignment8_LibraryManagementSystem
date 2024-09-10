using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment7.Application.DTOs
{
    public class BookReportDto
    {
            public int Id { get; set; }
            public string? category { get; set; }
            public string? title { get; set; }
            public string? author { get; set; }
            public string? publisher { get; set; }
            public double? price { get; set; }
            public int? totalBook { get; set; }
    }
}
