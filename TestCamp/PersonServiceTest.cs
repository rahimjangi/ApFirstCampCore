using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCamp;

public class PersonServiceTest
{
    private readonly IPersonService _personService;
    private readonly ICountriesService _countryService;

    public PersonServiceTest()
    {
        _personService = new PersonService();
        _countryService= new CountriesService();
    }
    #region AddPerson
    [Fact]
    public void AddPerson_NullPerson() {
        PersonAddRequest? personAddRequest = null;
        Assert.Throws<ArgumentNullException>(() => _personService.AddPerson(personAddRequest));
    }

    [Fact]
    public void AddPerson_NullPersonName()
    {
        PersonAddRequest? personAddRequest =new PersonAddRequest()
        {
            PersonName= null,

        };
        Assert.Throws<ArgumentException>(()=>_personService.AddPerson(personAddRequest));
    }

    [Fact]
    public void AddPerson_ProperObject() {
        CountryResponse countryResponse= _countryService.AddCountry(new CountryAddRequest() { CountryName = "USA" });
        PersonAddRequest? personAddRequest = new PersonAddRequest() { 
            PersonName= "Rahim",
            Address="Address",
            DateOfBirth=new DateTime(DateTime.Now.Year - 40,9,23),
            Email="email",
            Gender=GenderOptions.Male,
            ReceiveNewsLetters=true,
            CountryId=countryResponse.CountryId
        };
        PersonResponse personResponse= _personService.AddPerson(personAddRequest);
        Assert.NotNull(personResponse);
        Assert.True(personResponse.PersonId!=Guid.Empty);
        Assert.Contains(personResponse, _personService.GetAllPersons());
    }
    #endregion

    #region GetPersonByPersonId
    [Fact]
    public void GetPersonByPersonId_NullPersonId()
    {
       
        var personId=Guid.Empty;
        PersonResponse? personResponse=_personService.GetPersonByPersonId(personId);
        Assert.Null(personResponse);
    }

    [Fact]
    public void GetPersonByPersonId_ValidPersonId()
    {
        CountryResponse countryResponse = _countryService.AddCountry(new CountryAddRequest() { CountryName = "USA" });
        PersonAddRequest? personAddRequest = new PersonAddRequest()
        {
            PersonName = "Rahim",
            Address = "Address",
            DateOfBirth = new DateTime(DateTime.Now.Year - 40, 9, 23),
            Email = "email",
            Gender = GenderOptions.Male,
            ReceiveNewsLetters = true,
            CountryId = countryResponse.CountryId
        };
        PersonResponse? personResponse = _personService.AddPerson(personAddRequest);
        Assert.NotNull(personResponse);
        Assert.NotNull(personResponse.PersonId);
        Assert.Equal(personResponse.PersonId, _personService.GetPersonByPersonId(personResponse.PersonId)?.PersonId);
    }
    #endregion

#region GetAllPersons
    [Fact]
    public void GetAllPersons_EmptyList()
    {

    }

    [Fact]
    public void GetAllPersons_NotEmptyList()
    {

    }
#endregion
}
