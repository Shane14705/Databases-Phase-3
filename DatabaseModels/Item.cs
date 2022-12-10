using System;
using System.Collections.Generic;

namespace Phase3Databases.DatabaseModels;

public partial class Item
{
    public int ItemId { get; set; }

    public int? AgeRequirement { get; set; }

    public decimal Price { get; set; }

    public int QuantityAvailable { get; set; }

    public int DepartmentNumber { get; set; }

    public int Aisle { get; set; }

    public int ShelfLocation { get; set; }

    public string ItemName { get; set; } = null!;

    public virtual ICollection<ItemsOrdered> ItemsOrdereds { get; } = new List<ItemsOrdered>();

    public virtual ICollection<PickList> PickLists { get; } = new List<PickList>();

    public override string ToString()
    {
        return this.ItemName + "\t|\t$" + this.Price + "\t|\tId: " + this.ItemId + "\t|\tQuantity on-hand: " + this.QuantityAvailable;
    }
}
