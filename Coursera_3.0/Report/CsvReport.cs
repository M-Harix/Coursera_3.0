using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Coursera_3._0.Dto;

namespace Coursera_3._0.Report
{
    public class CsvReport
    {
        public CsvReport(IEnumerable<IGrouping<dynamic, StudentReportDto>> students, string outputDirectory)
        {
            string filePath = Path.Combine(outputDirectory, "studentsreport.csv");
            using (var writer = new StreamWriter(filePath))
            using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                csv.WriteField("Student Name");
                csv.WriteField("Total Credit");
                csv.NextRecord();
                csv.WriteField("");
                csv.WriteField("Course Name");
                csv.WriteField("Total Time");
                csv.WriteField("Credit");
                csv.WriteField("Instructor Name");
                csv.NextRecord();

                foreach (var student in students)
                {
                    csv.WriteField($"{student.Key.StudentName}");
                    csv.WriteField(student.Key.TotalCredit);
                    csv.NextRecord();

                    foreach (var course in student)
                    {
                        csv.WriteField("");
                        csv.WriteField(course.CourseName);
                        csv.WriteField(course.TotalTime);
                        csv.WriteField(course.Credit);
                        csv.WriteField($"{course.InstructorName}");
                        csv.NextRecord();
                    }
                }
                Console.WriteLine($"CSV report created successfully at {Path.GetFullPath(filePath)}");
            }
        }
    }
}