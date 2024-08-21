using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment5.Application.DTOs
{
    public class SearchDto
    {
        public string? ISBN { get; set; }
        public string? category { get; set; }
        public string? title {  get; set; }
        public string? author { get; set; }
        public string? logicOperator { get; set; }
        public string? language {  get; set; }
    }
}
