using Entities;
using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts;

/// <summary>
/// Busines Logic of Country Entity
/// </summary>
public interface ICountriesService
{   
    /// <summary>
    /// Adds Country Object to the list of Countries
    /// </summary>
    /// <param name="countryAddRequest">Country Object to add</param>
    /// <returns>Returns the Country object after adding it to the country list(including GUID)</returns>
    CountryResponse AddCountry(CountryAddRequest? countryAddRequest);
    IEnumerable<CountryResponse> GetAll();
}
