using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCamp
{
    public class CountriesServiceTest
    {
        private const string ConnectionString = @"Data Source=(localdb)\ProjectModels;Initial Catalog=PersonsDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private readonly ICountriesService _countriesService;

        //constructor
        public CountriesServiceTest()
        {
            _countriesService = new CountriesService(
                new ApplicationDbContext(
            new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlServer(ConnectionString).Options)
                );
        }

        #region AddCountry
        [Fact]
        public async Task AddCountry_NullCountry()
        {
            CountryAddRequest? request = null;
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _countriesService.AddCountry(request));
        }

        [Fact]
        public async void AddCountry_CountryNameIsNull()
        {
            CountryAddRequest? request = new CountryAddRequest();
            request.CountryName = null;
           await  Assert.ThrowsAsync<ArgumentException>(async ()=>await _countriesService.AddCountry(request));
        }

        [Fact]
        public async Task AddCountry_CountryNameIsDoublicate()
        {
            CountryAddRequest? request1 = new CountryAddRequest() { CountryName = "same" };
            _countriesService.AddCountry(request1);
            CountryAddRequest? request2 = new CountryAddRequest() { CountryName = "same" };
            Assert.ThrowsAsync<ArgumentException>(async () =>        
                   await _countriesService.AddCountry(request2)
            );

        }
        [Fact]
        public async Task AddCountry_ProperCountryDetails()
        {
            CountryAddRequest? request = new CountryAddRequest() { CountryName="IRAN"};
            CountryResponse response= await  _countriesService.AddCountry(request);
            Assert.True(response.CountryId!=Guid.Empty);
        }
        #endregion

        #region GetAllCountries
        [Fact]
        public async Task GetAllCountries_EmptyList()
        {
            IEnumerable<CountryResponse> responses =
               await _countriesService.GetAll();
            Assert.Empty(responses);
        }

        [Fact]
        public async Task GetAllCountries_NotEmpty()
        {
           var country= await _countriesService.AddCountry(new CountryAddRequest()
            {
                CountryName = "sample"
            });
            IEnumerable<CountryResponse> responses =
            await _countriesService.GetAll();
            Assert.NotEmpty(responses);
            Assert.Contains(country, responses);

        }
        #endregion

        #region GetCountryByCountryId
        [Fact]
        public async Task GetCountryByCountryId_NotNullCountryId()
        {
            CountryResponse country =await _countriesService.AddCountry(
                new CountryAddRequest() { CountryName="USA"}
                );
          Guid guid=(await _countriesService.GetAll()).Where( temp=>temp.CountryName==country.CountryName ).Select(country=>country.CountryId).FirstOrDefault();
            Assert.Equal(country,(await _countriesService.GetCountryByCountryId(guid)));
        }

        [Fact]
        public async Task GetCountryByCountryId_NullCountryId()
        {
            Guid? guid= null;
            CountryResponse? country =await _countriesService.GetCountryByCountryId(guid);
            Assert.Null(country);
        }
        #endregion
    }
}
