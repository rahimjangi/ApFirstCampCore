using CsvHelper;
using CsvHelper.Configuration;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using OfficeOpenXml;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;


namespace Services;

public class PersonService : IPersonService
{
    private readonly ApplicationDbContext _db;

    public PersonService(ApplicationDbContext dbContext)
    {
        _db = dbContext;
    }

    private  PersonResponse? ConvertPersonToPersonResponse(Person person)
    {

        if (person == null) return null;
        var genderOptions = GenderOptions.Other;
        PersonResponse personResponse= new PersonResponse() {
            PersonId = person.PersonId,
            CountryId = person.CountryId,
            Country=person.Country?.CountryName,
            PersonName = person.PersonName,
            Email = person.Email,
            DateOfBirth = person.DateOfBirth,
            Age = (person.DateOfBirth != null) ? (int)Math.Round((DateTime.Now - person.DateOfBirth).GetValueOrDefault().TotalDays / 365.24) : 0,
            Address = person.Address,
            Gender = (Enum.TryParse<GenderOptions>(person.Gender,true,out genderOptions)?genderOptions:GenderOptions.Other),
            ReceiveNewsLetters = person.ReceiveNewsLetters
        };
        return personResponse;
    }

    public async Task<PersonResponse>? AddPerson(PersonAddRequest? personAddRequest)
    {
        if(personAddRequest == null)throw new ArgumentNullException(nameof(personAddRequest));

        ModelValidate.Validate(personAddRequest);
        Person? person = personAddRequest.ToPerson();
        var savedPerson= await _db.Persons.AddAsync(person);
        await _db.SaveChangesAsync();
        return  savedPerson.Entity.ToPersonResponse();

    }


    public async Task<List<PersonResponse>> GetAllPersons()
    {
        var result = await _db.Persons.Include(p=>p.Country).ToListAsync();
        return result.Select(person => person.ToPersonResponse()).ToList();
    }

    public async Task<List<PersonResponse?>> GetFilteredPersons(string searchBy, string? searchString)
    {


        List<PersonResponse> allPersons = await GetAllPersons();
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

    public async Task<PersonResponse?> GetPersonByPersonId(Guid? personId)
    {
        if(personId==null) throw new ArgumentNullException(nameof(personId));
        Person? person=_db.Persons.FirstOrDefault(x=>x.PersonId== personId);
        return ConvertPersonToPersonResponse(person);
    }

    public async Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder)
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

    public async Task<PersonResponse?> UpdatePerson(PersonUpdateRequest? personUpdateRequest)
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
    public async Task DeletePerson(Guid? PersonId)
    {
        if (PersonId == null) throw new ArgumentNullException(nameof(PersonId));
        Person? person = _db.Persons.FirstOrDefault(x => x.PersonId == PersonId);
        if (person == null) throw new ArgumentException("Requested object does not exist");
        _db.Remove(person);
        _db.SaveChanges();
    }

    public async Task<MemoryStream> GetPersonsCsv()
    {
        MemoryStream memoryStream= new MemoryStream();
        StreamWriter streamWriter= new StreamWriter(memoryStream);
        CsvConfiguration csvConfiguration = new CsvConfiguration(
            CultureInfo.InvariantCulture
            );


        CsvWriter csvWriter = new CsvWriter(streamWriter,csvConfiguration);

        //csvWriter.WriteField(nameof(PersonResponse.PersonName));
        //csvWriter.WriteField(nameof(PersonResponse.Email));
        //csvWriter.WriteField(nameof(PersonResponse.DateOfBirth));
        //csvWriter.WriteField(nameof(PersonResponse.ReceiveNewsLetters));
        //csvWriter.WriteField(nameof(PersonResponse.Country));
        //csvWriter.WriteField(nameof(PersonResponse.Address));
        //csvWriter.WriteField(nameof(PersonResponse.Age));

        csvWriter.WriteHeader<PersonResponse>();
        csvWriter.NextRecord();
        var PersonsList=(await _db.Persons.Include(c=>c.Country).ToListAsync()).Select(person=>person.ToPersonResponse()).ToList();

        //foreach (var item in PersonsList)
        //{
        //    csvWriter.WriteField(item.PersonName);
        //    csvWriter.WriteField(item.Email);
        //    if (item.DateOfBirth.HasValue)
        //    {
        //        csvWriter.WriteField(item.DateOfBirth.Value.ToString("yyyy-MM-dd"));
        //    }
        //    else
        //    {
        //        csvWriter.WriteField("");
        //    }
        //    csvWriter.WriteField(item.ReceiveNewsLetters);
        //    csvWriter.WriteField(item.Country);
        //    csvWriter.WriteField(item.Address);
        //    csvWriter.WriteField(item.Age);
        //    csvWriter.NextRecord();
        //}

        await csvWriter.WriteRecordsAsync(PersonsList);
        csvWriter.Flush();
        memoryStream.Position=0;
        
        return memoryStream;

    }

    public async Task<MemoryStream?> GetPersonsExcel()
    {
        MemoryStream memoryStream= new MemoryStream();  
        using(ExcelPackage excelPackage= new ExcelPackage(memoryStream))
        {
            ExcelWorksheet excelWorksheet=excelPackage.Workbook.Worksheets.Add("Sheet1");
            excelWorksheet.Cells["A1"].Value = nameof(PersonResponse.PersonName);
            excelWorksheet.Cells["B1"].Value = nameof(PersonResponse.Country);
            excelWorksheet.Cells["C1"].Value = nameof(PersonResponse.Age);
            excelWorksheet.Cells["D1"].Value = nameof(PersonResponse.Email);
            excelWorksheet.Cells["E1"].Value = nameof(PersonResponse.ReceiveNewsLetters);
            excelWorksheet.Cells["F1"].Value = nameof(PersonResponse.DateOfBirth);
            excelWorksheet.Cells["G1"].Value = nameof(PersonResponse.Gender);

            using (ExcelRange headerCells = excelWorksheet.Cells["A1:G1"])
            {
                headerCells.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                headerCells.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                headerCells.Style.Font.Bold=true;
            }

            int row = 2;
            List<PersonResponse> people =await GetAllPersons();
            foreach (var person in people)
            {

                excelWorksheet.Cells[row,1].Value = person.PersonName;
                excelWorksheet.Cells[row,2].Value = person.Country;
                excelWorksheet.Cells[row,3].Value = person.Age;
                excelWorksheet.Cells[row,4].Value = person.Email;
                excelWorksheet.Cells[row,5].Value = person.ReceiveNewsLetters;
                excelWorksheet.Cells[row,6].Value =(person.DateOfBirth.HasValue)?person.DateOfBirth.Value.ToString("yyyy-MM-dd"):null;
                excelWorksheet.Cells[row,7].Value = person.Gender;

                row++;
                
            }
            excelWorksheet.Cells[$"A1:G{row}"].AutoFitColumns();
            await excelPackage.SaveAsync();

        }
        memoryStream.Position = 0;
        return memoryStream;


    }
}
