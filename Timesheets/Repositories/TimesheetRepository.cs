using Microsoft.EntityFrameworkCore;
using Timesheets.Infrastructure;
using Timesheets.Models;

namespace Timesheets.Repositories
{
    public interface ITimesheetRepository
    {
        void AddTimesheet(Timesheet timesheet);
        IList<Timesheet> GetAllTimesheets();

        IList<TimesheetsList> GetTimeSheets();
    }

    public class TimesheetRepository : ITimesheetRepository
    {
        private DataContext _context;

        public TimesheetRepository(DataContext context)
        {
            _context = context;
        }
        public void AddTimesheet(Timesheet timesheet)
        {
            _context.Timesheets.Add(timesheet);
            _context.SaveChanges();
        }

        public IList<Timesheet> GetAllTimesheets()
        {
            var timesheets = _context.Timesheets.Include(t=>t.TimesheetEntry).ToList();
            return timesheets;
        }

        public IList<TimesheetsList> GetTimeSheets()
        {

            var timesheetData = _context.Timesheets
                .Include(t => t.TimesheetEntry)
                .ToList();

            var projectTotalHoursDictionary = timesheetData
                .GroupBy(te => te.TimesheetEntry.Project)
                .ToDictionary(
                    group => group.Key,
                    group => group.Sum(te => Convert.ToDouble(te.TimesheetEntry.Hours))
                );

            return _context.Timesheets
                .Include(t => t.TimesheetEntry)
                .GroupBy(te => new
                {
                    te.TimesheetEntry.Project,
                    te.TimesheetEntry.FirstName,
                    te.TimesheetEntry.LastName
                })
                .Select(group => new TimesheetsList
                {
                    Project = group.Key.Project,
                    ProjectTotalHours = projectTotalHoursDictionary[group.Key.Project],
                    FirstName = group.Key.FirstName,
                    LastName = group.Key.LastName,
                    TotalHours = group.Sum(te => Convert.ToDouble(te.TimesheetEntry.Hours))
                })
                .OrderByDescending(te => te.ProjectTotalHours) // Sort by project total hours
                .ThenByDescending(te => te.TotalHours) // Then sort by employee total hours
                .ToList();
        }
    }
}
