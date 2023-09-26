using CsvHelper.Configuration;
using CsvHelper;
using Moq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timesheets.Services;
using Microsoft.AspNetCore.Mvc;
using Timesheets.Models;
using Timesheets.Controllers;

namespace Timesheets.Test
{
    public class CsvExportServiceTests
    {
        [Fact]
        public void DownloadCsv_ReturnsFileResultWithCsvContentType()
        {
            // Arrange
            var sampleCsv = "Project,ProjectTotalHours,FirstName,LastName,TotalHours\r\n" +
                                   "Project1,10.0,John,Doe,20.0\r\n" +
                                   "Project2,15.0,Jane,Smith,25.0\r\n";

            var sampleCsvContentBytes = Encoding.UTF8.GetBytes(sampleCsv);

            var timesheetsData = new List<ProjectTimesheet>
            {                
                new ProjectTimesheet { Project = "Project1", ProjectTotalHours = 10.0, FirstName = "John", LastName = "Doe", TotalHours = 20.0 },
                new ProjectTimesheet { Project = "Project2", ProjectTotalHours = 15.0, FirstName = "Jane", LastName = "Smith", TotalHours = 25.0 }                
            };

            var csvExportServiceMock = new Mock<ICsvExportService<ProjectTimesheet>>();
            csvExportServiceMock.Setup(mock => mock.ExportToCsv(It.IsAny<List<ProjectTimesheet>>()))
                .Returns(sampleCsvContentBytes);

            var timesheetServiceMock = new Mock<ITimesheetService>();
            timesheetServiceMock.Setup(mock => mock.GetTimeSheets()).Returns(timesheetsData);

            var controller = new TimesheetController(timesheetServiceMock.Object, csvExportServiceMock.Object);

            // Act
            var result = controller.DownloadCsv() as FileResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("text/csv", result.ContentType);
            Assert.Equal("timesheets.csv", result.FileDownloadName);
        }
    }
}
