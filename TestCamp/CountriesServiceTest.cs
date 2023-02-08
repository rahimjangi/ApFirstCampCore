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
            _countriesService = new CountriesService();
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
            _countriesService.AddCountry(new CountryAddRequest()
            {
                CountryName = "sample"
            });
            IEnumerable<CountryResponse> responses =
            _countriesService.GetAll();
            Assert.NotEmpty(responses);

        }
        #endregion
    }
}
