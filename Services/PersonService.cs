using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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

        public IEnumerable<PersonResponse> GetAllPersons()
        {
           return PersonList.Select(x => x.ToPersonResponse());
        }

        public PersonResponse? GetPersonByPersonId(Guid? personId)
        {
            if(personId==null) throw new ArgumentNullException(nameof(personId));
            var person=PersonList.FirstOrDefault(x=>x.PersonId== personId)?.ToPersonResponse();
            return person;
        }
    }
}
