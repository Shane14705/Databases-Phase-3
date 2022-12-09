using System;
using System.Collections.Generic;

namespace Phase3Databases.DatabaseModels;

public partial class PickList
{
    public PickList(int itemId, int quantityNeeded, int orderId)
    {
        this.ItemId = itemId;
        this.QuantityNeeded = quantityNeeded;
        this.OrderId = orderId;
    }
    public DateTime? StartTimestamp { get; set; }

    public int? EmployeeId { get; set; }

    public int ItemId { get; set; }

    public int QuantityNeeded { get; set; }

    public int OrderId { get; set; }

    public virtual Item Item { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;

    public virtual PickWalk? PickWalk { get; set; }
}
