using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment5.Application.DTOs
{
    public class SearchDto
    {
        public string? ISBN { get; set; } = null;
        public string? category { get; set; } = null;
        public string? title {  get; set; } = null;
        public string? author { get; set; } = null;
        public string? logicOperator { get; set; }
    }
}
