using AutoMapper;
using FitLog.Areas.Tracker.Services;
using FitLog.Areas.Tracker.ViewModels.Home;
using FitLog.Helpers;
using FitLog.Models.Enums.Errors;
using Microsoft.AspNetCore.Mvc;

namespace FitLog.Areas.Tracker.Controllers
{
    [Area(nameof(Tracker))]
    public class HomeController : Controller
    {
        private readonly ITrackerService _service;
        private readonly IMapper _mapper;

        public HomeController(ITrackerService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var entities = await _service.GetAllAsync();

            var viewModel = _mapper.Map<List<IndexViewModel>>(entities);

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult ImportModal()
        {
            var model = new ImportViewModel();
            return PartialView("_ImportModal", model);
        }

        [HttpPost]
        public async Task<IActionResult> Import(IFormFile file)
        {
            int result;

            if (file != null && file.Length > 0)
            {
                using var reader = new StreamReader(file.OpenReadStream());
                using var csv = new CsvHelper.CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture);

                csv.Context.RegisterClassMap<FitatuImportViewModelMap>();
                var importList = csv.GetRecords<ImportViewModel>().ToList();

                result = await _service.ImportAsync(importList);
            }
            else
            {
                result = (int)FitatuImportError.FileNotFound;
            }

            string message;
            string type;

            switch (result)
            {
                case -2:
                    message = "Import failed: entries for some dates already exist!";
                    type = "danger";
                    break;
                case -1:
                    message = "No file was uploaded.";
                    type = "warning";
                    break;
                case 0:
                    message = "No entries were imported.";
                    type = "info";
                    break;
                default:
                    message = "Import successful!";
                    type = "success";
                    break;
            }

            return Json(new { success = result > 0, message, type });
        }

    }
}
