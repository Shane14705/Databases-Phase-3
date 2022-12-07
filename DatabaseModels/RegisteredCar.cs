using System;
using System.Collections.Generic;

namespace Phase3Databases.DatabaseModels;

public partial class RegisteredCar
{
    public int CourierId { get; set; }

    public string LicensePlateNumber { get; set; } = null!;

    public string? Model { get; set; }

    public string? Color { get; set; }

    public virtual Courier Courier { get; set; } = null!;
}
