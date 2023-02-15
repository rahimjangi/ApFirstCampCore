using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities;

/// <summary>
/// Person domain model class
/// </summary>
public class Person
{
    [Key]
    public Guid PersonId { get; set; }
    [StringLength(50)]
    public string? PersonName { get; set; }
    [StringLength(50)]
    public string? Email { get; set;}
    public DateTime? DateOfBirth { get; set; }
    [StringLength(6)]
    public string? Gender { get; set; }
    public Guid? CountryId { get; set; }
    [StringLength(150)]
    public string? Address { get; set; }
    public bool ReceiveNewsLetters { get; set; }
    public string? TIM { get; set; }
    [ForeignKey("CountryId")]
    public virtual Country? Country  { get; set; }
}
