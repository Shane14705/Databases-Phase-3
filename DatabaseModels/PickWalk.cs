using System;
using System.Collections.Generic;

namespace Phase3Databases.DatabaseModels;

public partial class PickWalk
{
    //For some reason we cannot pass in an actual employee :shrug:
    public PickWalk(DateTime startTimestamp, int employeeId)
    {
        StartTimestamp = startTimestamp;
        this.EmployeeId = employeeId;
        this.TotalQuantityPicked = 0;
    }

    public DateTime StartTimestamp { get; set; }

    public int EmployeeId { get; set; }

    public int TotalQuantityPicked { get; set; }

    public DateTime? EndTimestamp { get; set; }

    public float? WalkDuration { get; set; }

    public float? PickRate { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual ICollection<PickList> PickLists { get; } = new List<PickList>();
}
