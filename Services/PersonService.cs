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


namespace Services
{
    public class PersonService : IPersonService
    {
        private readonly List<Person> PersonList;
        private readonly ICountriesService _countryService;

        public PersonService()
        {
            _countryService = new CountriesService();
            PersonList = new List<Person>();
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
                        matchingPersons = allPersons.OrderBy(x => x.PersonName).ToList();
                    }
                    else
                    {
                        matchingPersons = allPersons.OrderByDescending(x => x.PersonName).ToList();
                    }
                    break;


                case nameof(Person.DateOfBirth):
                    if (sortOrder.Equals(SortOrderOptions.ASC))
                    {
                        matchingPersons = allPersons.OrderBy(x => x.PersonName).ToList();
                    }
                    else
                    {
                        matchingPersons = allPersons.OrderByDescending(x => x.PersonName).ToList();
                    }
                    break;

                case nameof(Person.Gender):
                    if (sortOrder.Equals(SortOrderOptions.ASC))
                    {
                        matchingPersons = allPersons.OrderBy(x => x.PersonName).ToList();
                    }
                    else
                    {
                        matchingPersons = allPersons.OrderByDescending(x => x.PersonName).ToList();
                    }
                    break;

                case nameof(Person.CountryId):
                    if (sortOrder.Equals(SortOrderOptions.ASC))
                    {
                        matchingPersons = allPersons.OrderBy(x => x.PersonName).ToList();
                    }
                    else
                    {
                        matchingPersons = allPersons.OrderByDescending(x => x.PersonName).ToList();
                    }
                    break;

                case nameof(Person.Address):
                    if (sortOrder.Equals(SortOrderOptions.ASC))
                    {
                        matchingPersons = allPersons.OrderBy(x => x.PersonName).ToList();
                    }
                    else
                    {
                        matchingPersons = allPersons.OrderByDescending(x => x.PersonName).ToList();
                    }
                    break;

                default: matchingPersons = allPersons; break;
            }
            return matchingPersons;
        }
    }
}
