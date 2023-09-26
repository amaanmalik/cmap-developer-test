using System.ComponentModel.DataAnnotations;

namespace Timesheets.Models
{
    public class TimesheetEntry
    {
        [Key]
        public int Id { get; set; }

        public string Date { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Project { get; set; }

        public string Hours { get; set; }
    }
}
