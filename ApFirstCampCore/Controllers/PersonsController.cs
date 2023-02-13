using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;

namespace ApFirstCampCore.Controllers;

[Route("[controller]")]
public class PersonsController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public PersonsController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [Route("[action]")]
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


    [Route("[action]")]
    [HttpGet]
    public IActionResult Create()
    {
        ViewBag.Countries = _unitOfWork.CountriesService.GetAll().Select(temp=>
            new SelectListItem() { Text=temp.CountryName,Value=temp.CountryId.ToString()}
        );



        return View();
    }

    [Route("[action]")]
    [HttpPost]
    public IActionResult Create(PersonAddRequest personAddRequest)
        {
        if(ModelState.IsValid)
        {
           PersonResponse? createdPerson= _unitOfWork.PersonService.AddPerson(personAddRequest);
        }
        else
        {
            ViewBag.Countries = _unitOfWork.CountriesService.GetAll();
            ViewBag.Errors=ModelState.Values.SelectMany(v=>v.Errors).Select(x=>x.ErrorMessage).ToList();
            return View();
        }

        return RedirectToAction("Index","Persons");
    }

    [HttpGet]
    [Route("[action]/{personId}")]
    public IActionResult Edit(Guid personId)
    {
        PersonResponse personResponse =_unitOfWork.PersonService.GetPersonByPersonId(personId);
        ViewBag.Countries = _unitOfWork.CountriesService.GetAll().Select(country => new SelectListItem() {
            Text=country.CountryName,Value=country.CountryId.ToString()
        });

        return View(personResponse.ToPersonUpdateRequest());
    }

    [HttpPost]
    [Route("[action]/{personId}")]
    public IActionResult Edit(PersonUpdateRequest personUpdateRequest)
    {
        if (ModelState.IsValid)
        {
            _unitOfWork.PersonService.UpdatePerson(personUpdateRequest);
        return RedirectToAction("Index", "Persons");
        }
        else
        {
            return View("Edit",personUpdateRequest);
        }

    }

    [HttpGet]
    [Route("[action]/{personId}")]
    public IActionResult Delete(Guid personId)
    {
        PersonResponse? personResponse = _unitOfWork.PersonService.GetPersonByPersonId(personId);
        if (personResponse == null)
        {
            return RedirectToAction("Index","Persons");
        }
        return View(personResponse);
    }

    [HttpPost]
    [Route("[action]/{personId}")]
    public IActionResult Delete(PersonUpdateRequest personUpdateRequest)
    {
        var person=_unitOfWork.PersonService.GetPersonByPersonId(personUpdateRequest.PersonId);
        if (person != null)
        {
         _unitOfWork.PersonService.DeletePerson(person.PersonId);
        }
        return RedirectToAction("Index", "Persons");
    }
}
