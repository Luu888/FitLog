using AutoMapper;
using FitLog.Areas.Tracker.Services;
using FitLog.Areas.Tracker.ViewModels.Home;
using FitLog.Helpers;
using FitLog.Models.Enums.Errors;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.CodeAnalysis.CSharp.SyntaxTokenParser;

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

            if (file != null && file.Length > 0)
            {
                using var reader = new StreamReader(file.OpenReadStream());
                using var csv = new CsvHelper.CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture);

                csv.Context.RegisterClassMap<FitatuImportViewModelMap>();
                var importList = csv.GetRecords<ImportViewModel>().ToList();

                var result = await _service.ImportAsync(importList);

                return ToastHelper.ToJsonResult<FitatuImportError>(result);
            }
            else
            {
                return ToastHelper.ToJsonResult<FitatuImportError>((int)FitatuImportError.FileNotFound);
            }

        }

    }
}
