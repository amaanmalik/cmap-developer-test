using Timesheets.Infrastructure;
using Timesheets.Models;
using Timesheets.Repositories;

namespace Timesheets.Services
{
    public interface ITimesheetService
    {
        void Add(Timesheet timesheet);
        IList<Timesheet> GetAll();

        IList<ProjectTimesheet> GetTimeSheets();
    }

    public class TimesheetService : ITimesheetService
    {
        private readonly ITimesheetRepository _timesheetRepository;

        public TimesheetService(ITimesheetRepository timesheetRepository)
        {
            _timesheetRepository = timesheetRepository;
        }

        public void Add(Timesheet timesheet)
        {
            _timesheetRepository.AddTimesheet(timesheet);
        }

        public IList<Timesheet> GetAll()
        {
            var timesheets = _timesheetRepository.GetAllTimesheets();
            return timesheets;
        }

        public IList<ProjectTimesheet> GetTimeSheets()
        {
            return _timesheetRepository.GetTimeSheets();
        }
    }
}
