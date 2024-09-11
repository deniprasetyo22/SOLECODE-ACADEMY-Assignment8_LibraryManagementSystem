using Assignment7.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment7.Application.DTOs
{
    public class DashboardDto
    {
        public int TotalBooks { get; set; }
        public List<MostActiveMemberDto>? MostActiveMembers { get; set; }
        public IEnumerable<NumberOfBooksPerCategory> BooksPerCategory { get; set; }
        public int NumberOfProcesses { get; set; }
        public List<ProcessDto> AllProcesses { get; set; }
        public IEnumerable<OverdueDto> OverdueBooks { get; set; }
    }
}
