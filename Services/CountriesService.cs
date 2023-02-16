using CsvHelper;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services;

public class CountriesService : ICountriesService
{
    private readonly ApplicationDbContext _db;

    public CountriesService(ApplicationDbContext dbContext)
    {
        _db = dbContext;
    }

    public async Task<CountryResponse?> AddCountry(CountryAddRequest? countryAddRequest)
    {
        if (countryAddRequest == null)
        {
            throw new ArgumentNullException(nameof(countryAddRequest));
        }
        if(countryAddRequest.CountryName== null)
        {
            throw new ArgumentException(nameof(countryAddRequest.CountryName));
        }
       if(_db.Countries.Where(x=>x.CountryName== countryAddRequest.CountryName).Any())
        {
            throw new ArgumentException(nameof(countryAddRequest.CountryName));
        }

        Country country = countryAddRequest.ToCountry();
        
         Country savedCountry=(await _db.Countries.AddAsync(country)).Entity;
        await _db.SaveChangesAsync();
        return CountryExtensions.ToCountryResponse(savedCountry);


    }

    public async Task<List<CountryResponse>> GetAll()
    {
        List<Country>?countryList = await _db.Countries.ToListAsync();
        return countryList.Select(country=>country.ToCountryResponse()).ToList();
    }

    public async Task<CountryResponse?> GetCountryByCountryId(Guid? countryId)
    {
        Country? country=await _db.Countries.FirstOrDefaultAsync(country=>country.CountryId==countryId);
        return country.ToCountryResponse();
    }

    public async Task<int> UploadCountriesFromExcelFile(IFormFile formFile)
    {
        int numberOfInsertedRows = 0;
        MemoryStream memoryStream= new MemoryStream();
        await formFile.CopyToAsync(memoryStream);
        using(ExcelPackage package = new ExcelPackage(memoryStream))
        {
            ExcelWorksheet excelWorksheet = package.Workbook.Worksheets["Countries"];
            int rowCount = excelWorksheet.Dimension.Rows;
            for (int row = 2; row < rowCount; row++)
            {
                string? countryName=Convert.ToString(excelWorksheet.Cells[row, 1].Value);
                if(!string.IsNullOrEmpty(countryName))
                {
                    if(_db.Countries.Where(c=>c.CountryName.ToLower()==countryName.ToLower()).Count()==0)
                    {
                        Country country=new Country();
                        country.CountryName=countryName;
                        await _db.Countries.AddAsync(country);
                        await _db.SaveChangesAsync();
                        numberOfInsertedRows++;
                    }
                }
            }
        }
        return numberOfInsertedRows;
    }
}