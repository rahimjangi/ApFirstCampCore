using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services;

public class CountriesService : ICountriesService
{
    private readonly ApplicationDbContext _db;

    public CountriesService(ApplicationDbContext dbContext)
    {
        _db = dbContext;
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
       if(_db.Countries.Where(x=>x.CountryName== countryAddRequest.CountryName).Any())
        {
            throw new ArgumentException(nameof(countryAddRequest.CountryName));
        }

        Country country = countryAddRequest.ToCountry();
        //country.CountryId= Guid.NewGuid();
            //new Country() { CountryId = Guid.NewGuid(), CountryName = countryAddRequest.CountryName };
        Country addedCountry=_db.Countries.Add(country).Entity;
        _db.SaveChanges();
        return CountryExtensions.ToCountryResponse(addedCountry);


    }

    public IEnumerable<CountryResponse> GetAll()
    {

        return _db.Countries.ToList().Select(country => country.ToCountryResponse());
    }

    public CountryResponse GetCountryByCountryId(Guid? countryId)
    {
        return _db.Countries.ToList().Where(x => x.CountryId == countryId).Select(item=>item.ToCountryResponse()).FirstOrDefault();
    }
}