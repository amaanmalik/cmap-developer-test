using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Timesheets.Models;
using Timesheets.Services;

namespace Timesheets.Controllers
{
    public class TimesheetController : Controller
    {
        private ITimesheetService _timesheetService;

        public TimesheetController(ITimesheetService timesheetService)
        {
            _timesheetService = timesheetService;
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

            var timesheets = _timesheetService.GetAll();
            var res = _timesheetService.GetTimeSheets();

            ViewData["TimeSheetsList"] = res;

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}