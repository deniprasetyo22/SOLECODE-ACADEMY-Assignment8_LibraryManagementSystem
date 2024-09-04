using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Assignment7.Persistence.Models;

[Table("bookrequest")]
public partial class Bookrequest
{
    [Key]
    [Column("requestid")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Requestid { get; set; }

    [Column("requestname")]
    [StringLength(255)]
    public string? Requestname { get; set; }

    [Column("description")]
    public string? Description { get; set; }

    [Column("startdate", TypeName = "timestamp without time zone")]
    public DateTime? Startdate { get; set; } = DateTime.Now;

    [Column("enddate", TypeName = "timestamp without time zone")]
    public DateTime? Enddate { get; set; }

    [Column("title")]
    [StringLength(255)]
    public string? Title { get; set; }

    [Column("isbn")]
    [StringLength(255)]
    public string? Isbn { get; set; }

    [Column("author")]
    [StringLength(255)]
    public string? Author { get; set; }

    [Column("publisher")]
    [StringLength(255)]
    public string? Publisher { get; set; }

    [Column("processid")]
    public int? Processid { get; set; }

    [ForeignKey("Processid")]
    [InverseProperty("Bookrequests")]
    public virtual Process? Process { get; set; }
}
