using Phase3Databases.DatabaseModels;

namespace Phase3Databases;

public class CustomerClient : Client
{
    //private Phase3Context Phase3DB;
    private Customer user;
    
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
        catch (InvalidOperationException e)
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
            option = Client.OptionsMenu("Search for an item", "Place an order", "Check order status", "Log-out");
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
                    //Place_Order()
                    break;
                case 3:
                    //Check order status
                    break;
            }
        }
    }

    private List<Item> itemSearch(string query)
    {
        using (Phase3Context db = new Phase3Context(this.connstring))
        {
            return db.Items.Where(i => i.ItemName.Contains(query)).ToList();
        }
    }
}