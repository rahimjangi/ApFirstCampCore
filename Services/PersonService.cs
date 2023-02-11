using Entities;
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
    private readonly List<Person> PersonList;
    private readonly ICountriesService _countryService;

    public PersonService(bool initialize=true)
    {
        _countryService = new CountriesService(true);
        PersonList = new List<Person>();
        if(initialize)
        {
            PersonList.AddRange(new List<Person>() {
            new Person() { PersonId = Guid.Parse("8082ED0C-396D-4162-AD1D-29A13F929824"), PersonName = "Aguste", Email = "aleddy0@booking.com", DateOfBirth = DateTime.Parse("1993-01-02"), Gender = "Male", Address = "0858 Novick Terrace", ReceiveNewsLetters = false, CountryId = Guid.Parse("000C76EB-62E9-4465-96D1-2C41FDB64C3B") },
            new Person() { PersonId = Guid.Parse("06D15BAD-52F4-498E-B478-ACAD847ABFAA"), PersonName = "Jasmina", Email = "jsyddie1@miibeian.gov.cn", DateOfBirth = DateTime.Parse("1991-06-24"), Gender = "Female", Address = "0742 Fieldstone Lane", ReceiveNewsLetters = true, CountryId = Guid.Parse("32DA506B-3EBA-48A4-BD86-5F93A2E19E3F") },
            new Person() { PersonId = Guid.Parse("D3EA677A-0F5B-41EA-8FEF-EA2FC41900FD"), PersonName = "Kendall", Email = "khaquard2@arstechnica.com", DateOfBirth = DateTime.Parse("1993-08-13"), Gender = "Male", Address = "7050 Pawling Alley", ReceiveNewsLetters = false, CountryId = Guid.Parse("32DA506B-3EBA-48A4-BD86-5F93A2E19E3F") },
            new Person() { PersonId = Guid.Parse("89452EDB-BF8C-4283-9BA4-8259FD4A7A76"), PersonName = "Kilian", Email = "kaizikowitz3@joomla.org", DateOfBirth = DateTime.Parse("1991-06-17"), Gender = "Male", Address = "233 Buhler Junction", ReceiveNewsLetters = true, CountryId = Guid.Parse("DF7C89CE-3341-4246-84AE-E01AB7BA476E") },
            new Person() { PersonId = Guid.Parse("F5BD5979-1DC1-432C-B1F1-DB5BCCB0E56D"), PersonName = "Dulcinea", Email = "dbus4@pbs.org", DateOfBirth = DateTime.Parse("1996-09-02"), Gender = "Female", Address = "56 Sundown Point", ReceiveNewsLetters = false, CountryId = Guid.Parse("DF7C89CE-3341-4246-84AE-E01AB7BA476E") },
            new Person() { PersonId = Guid.Parse("A795E22D-FAED-42F0-B134-F3B89B8683E5"), PersonName = "Corabelle", Email = "cadams5@t-online.de", DateOfBirth = DateTime.Parse("1993-10-23"), Gender = "Female", Address = "4489 Hazelcrest Place", ReceiveNewsLetters = false, CountryId = Guid.Parse("15889048-AF93-412C-B8F3-22103E943A6D") },
            new Person() { PersonId = Guid.Parse("3C12D8E8-3C1C-4F57-B6A4-C8CAAC893D7A"), PersonName = "Faydra", Email = "fbischof6@boston.com", DateOfBirth = DateTime.Parse("1996-02-14"), Gender = "Female", Address = "2010 Farragut Pass", ReceiveNewsLetters = true, CountryId = Guid.Parse("80DF255C-EFE7-49E5-A7F9-C35D7C701CAB") },
            new Person() { PersonId = Guid.Parse("7B75097B-BFF2-459F-8EA8-63742BBD7AFB"), PersonName = "Oby", Email = "oclutheram7@foxnews.com", DateOfBirth = DateTime.Parse("1992-05-31"), Gender = "Male", Address = "2 Fallview Plaza", ReceiveNewsLetters = false, CountryId = Guid.Parse("80DF255C-EFE7-49E5-A7F9-C35D7C701CAB") },
            new Person() { PersonId = Guid.Parse("6717C42D-16EC-4F15-80D8-4C7413E250CB"), PersonName = "Seumas", Email = "ssimonitto8@biglobe.ne.jp", DateOfBirth = DateTime.Parse("1999-02-02"), Gender = "Male", Address = "76779 Norway Maple Crossing", ReceiveNewsLetters = false, CountryId = Guid.Parse("80DF255C-EFE7-49E5-A7F9-C35D7C701CAB") },
            new Person() { PersonId = Guid.Parse("6E789C86-C8A6-4F18-821C-2ABDB2E95982"), PersonName = "Freemon", Email = "faugustin9@vimeo.com", DateOfBirth = DateTime.Parse("1996-04-27"), Gender = "Male", Address = "8754 Becker Street", ReceiveNewsLetters = false, CountryId = Guid.Parse("80DF255C-EFE7-49E5-A7F9-C35D7C701CAB") }

        });
        }



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

        ModelValidate.Validate(personAddRequest);

        Person person=personAddRequest.ToPerson();
        person.PersonId=Guid.NewGuid();
        PersonList.Add(person);
        PersonResponse? personResponse= PersonList.FirstOrDefault(x=>x.PersonId==person.PersonId)?.ToPersonResponse();
        personResponse.Country=_countryService.GetCountryByCountryId(person.CountryId)?.CountryName;
        return personResponse;
        //return PersonList.FirstOrDefault(x => x.PersonId == person.PersonId).ToPersonResponse();
            
        //return person.ToPersonResponse();

    }


    public List<PersonResponse> GetAllPersons()
    {
       return PersonList.Select(x => x.ToPersonResponse()).ToList();
    }

    public List<PersonResponse>? GetFilteredPersons(string searchBy, string? searchString)
    {


        List<PersonResponse> allPersons = GetAllPersons();
        List<PersonResponse> matchingPersons = allPersons;

        if (string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty(searchString))
            return matchingPersons;

        switch (searchBy)
        {
            case nameof(Person.PersonName):
                matchingPersons = allPersons.Where(temp =>
                (!string.IsNullOrEmpty(temp.PersonName) ?
                temp.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                break;

            case nameof(Person.Email):
                matchingPersons = allPersons.Where(temp =>
                (!string.IsNullOrEmpty(temp.Email) ?
                temp.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                break;


            case nameof(Person.DateOfBirth):
                matchingPersons = allPersons.Where(temp =>
                (temp.DateOfBirth != null) ?
                temp.DateOfBirth.Value.ToString("dd MMMM yyyy").Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                break;

            case nameof(Person.Gender):
                matchingPersons = allPersons.Where(temp =>
                (!string.IsNullOrEmpty(temp.Gender.ToString()) ?
                temp.Gender.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                break;

            case nameof(Person.CountryId):
                matchingPersons = allPersons.Where(temp =>
                (!string.IsNullOrEmpty(temp.Country) ?
                temp.Country.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                break;

            case nameof(Person.Address):
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
        var person=PersonList.FirstOrDefault(x=>x.PersonId== personId)?.ToPersonResponse();
        return person;
    }

    public List<PersonResponse> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder)
    {
        List<PersonResponse> matchingPersons = allPersons;

        if (string.IsNullOrEmpty(sortBy) || string.IsNullOrEmpty(sortOrder.ToString()))
            return matchingPersons.OrderBy(x=>x.PersonName).ToList();

        switch (sortBy)
        {
            case nameof(Person.PersonName):
                if (sortOrder.Equals(SortOrderOptions.ASC)) {
                    matchingPersons = allPersons.OrderBy(x => x.PersonName).ToList();
                }
                else
                {
                    matchingPersons = allPersons.OrderByDescending(x => x.PersonName).ToList();
                }
                break;

            case nameof(Person.Email):
                if (sortOrder.Equals(SortOrderOptions.ASC))
                {
                    matchingPersons = allPersons.OrderBy(x => x.Email).ToList();
                }
                else
                {
                    matchingPersons = allPersons.OrderByDescending(x => x.Email).ToList();
                }
                break;


            case nameof(Person.DateOfBirth):
                if (sortOrder.Equals(SortOrderOptions.ASC))
                {
                    matchingPersons = allPersons.OrderBy(x => x.DateOfBirth).ToList();
                }
                else
                {
                    matchingPersons = allPersons.OrderByDescending(x => x.DateOfBirth).ToList();
                }
                break;

            case nameof(Person.Gender):
                if (sortOrder.Equals(SortOrderOptions.ASC))
                {
                    matchingPersons = allPersons.OrderBy(x => x.Gender).ToList();
                }
                else
                {
                    matchingPersons = allPersons.OrderByDescending(x => x.Gender).ToList();
                }
                break;

            case nameof(Person.CountryId):
                if (sortOrder.Equals(SortOrderOptions.ASC))
                {
                    matchingPersons = allPersons.OrderBy(x => x.CountryId).ToList();
                }
                else
                {
                    matchingPersons = allPersons.OrderByDescending(x => x.CountryId).ToList();
                }
                break;

            case nameof(Person.Address):
                if (sortOrder.Equals(SortOrderOptions.ASC))
                {
                    matchingPersons = allPersons.OrderBy(x => x.Address).ToList();
                }
                else
                {
                    matchingPersons = allPersons.OrderByDescending(x => x.Address).ToList();
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
        Person? person = PersonList.FirstOrDefault(x => x.PersonId == personUpdateRequest.PersonId);
        if (person == null) throw new ArgumentException("Requested object does not exist");
        PersonList.Remove(person);
        PersonList.Add(personUpdateRequest.ToPerson());
        return PersonList.FirstOrDefault(x => x.PersonId == personUpdateRequest.PersonId)?.ToPersonResponse();
    }
    public void DeletePerson(Guid? PersonId)
    {
        if (PersonId == null) throw new ArgumentNullException(nameof(PersonId));
        Person? person = PersonList.FirstOrDefault(x => x.PersonId == PersonId);
        if (person == null) throw new ArgumentException("Requested object does not exist");
        PersonList.Remove(person);
    }
}
