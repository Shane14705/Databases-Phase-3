using Phase3Databases.DatabaseModels;

namespace Phase3Databases;

public class CustomerClient : Client
{
    //private Phase3Context Phase3DB;
    private Customer user;
    private Random rand = new Random();
    
    public CustomerClient(string connstring) : base(connstring)
    {
        
        //Queries here? https://learn.microsoft.com/en-us/ef/core/querying/
        try
        {
            using (Phase3Context db = new Phase3Context(this.connstring))
            {
                user = db.Customers.Single(e => (e.CustomerId == uid));
            }
        }
        catch (Exception e)
        {
            //We either got more than one user for id (not possible) or got 0 results (id doesn't exist)
            throw new UserNotFoundException("Couldn't find user with id " + this.uid);
        }
        Console.WriteLine(user.FirstName);
    }

    public override void MainLoop()
    {
        int option = -1;
        while (option != 4)
        {
            Console.WriteLine("What would you like to do?\n");
            option = Client.OptionsMenu("Search for an item", "Place an order", "Pick up available orders", "Log-out");
            switch (option)
            {
                case 1:
                    //Search for an item
                    Console.WriteLine("What would you like to search for?");
                    List<Item> results = this.itemSearch(Console.ReadLine());
                    if (results.Count == 0)
                    {
                        Console.WriteLine("No results :(");
                        break;
                    }
                    else
                    {
                        foreach (Item i in results)
                        {
                            Console.WriteLine(i);
                        }
                    }
                    break;
                case 2:
                    PlaceOrder();
                    break;
                case 3:
                    PickupOrders();
                    break;
            }
        }
    }

    //if user has any outstanding orders, their status is updated and it will return true
    private bool PickupOrders()
    {
        using (Phase3Context db = new Phase3Context(this.connstring))
        {
            List<Order> pickupOrders = db.Orders
                .Where(order => ((order.CustomerId == uid) && (order.OrderStatus == 2))).ToList();

            if (!pickupOrders.Any())
            {
                Console.WriteLine("Looks like you do not have any orders available for pickup right now!");
                return false;
            }
            db.UpdateRange(pickupOrders);

            foreach (Order order in pickupOrders)
            {
                order.OrderStatus = 4;
                order.PickupTime = DateTime.Now;
                
                Console.WriteLine("You picked up order #" + order.OrderId + " with " + db.ItemsOrdereds.Count(io => (io.OrderId == order.OrderId)) + " items!");
            }

            db.SaveChanges();
            return true;
        }

        return false;
    }
    //Returns boolean showing whether order was successfully placed or not
    private bool PlaceOrder()
    {
        List<ValueTuple<int, int>> shopList = new List<ValueTuple<int, int>>();
        decimal orderTotal = 0;
        do
        {
            ValueTuple<int, int> entry;
            Console.WriteLine(
                "Please enter the id and quantity of the item you would like to add to your order, separated by a comma: (enter a negative item id when you are done)");
            try
            {
                
                string[] s = Console.ReadLine().Split(',');
                if (s[0].Contains('-'))
                {
                    break;
                } 
                entry = (int.Parse(s[0]), int.Parse(s[1]));
            }
            catch (Exception e)
            {
                Console.WriteLine("Your input was formatted incorrectly. Please try again.");
                continue;
            }

            if (entry.Item1 < 0)
            {
                break;
            }
            else if (entry.Item2 <= 0)
            {
                continue;
            }
            else
            {
                try
                {
                    ValueTuple<int, int> currenttup = shopList.Single(tup => (tup.Item1 == entry.Item1));
                    shopList.Remove(currenttup);
                    currenttup.Item2 += entry.Item2;
                    shopList.Add(currenttup);
                }

                catch (InvalidOperationException e)
                {
                    //Item not already in list
                    shopList.Add(entry);
                }
            }
        } while (true);

        if (shopList.Count == 0)
        {
            Console.WriteLine("Your cart is empty!");
            return false;
        }
        else
        {
             
            using (Phase3Context db = new Phase3Context(this.connstring))
            {
                for (int i = 0; i < shopList.Count; i++)
                {
                    try
                    {
                        Item item = db.Items.Where(j => j.ItemId == shopList[i].Item1).Single();
                        

                        if (item.AgeRequirement != null && user.Age < item.AgeRequirement)
                        {
                            Console.WriteLine("Sorry, you do not meet the age requirements to order " + item.ItemName +
                                              "! Would you still like to continue with your order?");
                            switch (OptionsMenu("Yes", "No"))
                            {
                                case 1:
                                    shopList.Remove(shopList[i]);
                                    continue;
                                case 2:
                                    return false;
                            }
                        }

                        if (item.QuantityAvailable <= 0)
                        {
                            Console.WriteLine("Sorry, it looks like we are all out of " + item.ItemName +
                                              "! Would you still like to continue with your order?");
                            switch (OptionsMenu("Yes", "No"))
                            {
                                case 1:
                                    shopList.Remove(shopList[i]);
                                    continue;
                                case 2:
                                    return false;
                            }
                        }
                        else if (item.QuantityAvailable < shopList[i].Item2)
                        {
                            Console.WriteLine("You requested " + shopList[i].Item2 + " " + item.ItemName +
                                              "(s), but it looks like we only have " + item.QuantityAvailable +
                                              " left! What would you like to do?");
                            switch (OptionsMenu("Just give me the " + item.QuantityAvailable + " in stock",
                                        "Remove item from order and continue", "Cancel entire order"))
                            {
                                case 1:
                                    shopList[i] = (shopList[i].Item1, item.QuantityAvailable);
                                    //Increment price proper amount
                                    orderTotal += (item.Price * shopList[i].Item2);
                                    continue;
                                case 2:
                                    shopList.Remove(shopList[i]);
                                    continue;
                                case 3:
                                    return false;
                            }
                        }
                        
                        //Increment price if we successfully validate the item for order
                        orderTotal += (item.Price * shopList[i].Item2);
                    }
                    catch (InvalidOperationException e)
                    {
                        Console.WriteLine("Could not find an item with id " + shopList[i].Item1 +
                                          "! Would you like to continue with the rest of your order?");
                        switch (OptionsMenu("Yes", "No"))
                        {
                            case 1:
                                shopList.Remove(shopList[i]);
                                continue;
                            case 2:
                                return false;
                        }
                    }
                }

                Order newOrder = new Order();
                newOrder.OrderId = rand.Next(100000000);
                newOrder.OrderTotal = orderTotal;
                newOrder.CustomerId = user.CustomerId;
                newOrder.OrderTimestamp = DateTime.Now;
                
                //Status of 1 = Placed and needing to be fulfilled
                newOrder.OrderStatus = 1;

                if (user.DeliveryLocation != null)
                {
                    Console.WriteLine("Would you like this order delivered?");
                    switch (OptionsMenu("Yes", "No"))
                    {
                        case 1:
                            //if an order has an estimated delivery time, then we know it is a delivery order
                            newOrder.EstimatedDeliveryTime = DateTime.Now.AddDays(2);
                            break;
                        case 2:
                            break;
                    }
                }

                db.Orders.Add(newOrder);
                
                foreach (ValueTuple<int, int> k in shopList)
                {
                    db.ItemsOrdereds.Add(new ItemsOrdered(newOrder.OrderId, k.Item1, k.Item2));
                    Item item = db.Items.Where(j => j.ItemId == k.Item1).Single();
                    db.Update(item);
                    item.QuantityAvailable -= k.Item2;
                    db.PickLists.Add(new PickList(k.Item1, k.Item2, newOrder.OrderId));
                }

                db.SaveChanges();
            }
        }
        //Here to keep the compiler quiet during testing
        return true;
    }
    
    private List<Item> itemSearch(string query)
    {
        using (Phase3Context db = new Phase3Context(this.connstring))
        {
            return db.Items.Where(i => i.ItemName.Contains(query)).ToList();
        }
    }
}