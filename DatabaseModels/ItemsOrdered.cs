using System;
using System.Collections.Generic;

namespace Phase3Databases.DatabaseModels;

public partial class ItemsOrdered
{
    public int OrderId { get; set; }

    public int ItemId { get; set; }

    public int QuantityRequested { get; set; }

    public virtual Item Item { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
