using System;
using System.Collections.Generic;

namespace Phase3Databases.DatabaseModels;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string? DeliveryLocation { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public DateOnly BirthDate { get; set; }

    public string PhoneNumber { get; set; } = null!;

    public int Age { get; set; }

    public virtual ICollection<Order> Orders { get; } = new List<Order>();
}
