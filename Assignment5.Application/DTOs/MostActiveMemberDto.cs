using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment7.Application.DTOs
{
    public class MostActiveMemberDto
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public int BorrowTotal { get; set; }
    }
}
