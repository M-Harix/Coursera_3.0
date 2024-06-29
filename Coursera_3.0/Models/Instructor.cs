using System;
using System.Collections.Generic;

namespace Coursera_3._0.Models;

public partial class Instructor
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public DateTime TimeCreated { get; set; }

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
}
