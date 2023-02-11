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
        private readonly ICountriesService _countriesService;

        public CountriesServiceTest()
        {
            _countriesService = new CountriesService(false);
        }

        #region AddCountry
        [Fact]
        public void AddCountry_NullCountry()
        {
            CountryAddRequest? request = null;
            Assert.Throws<ArgumentNullException>(() => _countriesService.AddCountry(request));
        }

        [Fact]
        public void AddCountry_CountryNameIsNull()
        {
            CountryAddRequest? request = new CountryAddRequest();
            request.CountryName = null;
            Assert.Throws<ArgumentException>(()=>_countriesService.AddCountry(request));
        }

        [Fact]
        public void AddCountry_CountryNameIsDoublicate()
        {
            CountryAddRequest? request1 = new CountryAddRequest() { CountryName = "same" };
            _countriesService.AddCountry(request1);
            CountryAddRequest? request2 = new CountryAddRequest() { CountryName = "same" };
            Assert.Throws<ArgumentException>(() =>        
                    _countriesService.AddCountry(request2)
            );

        }
        [Fact]
        public void AddCountry_ProperCountryDetails()
        {
            CountryAddRequest? request = new CountryAddRequest() { CountryName="IRAN"};
            CountryResponse response= _countriesService.AddCountry(request);
            Assert.True(response.CountryId!=Guid.Empty);
        }
        #endregion

        #region GetAllCountries
        [Fact]
        public void GetAllCountries_EmptyList()
        {
            IEnumerable<CountryResponse> responses =
                _countriesService.GetAll();
            Assert.Empty(responses);
        }

        [Fact]
        public void GetAllCountries_NotEmpty()
        {
           var country= _countriesService.AddCountry(new CountryAddRequest()
            {
                CountryName = "sample"
            });
            IEnumerable<CountryResponse> responses =
            _countriesService.GetAll();
            Assert.NotEmpty(responses);
            Assert.Contains(country, responses);

        }
        #endregion

        #region GetCountryByCountryId
        [Fact]
        public void GetCountryByCountryId_NotNullCountryId()
        {
            CountryResponse country = _countriesService.AddCountry(
                new CountryAddRequest() { CountryName="USA"}
                );
          Guid guid=_countriesService.GetAll().Where( temp=>temp.CountryName==country.CountryName ).Select(country=>country.CountryId).FirstOrDefault();
            Assert.Equal(country, _countriesService.GetCountryByCountryId(guid));
        }

        [Fact]
        public void GetCountryByCountryId_NullCountryId()
        {
            Guid? guid= null;
            CountryResponse? country = _countriesService.GetCountryByCountryId(guid);
            Assert.Null(country);
        }
        #endregion
    }
}
