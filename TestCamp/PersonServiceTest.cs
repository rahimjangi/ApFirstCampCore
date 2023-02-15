using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace TestCamp;

public class PersonServiceTest
{
    private const string ConnectionString = @"Data Source=(localdb)\ProjectModels;Initial Catalog=PersonsDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
    private readonly IPersonService _personService;
    private readonly ICountriesService _countryService;
    private readonly ITestOutputHelper _testOutputHelper;

    public PersonServiceTest(ITestOutputHelper testOutputHelper)
    {
        _personService = new PersonService(new ApplicationDbContext(
            new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlServer(ConnectionString).Options));

        _countryService = new CountriesService(
             new ApplicationDbContext(
            new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlServer(ConnectionString).Options));
        _testOutputHelper = testOutputHelper;
    }
    #region AddPerson
    [Fact]
    public async Task AddPerson_NullPerson() {
        PersonAddRequest? personAddRequest = null;
        await Assert.ThrowsAsync<ArgumentNullException>(async() =>await _personService.AddPerson(personAddRequest));
    }

    [Fact]
    public async Task AddPerson_NullPersonName()
    {
        PersonAddRequest? personAddRequest =new PersonAddRequest()
        {
            PersonName= null,

        };
        Assert.ThrowsAsync<ArgumentException>(async()=> await _personService.AddPerson(personAddRequest));
    }

    [Fact]
    public async Task AddPerson_ProperObject() {
        CountryResponse countryResponse= await _countryService.AddCountry(new CountryAddRequest() { CountryName = "USA" });
        PersonAddRequest? personAddRequest = new PersonAddRequest() { 
            PersonName= "Rahim",
            Address="Address",
            DateOfBirth=new DateTime(DateTime.Now.Year - 40,9,23),
            Email="email@gmail.com",
            Gender=GenderOptions.Male,
            ReceiveNewsLetters=true,
            CountryId=countryResponse.CountryId
        };
        PersonResponse personResponse= await _personService.AddPerson(personAddRequest);
        Assert.NotNull(personResponse);
        Assert.True(personResponse.PersonId!=Guid.Empty);
        Assert.Contains(personResponse,await _personService.GetAllPersons());
    }
    #endregion

    #region GetPersonByPersonId
    [Fact]
    public async Task GetPersonByPersonId_NullPersonId()
    {
       
        var personId=Guid.Empty;
        PersonResponse? personResponse=await _personService.GetPersonByPersonId(personId);
        Assert.Null(personResponse);
    }

    [Fact]
    public async Task GetPersonByPersonId_ValidPersonId()
    {
        CountryResponse countryResponse = await _countryService.AddCountry(new CountryAddRequest() { CountryName = "USA new" });
        PersonAddRequest? personAddRequest = new PersonAddRequest()
        {
            PersonName = "Rahim",
            Address = "Address",
            DateOfBirth = new DateTime(DateTime.Now.Year - 40, 9, 23),
            Email = "emailsdf@gmail.com",
            Gender = GenderOptions.Male,
            ReceiveNewsLetters = true,
            CountryId = countryResponse.CountryId
        };
        PersonResponse? personResponse =await _personService.AddPerson(personAddRequest);
        Assert.NotNull(personResponse);
        Assert.NotNull(personResponse.PersonId);
        Assert.Equal(personResponse.PersonId, (await _personService.GetPersonByPersonId(personResponse.PersonId)).PersonId);
    }
    #endregion

    #region GetAllPersons
    [Fact]
    public async Task GetAllPersons_EmptyList()
    {
        Assert.Empty(await _personService.GetAllPersons());
    }

    [Fact]
    public async Task GetAllPersons_NotEmptyList()
    {
        CountryResponse countryResponse =await _countryService.AddCountry(new CountryAddRequest() { CountryName = "USA" });
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
        Assert.NotNull(_personService.GetAllPersons());
    }
    #endregion

    #region GetFilteredPersons
    [Fact]
    public async Task GetFilteredPersons_EmptySearchText()
    {
        //Arrange
        CountryAddRequest country_request_1 = new CountryAddRequest() { CountryName = "USA" };
        CountryAddRequest country_request_2 = new CountryAddRequest() { CountryName = "India" };

        CountryResponse country_response_1 =await _countryService.AddCountry(country_request_1);
        CountryResponse country_response_2 = await _countryService.AddCountry(country_request_2);

        PersonAddRequest person_request_1 = new PersonAddRequest() { PersonName = "Smith", Email = "smith@example.com", Gender = GenderOptions.Male, Address = "address of smith", CountryId = country_response_1.CountryId, DateOfBirth = DateTime.Parse("2002-05-06"), ReceiveNewsLetters = true };

        PersonAddRequest person_request_2 = new PersonAddRequest() { PersonName = "Mary", Email = "mary@example.com", Gender = GenderOptions.Female, Address = "address of mary", CountryId = country_response_2.CountryId, DateOfBirth = DateTime.Parse("2000-02-02"), ReceiveNewsLetters = false };

        PersonAddRequest person_request_3 = new PersonAddRequest() { PersonName = "Rahman", Email = "rahman@example.com", Gender = GenderOptions.Male, Address = "address of rahman", CountryId = country_response_2.CountryId, DateOfBirth = DateTime.Parse("1999-03-03"), ReceiveNewsLetters = true };

        List<PersonAddRequest> person_requests = new List<PersonAddRequest>() { person_request_1, person_request_2, person_request_3 };

        List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

        foreach (PersonAddRequest person_request in person_requests)
        {
            PersonResponse person_response =await  _personService.AddPerson(person_request);
            person_response_list_from_add.Add(person_response);
        }

        //print person_response_list_from_add
        _testOutputHelper.WriteLine("Expected:");
        foreach (PersonResponse person_response_from_add in person_response_list_from_add)
        {
            _testOutputHelper.WriteLine(person_response_from_add.ToString());
        }

        //Act
        List<PersonResponse> persons_list_from_search =await _personService.GetFilteredPersons(nameof(Person.PersonName), "");

        //print persons_list_from_get
        _testOutputHelper.WriteLine("Actual:");
        foreach (PersonResponse person_response_from_get in persons_list_from_search)
        {
            _testOutputHelper.WriteLine(person_response_from_get.ToString());
        }

        //Assert
        foreach (PersonResponse person_response_from_add in person_response_list_from_add)
        {
            Assert.Contains(person_response_from_add, persons_list_from_search);
        }
    }


    //First we will add few persons; and then we will search based on person name with some search string. It should return the matching persons
    [Fact]
    public async Task GetFilteredPersons_SearchByPersonName()
    {
        //Arrange
        CountryAddRequest country_request_1 = new CountryAddRequest() { CountryName = "USA" };
        CountryAddRequest country_request_2 = new CountryAddRequest() { CountryName = "India" };

        CountryResponse country_response_1 =await  _countryService.AddCountry(country_request_1);
        CountryResponse country_response_2 = await _countryService.AddCountry(country_request_2);

        PersonAddRequest person_request_1 = new PersonAddRequest() { PersonName = "Smith", Email = "smith@example.com", Gender = GenderOptions.Male, Address = "address of smith", CountryId = country_response_1.CountryId, DateOfBirth = DateTime.Parse("2002-05-06"), ReceiveNewsLetters = true };

        PersonAddRequest person_request_2 = new PersonAddRequest() { PersonName = "Mary", Email = "mary@example.com", Gender = GenderOptions.Female, Address = "address of mary", CountryId = country_response_2.CountryId, DateOfBirth = DateTime.Parse("2000-02-02"), ReceiveNewsLetters = false };

        PersonAddRequest person_request_3 = new PersonAddRequest() { PersonName = "Rahman", Email = "rahman@example.com", Gender = GenderOptions.Male, Address = "address of rahman", CountryId = country_response_2.CountryId, DateOfBirth = DateTime.Parse("1999-03-03"), ReceiveNewsLetters = true };

        List<PersonAddRequest> person_requests = new List<PersonAddRequest>() { person_request_1, person_request_2, person_request_3 };

        List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

        foreach (PersonAddRequest person_request in person_requests)
        {
            PersonResponse person_response = await _personService.AddPerson(person_request);
            person_response_list_from_add.Add(person_response);
        }

        //print person_response_list_from_add
        _testOutputHelper.WriteLine("Expected:");
        foreach (PersonResponse person_response_from_add in person_response_list_from_add)
        {
            _testOutputHelper.WriteLine(person_response_from_add.ToString());
        }

        //Act
        List<PersonResponse> persons_list_from_search =await _personService.GetFilteredPersons(nameof(Person.PersonName), "ma");

        //print persons_list_from_get
        _testOutputHelper.WriteLine("Actual:");
        foreach (PersonResponse person_response_from_get in persons_list_from_search)
        {
            _testOutputHelper.WriteLine(person_response_from_get.ToString());
        }

        //Assert
        foreach (PersonResponse person_response_from_add in person_response_list_from_add)
        {
            if (person_response_from_add.PersonName != null)
            {
                if (person_response_from_add.PersonName.Contains("ma", StringComparison.OrdinalIgnoreCase))
                {
                    Assert.Contains(person_response_from_add, persons_list_from_search);
                }
            }
        }
    }
    #endregion

    #region GetSortedPersons
    [Fact]
    public async Task GetSortedPersons_ASC()
    {
        //Arrange
        CountryAddRequest country_request_1 = new CountryAddRequest() { CountryName = "USA" };
        CountryAddRequest country_request_2 = new CountryAddRequest() { CountryName = "India" };

        CountryResponse country_response_1 = await  _countryService.AddCountry(country_request_1);
        CountryResponse country_response_2 = await _countryService.AddCountry(country_request_2);

        PersonAddRequest person_request_1 = new PersonAddRequest() { PersonName = "Smith", Email = "smith@example.com", Gender = GenderOptions.Male, Address = "address of smith", CountryId = country_response_1.CountryId, DateOfBirth = DateTime.Parse("2002-05-06"), ReceiveNewsLetters = true };

        PersonAddRequest person_request_2 = new PersonAddRequest() { PersonName = "Mary", Email = "mary@example.com", Gender = GenderOptions.Female, Address = "address of mary", CountryId = country_response_2.CountryId, DateOfBirth = DateTime.Parse("2000-02-02"), ReceiveNewsLetters = false };

        PersonAddRequest person_request_3 = new PersonAddRequest() { PersonName = "Rahman", Email = "rahman@example.com", Gender = GenderOptions.Male, Address = "address of rahman", CountryId = country_response_2.CountryId, DateOfBirth = DateTime.Parse("1999-03-03"), ReceiveNewsLetters = true };

        List<PersonAddRequest> person_requests = new List<PersonAddRequest>() { person_request_1, person_request_2, person_request_3 };

        List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

        foreach (PersonAddRequest person_request in person_requests)
        {
            PersonResponse person_response = await _personService.AddPerson(person_request);
            person_response_list_from_add.Add(person_response);
        }

        //print person_response_list_from_add
        _testOutputHelper.WriteLine("Expected:");
        foreach (PersonResponse person_response_from_add in person_response_list_from_add)
        {
            _testOutputHelper.WriteLine(person_response_from_add.ToString());
        }
        List<PersonResponse> allPersons=await _personService.GetAllPersons();
        //Act
        List<PersonResponse> persons_list_from_sort =await _personService.GetSortedPersons(allPersons,nameof(Person.PersonName), SortOrderOptions.ASC);

        //print persons_list_from_get
        _testOutputHelper.WriteLine("Actual:");
        foreach (PersonResponse person_response_from_get in persons_list_from_sort)
        {
            _testOutputHelper.WriteLine(person_response_from_get.ToString());
        }

        person_response_list_from_add= person_response_list_from_add.OrderBy(x=>x.PersonName).ToList();

        //Assert
       for(int i=0; i<person_response_list_from_add.Count;i++)
        {
            Assert.Equal(person_response_list_from_add.ElementAt(i), persons_list_from_sort.ElementAt(i));
        }

    }

    [Fact]
    public async Task GetSortedPersons_DESC()
    {
        //Arrange
        CountryAddRequest country_request_1 = new CountryAddRequest() { CountryName = "USA" };
        CountryAddRequest country_request_2 = new CountryAddRequest() { CountryName = "India" };

        CountryResponse country_response_1 =await _countryService.AddCountry(country_request_1);
        CountryResponse country_response_2 = await _countryService.AddCountry(country_request_2);

        PersonAddRequest person_request_1 = new PersonAddRequest() { PersonName = "Smith", Email = "smith@example.com", Gender = GenderOptions.Male, Address = "address of smith", CountryId = country_response_1.CountryId, DateOfBirth = DateTime.Parse("2002-05-06"), ReceiveNewsLetters = true };

        PersonAddRequest person_request_2 = new PersonAddRequest() { PersonName = "Mary", Email = "mary@example.com", Gender = GenderOptions.Female, Address = "address of mary", CountryId = country_response_2.CountryId, DateOfBirth = DateTime.Parse("2000-02-02"), ReceiveNewsLetters = false };

        PersonAddRequest person_request_3 = new PersonAddRequest() { PersonName = "Rahman", Email = "rahman@example.com", Gender = GenderOptions.Male, Address = "address of rahman", CountryId = country_response_2.CountryId, DateOfBirth = DateTime.Parse("1999-03-03"), ReceiveNewsLetters = true };

        List<PersonAddRequest> person_requests = new List<PersonAddRequest>() { person_request_1, person_request_2, person_request_3 };

        List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

        foreach (PersonAddRequest person_request in person_requests)
        {
            PersonResponse person_response =await _personService.AddPerson(person_request);
            person_response_list_from_add.Add(person_response);
        }

        //print person_response_list_from_add
        _testOutputHelper.WriteLine("Expected:");
        foreach (PersonResponse person_response_from_add in person_response_list_from_add)
        {
            _testOutputHelper.WriteLine(person_response_from_add.ToString());
        }
        List<PersonResponse> allPersons = await _personService.GetAllPersons();
        //Act
        List<PersonResponse> persons_list_from_sort =await _personService.GetSortedPersons(allPersons, nameof(Person.PersonName), SortOrderOptions.DESC);

        //print persons_list_from_get
        _testOutputHelper.WriteLine("Actual:");
        foreach (PersonResponse person_response_from_get in persons_list_from_sort)
        {
            _testOutputHelper.WriteLine(person_response_from_get.ToString());
        }

        person_response_list_from_add = person_response_list_from_add.OrderByDescending(x => x.PersonName).ToList();

        //Assert
        for (int i = 0; i < person_response_list_from_add.Count; i++)
        {
            Assert.Equal(person_response_list_from_add.ElementAt(i), persons_list_from_sort.ElementAt(i));
        }

    }
    #endregion

    #region UpdatePerson

    [Fact]
    public async Task UpdatePerson_NullPerson()
    {
        PersonUpdateRequest? personUpdateRequest = null;
        Assert.ThrowsAsync<ArgumentNullException>(async() => await _personService.UpdatePerson(personUpdateRequest));

    }

    [Fact]
    public async Task UpdatePerson_NullPersonID()
    {
        PersonUpdateRequest? personUpdateRequest = new PersonUpdateRequest() { PersonId= Guid.NewGuid()};
        Assert.ThrowsAsync<ArgumentException>(async() =>await  _personService.UpdatePerson(personUpdateRequest));

    }

    [Fact]
    public async Task UpdatePerson_PersonDetail()
    {
        CountryResponse countryResponse =await _countryService.AddCountry(new CountryAddRequest() { CountryName = "USA" });
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
        PersonResponse? personResponse =await _personService.AddPerson(personAddRequest);

        PersonUpdateRequest? personUpdateRequest = new PersonUpdateRequest()
        {
            PersonId = personResponse.PersonId,
            PersonName= "MASHRAHIM",
            Address= personResponse.Address,
            CountryId= personResponse.CountryId,
            DateOfBirth= personResponse.DateOfBirth,
            Email= personResponse.Email,
            Gender= personResponse.Gender,
            ReceiveNewsLetters= personResponse.ReceiveNewsLetters
        };

        PersonResponse? updatedPerson =await _personService.UpdatePerson(personUpdateRequest);

        Assert.NotNull(updatedPerson);
        Assert.NotEqual(updatedPerson,personResponse);

    }
    #endregion

    #region DeletePerson
    [Fact]
    public async Task DeletePerson_NullPersonId() {
        Assert.ThrowsAny<ArgumentException>(async () => { 
            await _personService.DeletePerson(Guid.Empty);
        });
    }

    [Fact]
    public async Task DeletePerson_PersonId() {
        CountryResponse countryResponse = await _countryService.AddCountry(new CountryAddRequest() { CountryName = "USA" });
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
        PersonResponse? personResponse = await _personService.AddPerson(personAddRequest);
        Assert.True((await _personService.GetAllPersons()).Count == 1);
        _personService.DeletePerson(personResponse?.PersonId);
        Assert.True((await _personService.GetAllPersons()).Count==0);
    }
    #endregion

}
