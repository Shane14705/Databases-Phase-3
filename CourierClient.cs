using Phase3Databases.DatabaseModels;

namespace Phase3Databases;

public class CourierClient : Client
{
    private Courier user;

    private bool IsAvailable
    {
        get
        {
            using (Phase3Context db = new Phase3Context(this.connstring))
            {
                if (db.Couriers.Where(cor => (cor.CourierId == uid)).Single().Available)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
    
    public CourierClient(string connstring) : base(connstring)
    {
        
        //Queries here? https://learn.microsoft.com/en-us/ef/core/querying/
        try
        {
            using (Phase3Context db = new Phase3Context(this.connstring))
            {
                user = db.Couriers.Single(u => (u.CourierId == uid));
            }
        }
        catch (InvalidOperationException e)
        {
            //We either got more than one user for id (not possible) or got 0 results (id doesn't exist)
            throw new UserNotFoundException("Couldn't find user with id " + this.uid);
        }

        Console.WriteLine(user.FirstName);
    }

    private bool StartDelivery()
    {
        if (!IsAvailable)
        {
            Console.WriteLine("Sorry, looks like you already have a delivery you need to finish first before you can start another one!");
            return false;
        }
        
        using (Phase3Context db = new Phase3Context(this.connstring))
        {
            List<Order> availableOrders =
                db.Orders.Where(or => ((or.OrderStatus == 2) && (or.EstimatedDeliveryTime != null))).ToList();
            if (!availableOrders.Any())
            {
                Console.WriteLine("It doesn't look like there are any orders that need to be delivered right now!");
                return false;
            }
            else
            {
                //this is where we would pick the order closest to the courier, but for now we will just always pick the first order
                availableOrders[0].CourierId = uid;
                //3 for "in-transit"
                availableOrders[0].OrderStatus = 3;
                db.Update(user);
                user.Available = false;

                db.SaveChanges();
            }
        }

        return false;
    }

    private bool EndDelivery()
    {
        if (IsAvailable)
        {
            Console.WriteLine("It looks like you have not started a delivery yet!");
            return false;
        }

        using (Phase3Context db = new Phase3Context(this.connstring))
        {
            db.Attach(user);
            Order currentOrder = db.Orders
                .Single(order => ((order.CourierId == uid) && (order.OrderStatus == 3)));
            db.Update(currentOrder);
            user.CurrentLocation = db.Customers.Single(customer => (currentOrder.CustomerId == customer.CustomerId)).DeliveryLocation;
            currentOrder.DeliveryTime = DateTime.Now;
            currentOrder.HoursElapsed = (float) (currentOrder.DeliveryTime - currentOrder.OrderTimestamp).Value.TotalHours;
            currentOrder.PickupTime = currentOrder.DeliveryTime;
            currentOrder.OrderStatus = 4;
            user.Available = true;
            
            db.SaveChanges();
        }

        return false;
    }
    
    public override void MainLoop()
    {
        int option = -1;
        while (option != 3)
        {
            Console.WriteLine("What would you like to do?\n");
            option = Client.OptionsMenu("Pickup a delivery", "Complete a delivery", "Log-out");
            switch (option)
            {
                case 1:
                    StartDelivery();
                    break;
                case 2:
                    EndDelivery();
                    break;
            }
        }
    }
}