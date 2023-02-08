using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities;
/// <summary>
/// Domain model for COUNTRY
/// </summary>
public class Country
{
    public Guid CountryId { get; set; }
    public string? CountryName { get; set; }    
}
