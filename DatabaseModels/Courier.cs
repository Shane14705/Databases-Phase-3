using System;
using System.Collections.Generic;

namespace Phase3Databases.DatabaseModels;

public partial class Courier
{
    public int CourierId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string? CurrentLocation { get; set; }

    public bool Available { get; set; }

    public virtual ICollection<Order> Orders { get; } = new List<Order>();

    public virtual ICollection<RegisteredCar> RegisteredCars { get; } = new List<RegisteredCar>();
}
