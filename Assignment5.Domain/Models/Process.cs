using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Assignment7.Persistence.Models;

[Table("process")]
public partial class Process
{
    [Key]
    [Column("processid")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Processid { get; set; }

    [Column("workflowid")]
    public int? Workflowid { get; set; }

    [Column("requesterid", TypeName = "character varying")]
    public string? Requesterid { get; set; }

    [Column("requesttype")]
    [StringLength(255)]
    public string? Requesttype { get; set; }

    [Column("status")]
    [StringLength(255)]
    public string? Status { get; set; } = "Pending";

    [Column("currentstepid")]
    public int? Currentstepid { get; set; } = 2;

    [Column("requestdate", TypeName = "timestamp without time zone")]
    public DateTime? Requestdate { get; set; } = DateTime.Now;

    [InverseProperty("Process")] 
    public virtual ICollection<Bookrequest> Bookrequests { get; set; } = new List<Bookrequest>();

    [ForeignKey("Currentstepid")]
    [InverseProperty("Processes")]
    [JsonIgnore]
    public virtual Workflowsequence? Currentstep { get; set; }

    [ForeignKey("Requesterid")]
    [InverseProperty("Processes")]
    [JsonIgnore]
    public virtual AspNetUser? Requester { get; set; }

    [ForeignKey("Workflowid")]
    [InverseProperty("Processes")]
    [JsonIgnore]
    public virtual Workflow? Workflow { get; set; }

    [InverseProperty("Request")]
    public virtual ICollection<Workflowaction> Workflowactions { get; set; } = new List<Workflowaction>();
}
