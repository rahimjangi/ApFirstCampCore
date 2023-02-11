using Entities;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;

namespace ApFirstCampCore.Controllers;

public class PersonsController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public PersonsController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [Route("persons/index")]
    [Route("/")]
    public IActionResult Index(string? searchField, string? searchString)
    {
        if(searchField == null || searchString == null)
        ViewBag.SearchFields = new Dictionary<string, string>()
        {
            { nameof(Person.PersonName),"Person Name" },
            { nameof(Person.Email),"Email" },
            { nameof(Person.Gender),"Gender" },
            { nameof(Person.DateOfBirth),"Date Of Birth" },
            { nameof(Person.Address),"Address" }
        };
        List<PersonResponse> persons = _unitOfWork.PersonService.GetAllPersons();
        foreach (var item in persons)
        {
            item.Country = _unitOfWork.CountriesService.GetCountryByCountryId(item.CountryId)?.CountryName;
        }
        return View(persons);
    }
}
