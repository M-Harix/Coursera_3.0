using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursera_3._0.Dto
{
    [Keyless]
    public class StudentReportDto
    {
        public string PIN { get; set; }
        public string StudentName { get; set; }
        public int TotalCredit { get; set; }
        public string CourseName { get; set; }
        public byte TotalTime { get; set; }
        public byte Credit { get; set; }
        public string InstructorName { get; set; }
    }
}
