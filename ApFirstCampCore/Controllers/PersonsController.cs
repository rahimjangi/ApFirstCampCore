﻿using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rotativa.AspNetCore;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;
using System.IO;

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
    public async Task<IActionResult> Index(string searchBy, string? searchString, string sortBy = nameof(PersonResponse.PersonName), SortOrderOptions sortOrder = SortOrderOptions.ASC)
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
        List<PersonResponse> persons = await _unitOfWork.PersonService.GetAllPersons();

        if (searchBy != null && searchString != null)
        {
            persons = await _unitOfWork.PersonService.GetFilteredPersons(searchBy, searchString);
            
        }


        List<PersonResponse> sortedPersons = await _unitOfWork.PersonService.GetSortedPersons(persons, sortBy, sortOrder);
        ViewBag.CurrentSortBy = sortBy;
        ViewBag.CurrentSortOrder = sortOrder.ToString();

        ViewBag.CurrentSearchBy = searchBy;
        ViewBag.CurrentSearchString = searchString;
        return View(sortedPersons);
    }


    [Route("[action]")]
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var countryList = await _unitOfWork.CountriesService.GetAll();
        ViewBag.Countries = countryList.ToList().Select(temp=>
            new SelectListItem() { Text=temp.CountryName,Value=temp.CountryId.ToString()}
        );



        return View();
    }

    [Route("[action]")]
    [HttpPost]
    public async Task<IActionResult> Create(PersonAddRequest personAddRequest)
        {
        if(ModelState.IsValid)
        {
           PersonResponse? createdPerson= await _unitOfWork.PersonService.AddPerson(personAddRequest);
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
    public async Task<IActionResult> Edit(Guid personId)
    {
        PersonResponse? personResponse =await _unitOfWork.PersonService.GetPersonByPersonId(personId);
        ViewBag.Countries = (await _unitOfWork.CountriesService.GetAll()).Select(country => new SelectListItem() {
            Text=country.CountryName,Value=country.CountryId.ToString()
        });

        return View(personResponse.ToPersonUpdateRequest());
    }

    [HttpPost]
    [Route("[action]/{personId}")]
    public async Task<IActionResult> Edit(PersonUpdateRequest personUpdateRequest)
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
    public async Task<IActionResult> Delete(Guid personId)
    {
        PersonResponse? personResponse = await _unitOfWork.PersonService.GetPersonByPersonId(personId);
        if (personResponse == null)
        {
            return RedirectToAction("Index","Persons");
        }
        return View(personResponse);
    }

    [HttpPost]
    [Route("[action]/{personId}")]
    public async Task<IActionResult> Delete(PersonUpdateRequest personUpdateRequest)
    {
        var person=await _unitOfWork.PersonService.GetPersonByPersonId(personUpdateRequest.PersonId);
        if (person != null)
        {
         await _unitOfWork.PersonService.DeletePerson(person.PersonId);
        }
        return RedirectToAction("Index", "Persons");
    }

    [HttpGet]
    [Route("[action]")]
    public async Task<IActionResult> PersonPDF()
    {
        List<PersonResponse> personResponses = await _unitOfWork.PersonService.GetAllPersons();
        return new ViewAsPdf("PersonsPDF", personResponses, ViewData)
        {
            PageMargins = new Rotativa.AspNetCore.Options.Margins() { Top = 20, Right = 20, Bottom = 20, Left = 20 },
            PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
        };
    }

    [Route("[action]")]
    public async Task<IActionResult> PersonsCsv()
    {
        MemoryStream ms= await _unitOfWork.PersonService.GetPersonsCsv();

        return File(ms,"application/octed-stream","persons.csv");
    }
        
}
