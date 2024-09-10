using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Assignment5.Domain.Models;
using Assignment7.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Assignment7.Persistence.Models;

[Table("borrow")]
public partial class Borrow
{
    [Key]
    [Column("borrowid")]
    public int Borrowid { get; set; }

    [Column("userid")]
    public string Userid { get; set; } = null!;

    [Column("bookid")]
    public int Bookid { get; set; }

    [Column("dateofborrow")]
    public DateOnly? Dateofborrow { get; set; }

    [Column("deadlinereturn")]
    public DateOnly? Deadlinereturn { get; set; }

    [Column("dateofreturn")]
    public DateOnly? Dateofreturn { get; set; }

    [Column("penalty")]
    [StringLength(50)]
    public string? Penalty { get; set; }

    [ForeignKey("Bookid")]
    [InverseProperty("Borrows")]
    public virtual Book? Book { get; set; }

    [ForeignKey("Userid")]
    [InverseProperty("Borrows")]
    public virtual AppUser? User { get; set; }

    [NotMapped]
    public DateOnlyObject? DateOfBorrowObject { get; set; }
    [NotMapped]
    public DateOnlyObject? DeadlinereturnObject { get; set; }
    [NotMapped]
    public DateOnlyObject? DateOfReturnObject { get; set; }

    public void ConvertDateOfBorrowToDateOnly()
    {
        if (DateOfBorrowObject != null)
        {
            Dateofborrow = new DateOnly(DateOfBorrowObject.Year, DateOfBorrowObject.Month, DateOfBorrowObject.Day);
        }
    }
    public void ConvertDeadliniReturnToDateOnly()
    {
        if (DeadlinereturnObject != null)
        {
            Deadlinereturn = new DateOnly(DeadlinereturnObject.Year, DeadlinereturnObject.Month, DeadlinereturnObject.Day);
        }
    }
    public void ConvertDateOfReturnToDateOnly()
    {
        if (DateOfReturnObject != null)
        {
            Dateofreturn = new DateOnly(DateOfReturnObject.Year, DateOfReturnObject.Month, DateOfReturnObject.Day);
        }

    }
}
