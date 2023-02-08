using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO;
/// <summary>
/// DTO class used to return CountryResponse
/// </summary>
public class CountryResponse
{
    public Guid CountryId { get; set; }
    public string? CountryName { get; set; }

   
}
public static class CountryExtensions
{
    public static CountryResponse ToCountryResponse(Country country)
    {
        return new CountryResponse { CountryId = country.CountryId, CountryName = country.CountryName };
    }
}
