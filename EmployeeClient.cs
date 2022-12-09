using Phase3Databases.DatabaseModels;

namespace Phase3Databases;

public class EmployeeClient : Client
{
    private Employee user;

    public bool IsPicking
    {
        get
        {
            using (Phase3Context db = new Phase3Context(this.connstring))
            {
                if (db.PickLists.Where(pl => pl.EmployeeId == uid).Count() > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
    }

    public EmployeeClient(string connstring) : base(connstring)
    {

        //Queries here? https://learn.microsoft.com/en-us/ef/core/querying/
        try
        {
            using (Phase3Context db = new Phase3Context(this.connstring))
            {
                user = db.Employees.Single(e => (e.EmployeeId == uid));
            }
        }
        catch (InvalidOperationException e)
        {
            //We either got more than one user for id (not possible) or got 0 results (id doesn't exist)
            throw new UserNotFoundException("Couldn't find user with id " + this.uid);
        }

        Console.WriteLine(user.FirstName);
    }

    //Returns bool representing whether a pickwalk was successfully done or not.
    private bool StartPick_Walk()
    {
        using (Phase3Context db = new Phase3Context(this.connstring))
        {
            if (IsPicking)
            {
                Console.WriteLine("You already have items to pick! Finish that order first!");
                return false;
            }
            List<PickList> queue = db.PickLists.Where(pl => (pl.PickWalk == null)).ToList();
            if (queue.Count <= 0)
            {
                Console.WriteLine("No items are in the picking queue right now!");
                return false;
            }
            
            queue = queue.GroupBy(pl => pl.Order).First().ToList();
            db.Attach(user);
            PickWalk newWalk = new PickWalk(DateTime.Now, this.user);
            foreach (PickList k in queue)
            {
                newWalk.PickLists.Add(k);
            }

            db.PickWalks.Add(newWalk);
            db.SaveChanges();
        }

        return true;
    }

    public override void MainLoop()
    {
        int option = -1;
        while (option != 2)
        {
            Console.WriteLine("What would you like to do?\n");
            option = Client.OptionsMenu("Start a Pickwalk", "Log-out");
            switch (option)
            {
                case 1:
                    //Start PickWalk
                    StartPick_Walk();
                    break;

            }
        }
    }
}