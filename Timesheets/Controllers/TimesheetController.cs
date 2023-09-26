using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Timesheets.Models;
using Timesheets.Services;

namespace Timesheets.Controllers
{
    public class TimesheetController : Controller
    {
        private ITimesheetService _timesheetService;
        private ICsvExportService<ProjectTimesheet> _csvExportService;

        public TimesheetController(ITimesheetService timesheetService, ICsvExportService<ProjectTimesheet> csvExportService)
        {
            _timesheetService = timesheetService;
            _csvExportService = csvExportService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(TimesheetEntry timesheetEntry)
        {            
            var timesheet = new Timesheet()
            {
                TimesheetEntry = timesheetEntry,
                TotalHours = timesheetEntry.Hours
            };

            _timesheetService.Add(timesheet);

            var res = _timesheetService.GetTimeSheets();

            ViewData["TimeSheetsList"] = res;

            return View();
        }

        public IActionResult DownloadCsv()
        {          
            var timesheetsData = _timesheetService.GetTimeSheets();

            var csvBytes = _csvExportService.ExportToCsv((List<ProjectTimesheet>)timesheetsData);

            // Return the CSV file as a downloadable file
            return File(csvBytes, "text/csv", "timesheets.csv");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}