using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment5.Application.DTOs
{
    public class paginationDto
    {
        public int pageNumber { get; set; } = 1;
        public int pageSize { get; set; } = 5;
    }
}
