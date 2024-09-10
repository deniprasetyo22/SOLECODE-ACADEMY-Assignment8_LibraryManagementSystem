using Assignment5.Domain.Models;
using Assignment7.Persistence.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment5.Persistence.Context
{
    public partial class LibraryContext : IdentityDbContext<AppUser>
    {
        public LibraryContext(DbContextOptions<LibraryContext> options): base(options)
        {
        }


        public virtual DbSet<Book> Books { get; set; }

        public virtual DbSet<Bookrequest> Bookrequests { get; set; }

        public virtual DbSet<Nextsteprule> Nextsteprules { get; set; }

        public virtual DbSet<Process> Processes { get; set; }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<Workflow> Workflows { get; set; }

        public virtual DbSet<Workflowaction> Workflowactions { get; set; }

        public virtual DbSet<Workflowsequence> Workflowsequences { get; set; }
        public virtual DbSet<Borrow> Borrows { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder
            .UseNpgsql("Name=DefaultConnection");
    }
}
