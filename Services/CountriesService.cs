using Entities;
using Microsoft.EntityFrameworkCore;
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

    public async Task<CountryResponse?> AddCountry(CountryAddRequest? countryAddRequest)
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
        
         Country savedCountry=(await _db.Countries.AddAsync(country)).Entity;
        await _db.SaveChangesAsync();
        return CountryExtensions.ToCountryResponse(savedCountry);


    }

    public async Task<List<CountryResponse>> GetAll()
    {
        List<Country>?countryList = await _db.Countries.ToListAsync();
        return countryList.Select(country=>country.ToCountryResponse()).ToList();
    }

    public async Task<CountryResponse?> GetCountryByCountryId(Guid? countryId)
    {
        Country? country=await _db.Countries.FirstOrDefaultAsync(country=>country.CountryId==countryId);
        return country.ToCountryResponse();
    }


}