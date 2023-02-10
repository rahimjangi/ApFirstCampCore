using Entities;
using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO;


/// <summary>
/// Acts as DTO for Person model response
/// </summary>
public class PersonResponse
{
    public Guid PersonId { get; set; }
    public string? PersonName { get; set; }
    public string? Email { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public int? Age { get; set; }
    public GenderOptions? Gender { get; set; }
    public Guid? CountryId { get; set; }
    public string? Country { get; set; }
    public string? Address { get; set; }
    public bool ReceiveNewsLetters { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != typeof(PersonResponse)) return false;
        PersonResponse otherPerson= obj as PersonResponse;
        return PersonId == otherPerson.PersonId &&
            PersonName==otherPerson.PersonName ;
    }

    public PersonUpdateRequest ToPersonUpdateRequest()
    {
        return new PersonUpdateRequest() { 
            PersonId= PersonId,
            PersonName=PersonName,
            Email=Email,
            DateOfBirth=DateOfBirth,
            Address=Address,
            CountryId=CountryId,
            Gender=Gender,
            ReceiveNewsLetters=ReceiveNewsLetters,
        };
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string? ToString()
    {
        return $"PersonResponse: {PersonId} - {PersonName} - {Email} - {DateOfBirth} - {Age} - {Gender} - {Country} - {Address} - {ReceiveNewsLetters}";
    }
}

public static class PersonExtensions
{
    public static PersonResponse ToPersonResponse(this Person person) => new PersonResponse
    {
        PersonId = person.PersonId,
        CountryId = person.CountryId,
        PersonName = person.PersonName,
        Email = person.Email,
        DateOfBirth = person.DateOfBirth,
        Age = (person.DateOfBirth!=null)?(int)Math.Round((DateTime.Now - person.DateOfBirth).GetValueOrDefault().TotalDays / 365.24):0,
        Address = person.Address,
        Gender=Enum.Parse<GenderOptions>(person.Gender)

    };
}
