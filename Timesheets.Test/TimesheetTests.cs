using Castle.Core.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using Timesheets.Infrastructure;
using Timesheets.Models;
using Timesheets.Repositories;
using Timesheets.Services;

namespace Timesheets.Test
{
    public class TimesheetTests
    {
        private readonly TimesheetService _timesheetService;
        private readonly Mock<ITimesheetRepository> _mockRepository;

        public TimesheetTests()
        {
            _mockRepository = new Mock<ITimesheetRepository>();
            _timesheetService = new TimesheetService(_mockRepository.Object);
        }

        [Fact]
        public void GivenAValidTimesheet_ThenAddTimesheetToInMemoryDatabase()
        {
            //Arrange
            var timesheet = new Timesheet();
            var timesheetEntry = new TimesheetEntry()
            {
                Id = 1,
                Date = "01/09/2023",
                Project = "Test Project",
                FirstName = "Test",
                LastName = "Test",
                Hours = "7.5"
            };
            timesheet.Id = 1;
            timesheet.TimesheetEntry = timesheetEntry;
            timesheet.TotalHours = timesheetEntry.Hours;

            // Act
            _timesheetService.Add(timesheet);

            // Assert
            _mockRepository.Verify(repo => repo.AddTimesheet(It.IsAny<Timesheet>()), Times.Once);
        }

        [Fact]
        public void GetTimeSheets_ReturnsAllTimeSheetsFromDatabase()
        {
            // Arrange
            var timesheetsList = Enumerable.Range(1, 10).Select(i => new Timesheet
            {
                Id = i,
                TimesheetEntry = new TimesheetEntry
                {
                    Project = $"Project {i}",
                    FirstName = "John",
                    LastName = "Doe",
                    Hours = "10.5"
                },

            }).ToList();

            foreach (var timesheet in timesheetsList)
            {
                _timesheetService.Add(timesheet);
            }

            _mockRepository.Setup(repo => repo.GetAllTimesheets()).Returns(timesheetsList);

            // Act
            var result = _timesheetService.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(10, result.Count);
        }

        [Fact]
        public void GetTimeSheets_ReturnsTimeSheetsSortedByProjectHoursDescending()
        {
            //Arrange
            var timesheetsList = new List<Timesheet>
            {
                new Timesheet
                {
                    Id = 1,
                    TimesheetEntry = new TimesheetEntry
                    {
                        Project = "Project A",
                        FirstName = "John",
                        LastName = "Doe",
                        Hours = "5.0"
                    },
                },
                new Timesheet
                {
                    Id = 2,
                    TimesheetEntry = new TimesheetEntry
                    {
                        Project = "Project A",
                        FirstName = "John",
                        LastName = "Doe",
                        Hours = "5.0"
                    },
                },
                new Timesheet
                {
                    Id = 3,
                    TimesheetEntry = new TimesheetEntry
                    {
                        Project = "Project B",
                        FirstName = "John",
                        LastName = "Doe",
                        Hours = "5.0"
                    },
                }
            };

            var timesheetsListOrdered = new List<TimesheetsList>
            {
                new TimesheetsList
                {
                    Project = "Project A",
                    FirstName = "John",
                    LastName = "Doe",
                    ProjectTotalHours = 10,
                    TotalHours = 10                    
                },
                 new TimesheetsList
                {
                    Project = "Project B",
                    FirstName = "John",
                    LastName = "Doe",
                    ProjectTotalHours = 10,
                    TotalHours = 5
                }
                
            };

            // Configure the repository to return the timesheetsList
            _mockRepository.Setup(repo => repo.GetTimeSheets()).Returns(timesheetsListOrdered);

            // Act
            var result = _timesheetService.GetTimeSheets();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count); // Assuming there are 3 unique projects in the example data

            // Verify that timesheets are sorted by project hours in descending order
            var projectHoursOrder = result.Select(t => t.ProjectTotalHours).ToList();
            var sortedProjectHoursOrder = projectHoursOrder.OrderByDescending(h => h).ToList();
            Assert.Equal(sortedProjectHoursOrder, projectHoursOrder);
        }
    }
}
