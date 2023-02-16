using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using Services;

namespace ApFirstCampCore.Controllers
{
    [Route("[controller]")]
    public class CountriesController : Controller
    {
        private readonly IUnitOfWork _db;

        public CountriesController(IUnitOfWork db)
        {
            _db = db;
        }

        [Route("[action]")]
        public IActionResult UploadFromExcel()
        {
            return View();
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> UploadFromExcel(IFormFile excelFile)
        {
            if (excelFile == null || excelFile.Length == 0)
            {
                ViewBag.ErrorMessage = "Please select an xlsx file";
                return View();
            }

            if (!Path.GetExtension(excelFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                ViewBag.ErrorMessage = "Unsupported file. 'xlsx' file is expected";
                return View();
            }

            int countriesCountInserted = await _db.CountriesService.UploadCountriesFromExcelFile(excelFile);

            ViewBag.Message = $"{countriesCountInserted} Countries Uploaded";
            return View();
        }
    }
}
