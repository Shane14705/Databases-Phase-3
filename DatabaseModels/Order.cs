using System;
using System.Collections.Generic;

namespace Phase3Databases.DatabaseModels;

public partial class Order
{
    public int OrderId { get; set; }

    public decimal OrderTotal { get; set; }

    public DateTime OrderTimestamp { get; set; }

    public int OrderStatus { get; set; }

    public int? CourierId { get; set; }

    public DateTime? PickupTime { get; set; }

    public DateTime? DeliveryTime { get; set; }

    public float? HoursElapsed { get; set; }

    public float? DistanceRemaining { get; set; }

    public DateTime? EstimatedDeliveryTime { get; set; }

    public int CustomerId { get; set; }

    public virtual Courier? Courier { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<ItemsOrdered> ItemsOrdereds { get; } = new List<ItemsOrdered>();

    public virtual ICollection<PickList> PickLists { get; } = new List<PickList>();
}
