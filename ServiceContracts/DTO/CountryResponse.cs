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

    public override bool Equals(object? obj)
    {
        if(obj == null) return false;
        if(obj.GetType() != typeof(CountryResponse)) return false;

        CountryResponse? country =obj as CountryResponse;
        return this.CountryName==country.CountryName && this.CountryId==country.CountryId;
    }

    public override int GetHashCode()
    {
        throw new NotImplementedException();
    }
}
public static class CountryExtensions
{
    public static CountryResponse ToCountryResponse(this Country country)
    {
        return new CountryResponse { CountryId = country.CountryId, CountryName = country.CountryName };
    }
}
