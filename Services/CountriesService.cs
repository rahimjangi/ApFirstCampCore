using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        public List<Country> CountryList { get; set; }= new List<Country>();
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

            return CountryList.Select(country => CountryExtensions.ToCountryResponse(country));
        }
    }
}