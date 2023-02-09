using Entities;
using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO;

/// <summary>
/// Acts as DTO for inserting into Person class model
/// </summary>
public class PersonAddRequest
{
    [Required(ErrorMessage ="Can not be blank or null")]
    public string PersonName { get; set; }
    [Required(ErrorMessage = "Can not be blank or null")]
    public string? Email { get; set; }
    [Required(ErrorMessage = "Can not be blank or null")]
    public DateTime? DateOfBirth { get; set; }
    [Required(ErrorMessage = "Can not be blank or null")]
    public GenderOptions? Gender { get; set; }
    [Required(ErrorMessage = "Can not be blank or null")]
    public Guid? CountryId { get; set; }
    [Required(ErrorMessage = "Can not be blank or null")]
    public string? Address { get; set; }
    [Required(ErrorMessage = "Can not be blank or null")]
    public bool ReceiveNewsLetters { get; set; }

    /// <summary>
    /// Converts the current type of PersonRequest to Person model
    /// </summary>
    /// <returns></returns>
    public Person ToPerson()
    {
        return new Person
        {
            PersonName = PersonName,
            Email = Email,
            DateOfBirth = DateOfBirth,
            Gender = Gender?.ToString(),
            CountryId = CountryId,
            Address = Address,
            ReceiveNewsLetters = ReceiveNewsLetters
        };
    }
}
