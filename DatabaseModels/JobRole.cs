using System;
using System.Collections.Generic;

namespace Phase3Databases.DatabaseModels;

public partial class JobRole
{
    public int EmployeeId { get; set; }

    public string RoleName { get; set; } = null!;

    public int DepartmentNumber { get; set; }

    public virtual Employee Employee { get; set; } = null!;
}
