using Phase3Databases.DatabaseModels;

namespace Phase3Databases;

public class EmployeeClient : Client
{
    private Employee user;

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
        throw new NotImplementedException();
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