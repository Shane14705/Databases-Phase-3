using System;
using System.Collections.Generic;

namespace Phase3Databases.DatabaseModels;

public partial class PickWalk
{
    public PickWalk(DateTime startTimestamp, Employee employee)
    {
        StartTimestamp = startTimestamp;
        Employee = employee;
        this.TotalQuantityPicked = 0;
    }

    public DateTime StartTimestamp { get; set; }

    public int EmployeeId { get; set; }

    public int TotalQuantityPicked { get; set; }

    public DateTime? EndTimestamp { get; set; }

    public int? WalkDuration { get; set; }

    public float? PickRate { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual ICollection<PickList> PickLists { get; } = new List<PickList>();
}
