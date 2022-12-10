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
                if (db.PickLists.Any(pl => pl.EmployeeId == uid))
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

    private PickWalk getCurrentWalk()
    {
        using (Phase3Context db = new Phase3Context(this.connstring))
        {
            List<PickList> list = db.PickLists.Where(pl => pl.EmployeeId == uid).ToList();
            if (list.Count() > 0)
            {
                PickWalk walk = db.PickWalks.Where(pw =>
                    ((pw.EmployeeId == list[0].EmployeeId) && (pw.StartTimestamp == list[0].StartTimestamp))).Single();
                return walk;
            }
            else
            {
                throw new InvalidOperationException("Didn't find any items associated with this employee in the picking queue.");
            }
        }
    }

    //Returns bool representing whether a pickwalk was successfully ended or not.
    private bool EndPick_Walk()
    {
        PickWalk walk;
        if (!IsPicking)
        {
            Console.WriteLine("You dont have any items to be picked right now!");
            return false;
        }
        else
        {
            // try
            // {
            //     walk = getCurrentWalk();
            // }
            // catch (System.InvalidOperationException e)
            // {
            //     Console.WriteLine(e);
            //     return false;
            // }
            
            using (Phase3Context db = new Phase3Context(this.connstring))
            {
                List<PickList> list = db.PickLists.Where(pl => pl.EmployeeId == uid).ToList();
                if (list.Count() > 0)
                {
                    walk = db.PickWalks.Where(pw =>
                        ((pw.EmployeeId == list[0].EmployeeId) && (pw.StartTimestamp == list[0].StartTimestamp))).Single();
                }
                else
                {
                    throw new InvalidOperationException("Didn't find any items associated with this employee in the picking queue.");
                }
                db.Update(user);
                db.Update(walk);
                // db.Attach(user);
                walk.EndTimestamp = DateTime.Now;
                walk.WalkDuration = (float) (walk.EndTimestamp - walk.StartTimestamp).Value.TotalHours;
                walk.PickRate = (walk.PickLists.Count / walk.WalkDuration);
                walk.TotalQuantityPicked = walk.PickLists.Count;
                db.SaveChanges();
                //Get number of walks in the system and divide their pickrates
                int numwalks = db.PickWalks.Count(pw => (pw.EmployeeId == uid));
                user.CumulativePickrate += (walk.PickRate / numwalks).Value;
                
                //Mark order as fulfilled
                list[0].Order.OrderStatus = 2;
                db.PickLists.RemoveRange(walk.PickLists);
                db.SaveChanges();
            }

            return true;
        }

        return false;
    }
    
    
    //Returns bool representing whether a pickwalk was successfully done or not.
    private bool StartPick_Walk()
    {
        if (IsPicking)
        {
            Console.WriteLine("You already have items to pick! Finish that order first!");
            return false;
        }
        
        using (Phase3Context db = new Phase3Context(this.connstring))
        {
            List<PickList> queue = db.PickLists.Where(pl => (pl.PickWalk == null)).ToList();
            if (queue.Count <= 0)
            {
                Console.WriteLine("No items are in the picking queue right now!");
                return false;
            }
            
            queue = queue.GroupBy(pl => pl.Order).First().ToList();
            db.Attach(user);
            PickWalk newWalk = new PickWalk(DateTime.Now, this.uid);
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
        while (option != 3)
        {
            Console.WriteLine("What would you like to do?\n");
            option = Client.OptionsMenu("Start a Pickwalk", "End Pickwalk", "Log-out");
            switch (option)
            {
                case 1:
                    //Start PickWalk
                    StartPick_Walk();
                    break;
                
                case 2:
                    //End Pickwalk
                    EndPick_Walk();
                    break;
            }
        }
    }
}