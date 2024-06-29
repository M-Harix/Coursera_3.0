using Coursera_3._0.Data;
using Coursera_3._0.Dto;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Globalization;


namespace Coursera_3._0.Report
{
    public class StudentReport
    {
        public static void TakeInputs()
        {
            Console.WriteLine("Enter the Comma separated list of personal identifiers (PIN) of the students to be included in the report OR press Enter to select all students:");
            string std_pin_Input = Console.ReadLine();
            string[] std_pin_Input_List = string.IsNullOrWhiteSpace(std_pin_Input) ? new string[0] : std_pin_Input.Split(',');

        credit:
            Console.WriteLine("Enter required minimum credit:");
            string minimumCredit = Console.ReadLine();
            if (!byte.TryParse(minimumCredit, out byte minimum_Credit))
            {
                Console.WriteLine("Credit is required and must be a number.");
                goto credit;
            }

        startdate:
            Console.WriteLine("Enter the start date of the time period for which the students should have collected the requested credit (yyyy-mm-dd):");
            string startdate = Console.ReadLine();
            if (!DateTime.TryParseExact(startdate, "yyyy-mm-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime start_date))
            {
                Console.WriteLine("Invalid date format. Please enter the date in the correct format (yyyy-mm-dd).");
                goto startdate;
            }

        enddate:
            Console.WriteLine("Enter the end date of the time period for which the students should have collected the requested credit (yyyy-mm-dd):");
            string enddate = Console.ReadLine();
            if (!DateTime.TryParseExact(enddate, "yyyy-mm-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime end_date))
            {
                Console.WriteLine("Invalid date format. Please enter the date in the correct format (yyyy-mm-dd).");
                goto enddate;
            }

            Console.WriteLine("Enter the output format (csv or html) OR press Enter for both:");
            string format = Console.ReadLine();

        path:
            Console.WriteLine("Enter the path to save result (e.g., D://New Folder//):");
            string path = Console.ReadLine();
            if (string.IsNullOrEmpty(path))
            {
                Console.WriteLine("Path is required.");
                goto path;
            }
            Console.WriteLine();

            GetStudentsReport(std_pin_Input_List, minimum_Credit, start_date, end_date, path, format);
        }

        public static void GetStudentsReport(string[] studentPin, byte credit, DateTime startingDate, DateTime endingDate, string path, string format)
        {
            var dbContext = new CourseraContext();
            try
            {
                var minimumCreditParam = new SqlParameter("@MinimumCredit", SqlDbType.TinyInt) { Value = (object)DBNull.Value };
                var startingDateParam = new SqlParameter("@StartingDate", SqlDbType.DateTime) { Value = (object)DBNull.Value };
                var endingDateParam = new SqlParameter("@EndingDate", SqlDbType.DateTime) { Value = (object)DBNull.Value };
                // Calling Store Procedure
                var studentReports = dbContext.Set<StudentReportDto>().FromSqlRaw("EXEC [dbo].[StudentReport] @MinimumCredit, @StartingDate, @EndingDate",
                        minimumCreditParam, startingDateParam, endingDateParam).ToList();

                if (studentReports != null && studentReports.Any())
                {
                    var filteredReports = studentReports
                                            .Where(report => studentPin == null || !studentPin.Any() || studentPin.Contains(report.PIN))
                                            .ToList();

                    var groupedData = filteredReports.GroupBy(sc => new { sc.PIN, sc.StudentName, sc.TotalCredit });

                    if (groupedData != null && groupedData.Count() != 0)
                    {
                        if (format == "csv")
                        {
                            var report = new CsvReport(groupedData, path);
                        }
                        else if (format == "html")
                        {
                            var report = new HtmlReport(groupedData, path);
                        }
                        else
                        {
                            var htmlReport = new HtmlReport(groupedData, path);
                            var csvReport = new CsvReport(groupedData, path);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Student not found..");
                    }
                }
                else
                {
                    Console.WriteLine("Student not found..");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while querying students: {ex.Message}");
            }
        }
    }
}