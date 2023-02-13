using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities;
/// <summary>
/// Domain model for COUNTRY
/// </summary>
public class Country
{
    [Key]
    public Guid CountryId { get; set; }
    [StringLength(25)]
    public string? CountryName { get; set; }    
}
