using AutoMapper;
using FitLog.Areas.Tracker.Services;
using FitLog.Areas.Tracker.ViewModels.Home;
using FitLog.Helpers;
using FitLog.Models.DatabaseEntities;
using FitLog.Models.Enums.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitLog.Areas.Tracker.Controllers
{
    [Authorize]
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
        public async Task<IActionResult> Table()
        {
            var entities = await _service.GetAllAsync();
            var viewModel = _mapper.Map<List<IndexViewModel>>(entities);

            return PartialView("_List", viewModel);
        }

        public async Task<IActionResult> Edit(int id) 
        {
            var entity = await _service.GetAsync(id);
            var viewModel = _mapper.Map<EditViewModel>(entity);

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditViewModel viewModel)
        {
            if(viewModel == null)
                return BadRequest();


            var entity = _mapper.Map<DailyEntry>(viewModel);
            //var viewModel = _mapper.Map<EditViewModel>(entity);

            int result = await _service.UpdateAsync(id, entity);

            return RedirectToAction(nameof(Edit), new { id });
        }

        [HttpGet]
        public async Task<IActionResult> Day(DateTime? day = null)
        {
            day = day ?? DateTime.Now;

            var entity = await _service.GetSelectedDayAsync(day.Value);
            var viewModel = _mapper.Map<DaySummaryViewModel>(entity);

            return View(viewModel);
        }

        #region - IMPORT -

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

        #endregion

    }
}
