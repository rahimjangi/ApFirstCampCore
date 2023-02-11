using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services;

public class CountriesService : ICountriesService
{
    private readonly List<Country> CountryList;

    public CountriesService(bool initialize=true)
    {
        CountryList = new List<Country>();
        if (initialize)
        {

            CountryList.AddRange(new List<Country>()
            { new Country() { CountryId = Guid.Parse("000C76EB-62E9-4465-96D1-2C41FDB64C3B"), CountryName = "USA" },
            new Country() { CountryId = Guid.Parse("32DA506B-3EBA-48A4-BD86-5F93A2E19E3F"), CountryName = "UK" },
            new Country() { CountryId = Guid.Parse("DF7C89CE-3341-4246-84AE-E01AB7BA476E"), CountryName = "ITALY" },
            new Country() { CountryId = Guid.Parse("A734A796-565D-4B5F-925B-8487ABFCF5EE"), CountryName = "GERMANY" },
            new Country() { CountryId = Guid.Parse("15889048-AF93-412C-B8F3-22103E943A6D"), CountryName = "CANADA" },
            new Country() { CountryId = Guid.Parse("80DF255C-EFE7-49E5-A7F9-C35D7C701CAB"), CountryName = "SPAIN" },
            new Country() { CountryId = Guid.Parse("C2396C3E-F5AC-40C0-A4A3-25D967D54095"), CountryName = "FRANCE" }
            });
        }
    }

    public CountryResponse AddCountry(CountryAddRequest? countryAddRequest)
    {
        if (countryAddRequest == null)
        {
            throw new ArgumentNullException(nameof(countryAddRequest));
        }
        if(countryAddRequest.CountryName== null)
        {
            throw new ArgumentException(nameof(countryAddRequest.CountryName));
        }
       if(CountryList.Where(x=>x.CountryName== countryAddRequest.CountryName).Any())
        {
            throw new ArgumentException(nameof(countryAddRequest.CountryName));
        }

        Country country = countryAddRequest.ToCountry();
        country.CountryId= Guid.NewGuid();
            //new Country() { CountryId = Guid.NewGuid(), CountryName = countryAddRequest.CountryName };
        CountryList.Add(country);
        return CountryExtensions.ToCountryResponse(country);


    }

    public IEnumerable<CountryResponse> GetAll()
    {

        return CountryList.Select(country => country.ToCountryResponse());
    }

    public CountryResponse GetCountryByCountryId(Guid? countryId)
    {
        return CountryList.Where(x => x.CountryId == countryId).Select(item=>item.ToCountryResponse()).FirstOrDefault();
    }
}