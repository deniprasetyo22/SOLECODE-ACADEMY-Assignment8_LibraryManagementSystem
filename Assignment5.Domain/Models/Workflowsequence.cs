using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Assignment7.Persistence.Models;

[Table("workflowsequences")]
public partial class Workflowsequence
{
    [Key]
    [Column("stepid")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Stepid { get; set; }

    [Column("workflowid")]
    public int? Workflowid { get; set; }

    [Column("steporder")]
    public int? Steporder { get; set; }

    [Column("stepname")]
    [StringLength(255)]
    public string? Stepname { get; set; }

    [Column("requiredrole")]
    [StringLength(255)]
    public string? Requiredrole { get; set; }

    [Column("nextstepid")]
    public int? Nextstepid { get; set; }

    [InverseProperty("Nextstep")]
    public virtual ICollection<Workflowsequence> InverseNextstep { get; set; } = new List<Workflowsequence>();

    [ForeignKey("Nextstepid")]
    [InverseProperty("InverseNextstep")]
    public virtual Workflowsequence? Nextstep { get; set; }

    [InverseProperty("Currentstep")]
    public virtual ICollection<Process> Processes { get; set; } = new List<Process>();

    [ForeignKey("Requiredrole")]
    [InverseProperty("Workflowsequences")]
    public virtual AspNetRole? RequiredroleNavigation { get; set; }

    [ForeignKey("Workflowid")]
    [InverseProperty("Workflowsequences")]
    public virtual Workflow? Workflow { get; set; }

    [InverseProperty("Step")]
    public virtual ICollection<Workflowaction> Workflowactions { get; set; } = new List<Workflowaction>();
}
