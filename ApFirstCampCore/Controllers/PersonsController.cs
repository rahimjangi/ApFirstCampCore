using Entities;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
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
    public IActionResult Index(string searchBy, string? searchString, string sortBy = nameof(PersonResponse.PersonName), SortOrderOptions sortOrder = SortOrderOptions.ASC)
    {

        ViewBag.SearchFields = new Dictionary<string, string>()
        {
            { nameof(Person.PersonName),"Person Name" },
            { nameof(Person.Email),"Email" },
            { nameof(Person.Gender),"Gender" },
            { nameof(Person.DateOfBirth),"Date Of Birth" },
            { nameof(Person.Address),"Address" },
            {"Country","Country" }
        };
        List<PersonResponse> persons = _unitOfWork.PersonService.GetAllPersons();

        if (searchBy != null && searchString != null)
        {
            //if (searchBy == "Country")
            //{
            //    var ids=_unitOfWork.CountriesService.GetAll()?
            //        .Where(x=>x.CountryName.Contains(searchString,StringComparison.OrdinalIgnoreCase))?.Select(x=>x.CountryId).ToList();
            //    persons = persons.Where(x => ids.Any(srch => x.CountryId == srch)).ToList();
            //}else
            persons = _unitOfWork.PersonService.GetFilteredPersons(searchBy, searchString);
        }
        foreach (var item in persons)
        {
            item.Country = _unitOfWork.CountriesService.GetCountryByCountryId(item.CountryId)?.CountryName;
        }

        List<PersonResponse> sortedPersons = _unitOfWork.PersonService.GetSortedPersons(persons, sortBy, sortOrder);
        ViewBag.CurrentSortBy = sortBy;
        ViewBag.CurrentSortOrder = sortOrder.ToString();

        ViewBag.CurrentSearchBy = searchBy;
        ViewBag.CurrentSearchString = searchString;
        return View(sortedPersons);
    }
}
