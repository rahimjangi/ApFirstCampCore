using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;


namespace Services;

public class PersonService : IPersonService
{
    private readonly ApplicationDbContext _db;
    private readonly ICountriesService _countryService;

    public PersonService(ApplicationDbContext dbContext)
    {
        _db = dbContext;
        _countryService = new CountriesService(_db);
    }

    private  PersonResponse? ConvertPersonToPersonResponse(Person person)
    {

        //_db.Database.ExecuteSqlRaw("select * from country where countryid={0}", 1);
        //_db.Persons.FromSqlRaw<Person>("select * from person where personid={0}",1);

        if (person == null) return null;
        var genderOptions = GenderOptions.Other;
        PersonResponse personResponse= new PersonResponse() {

            PersonId = person.PersonId,
            CountryId = person.CountryId,
            PersonName = person.PersonName,
            Email = person.Email,
            DateOfBirth = person.DateOfBirth,
            Age = (person.DateOfBirth != null) ? (int)Math.Round((DateTime.Now - person.DateOfBirth).GetValueOrDefault().TotalDays / 365.24) : 0,
            Address = person.Address,
            Gender = (Enum.TryParse<GenderOptions>(person.Gender,true,out genderOptions)?genderOptions:GenderOptions.Other),
            ReceiveNewsLetters = person.ReceiveNewsLetters
        };
        //PersonResponse response = person.ToPersonResponse();
        //response.Country=_countryService.GetCountryByCountryId(person.CountryId)?.CountryName;
        //return response;
        personResponse.Country = _countryService.GetCountryByCountryId(person.CountryId)?.CountryName;
        return personResponse;
    }

    public PersonResponse? AddPerson(PersonAddRequest? personAddRequest)
    {
        if(personAddRequest == null)throw new ArgumentNullException(nameof(personAddRequest));
        //if(String.IsNullOrEmpty(personAddRequest.PersonName)) throw new ArgumentException(nameof(personAddRequest.PersonName));

        //ValidationContext validationContext = new ValidationContext(personAddRequest);
        //List<ValidationResult> validationResults = new List<ValidationResult>();
        //bool isValid=Validator.TryValidateObject(personAddRequest, validationContext,validationResults,true);
        //if(!isValid)
        //{
        //    throw new ArgumentException(validationResults.FirstOrDefault()?.ErrorMessage);
        //}
        var pp=_db.sp_addPerson(personAddRequest.ToPerson());

        ModelValidate.Validate(personAddRequest);

        Person person=personAddRequest.ToPerson();
        person.PersonId=Guid.NewGuid();
        _db.Add(person);
        _db.SaveChanges();
        PersonResponse? personResponse= _db.Persons.FirstOrDefault(x=>x.PersonId==person.PersonId)?.ToPersonResponse();
        personResponse.Country=_countryService.GetCountryByCountryId(person.CountryId)?.CountryName;
        return personResponse;
        //return PersonList.FirstOrDefault(x => x.PersonId == person.PersonId).ToPersonResponse();
            
        //return person.ToPersonResponse();

    }


    public List<PersonResponse> GetAllPersons()
    {
        //_db.sp_getAllPersons();
        //_db.Database.FromSqlRaw("EXECUTE [dbo].[GetAllPersons]", new { }).ToList();
        //List<PersonResponse> personResponses = new List<PersonResponse>();
        //foreach (Person person in _db.Persons.ToList())
        //{
        //    personResponses.Add(ConvertPersonToPersonResponse(person));
        //}
        //_db.SaveChanges();
        //return personResponses;
        //return _db.Persons.ToList().Select(x => ConvertPersonToPersonResponse(x)).ToList();
        return _db.sp_getAllPersons().ToList().Select(x => ConvertPersonToPersonResponse(x)).ToList();
    }

    public List<PersonResponse>? GetFilteredPersons(string searchBy, string? searchString)
    {


        List<PersonResponse> allPersons = GetAllPersons();
        List<PersonResponse> matchingPersons = allPersons;

        if (string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty(searchString))
            return matchingPersons;

        switch (searchBy)
        {
            case nameof(PersonResponse.PersonName):
                matchingPersons = allPersons.Where(temp =>
                (!string.IsNullOrEmpty(temp.PersonName) ?
                temp.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                break;

            case nameof(PersonResponse.Email):
                matchingPersons = allPersons.Where(temp =>
                (!string.IsNullOrEmpty(temp.Email) ?
                temp.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                break;


            case nameof(PersonResponse.DateOfBirth):
                matchingPersons = allPersons.Where(temp =>
                (temp.DateOfBirth != null) ?
                temp.DateOfBirth.Value.ToString("dd MMMM yyyy").Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                break;

            case nameof(PersonResponse.Gender):
                matchingPersons = allPersons.Where(temp =>
                (!string.IsNullOrEmpty(temp.Gender.ToString()) ?
                temp.Gender.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                break;

            case nameof(PersonResponse.Country):
                matchingPersons = allPersons.Where(temp =>
                (!string.IsNullOrEmpty(temp.Country) ?
                temp.Country.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                break;

            case nameof(PersonResponse.Address):
                matchingPersons = allPersons.Where(temp =>
                (!string.IsNullOrEmpty(temp.Address) ?
                temp.Address.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                break;

            default: matchingPersons = allPersons; break;
        }
        return matchingPersons;
    }

    public PersonResponse? GetPersonByPersonId(Guid? personId)
    {
        if(personId==null) throw new ArgumentNullException(nameof(personId));
        Person? person=_db.Persons.FirstOrDefault(x=>x.PersonId== personId);
        return ConvertPersonToPersonResponse(person);
    }

    public List<PersonResponse> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder)
    {
        List<PersonResponse> matchingPersons = allPersons;

        if (string.IsNullOrEmpty(sortBy) || string.IsNullOrEmpty(sortOrder.ToString()))
            return matchingPersons.OrderBy(x=>x.PersonName).ToList();

        switch (sortBy)
        {
            case nameof(PersonResponse.PersonName):
                if (sortOrder.Equals(SortOrderOptions.ASC)) {
                    matchingPersons = allPersons.OrderBy(x => x.PersonName).ToList();
                }
                else
                {
                    matchingPersons = allPersons.OrderByDescending(x => x.PersonName).ToList();
                }
                break;

            case nameof(PersonResponse.Email):
                if (sortOrder.Equals(SortOrderOptions.ASC))
                {
                    matchingPersons = allPersons.OrderBy(x => x.Email).ToList();
                }
                else
                {
                    matchingPersons = allPersons.OrderByDescending(x => x.Email).ToList();
                }
                break;


            case nameof(PersonResponse.DateOfBirth):
                if (sortOrder.Equals(SortOrderOptions.ASC))
                {
                    matchingPersons = allPersons.OrderBy(x => x.DateOfBirth).ToList();
                }
                else
                {
                    matchingPersons = allPersons.OrderByDescending(x => x.DateOfBirth).ToList();
                }
                break;

            case nameof(PersonResponse.Gender):
                if (sortOrder.Equals(SortOrderOptions.ASC))
                {
                    matchingPersons = allPersons.OrderBy(x => x.Gender).ToList();
                }
                else
                {
                    matchingPersons = allPersons.OrderByDescending(x => x.Gender).ToList();
                }
                break;

            case nameof(PersonResponse.Country):
                if (sortOrder.Equals(SortOrderOptions.ASC))
                {
                    matchingPersons = allPersons.OrderBy(x => x.Country).ToList();
                }
                else
                {
                    matchingPersons = allPersons.OrderByDescending(x => x.Country).ToList();
                }
                break;

            case nameof(PersonResponse.Address):
                if (sortOrder.Equals(SortOrderOptions.ASC))
                {
                    matchingPersons = allPersons.OrderBy(x => x.Address).ToList();
                }
                else
                {
                    matchingPersons = allPersons.OrderByDescending(x => x.Address).ToList();
                }
                break;
            case nameof(PersonResponse.Age):
                if (sortOrder.Equals(SortOrderOptions.ASC))
                {
                    matchingPersons = allPersons.OrderBy(x => x.Age).ToList();
                }
                else
                {
                    matchingPersons = allPersons.OrderByDescending(x => x.Age).ToList();
                }
                break;

            default: matchingPersons = allPersons; break;
        }
        return matchingPersons;
    }

    public PersonResponse? UpdatePerson(PersonUpdateRequest? personUpdateRequest)
    {
        if (personUpdateRequest == null) throw new ArgumentNullException(nameof(personUpdateRequest));
        Helpers.ModelValidate.Validate(personUpdateRequest);
        Person? person = _db.Persons.FirstOrDefault(x => x.PersonId == personUpdateRequest.PersonId);
        if (person == null) throw new ArgumentException("Requested object does not exist");
        _db.Remove(person);
        _db.Add(personUpdateRequest.ToPerson());
        _db.SaveChanges();
        return _db.Persons.FirstOrDefault(x => x.PersonId == personUpdateRequest.PersonId)?.ToPersonResponse();
    }
    public void DeletePerson(Guid? PersonId)
    {
        if (PersonId == null) throw new ArgumentNullException(nameof(PersonId));
        Person? person = _db.Persons.FirstOrDefault(x => x.PersonId == PersonId);
        if (person == null) throw new ArgumentException("Requested object does not exist");
        _db.Remove(person);
        _db.SaveChanges();
    }
}
