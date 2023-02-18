using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class CountriesRepository : ICountriesRepository
    {
        private readonly ApplicationDbContext _db;

        public CountriesRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Country> AddCountry(Country country)
        {
             var savedCountry=await _db.Countries.AddAsync(country);
            await _db.SaveChangesAsync();
            return savedCountry.Entity;

        }

        public async Task<List<Country>> GetAllCountries()
        {
            var contriesFromDb = await _db.Countries.ToListAsync();
            return contriesFromDb;
        }

        public async  Task<Country?> GetCountryByCountryID(Guid countryID)
        {
            var countryFromDb = await _db.Countries.FirstOrDefaultAsync(x => x.CountryId == countryID);
            return countryFromDb;
        }

        public  async Task<Country?> GetCountryByCountryName(string countryName)
        {
            var countryFromDb=await _db.Countries.FirstOrDefaultAsync(x=>x.CountryName == countryName);
            return countryFromDb;
        }
    }
}
