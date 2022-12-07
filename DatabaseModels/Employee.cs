using System;
using System.Collections.Generic;

namespace Phase3Databases.DatabaseModels;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public float CumulativePickrate { get; set; }

    public decimal Salary { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public virtual ICollection<JobRole> JobRoles { get; } = new List<JobRole>();

    public virtual ICollection<PickWalk> PickWalks { get; } = new List<PickWalk>();
}
