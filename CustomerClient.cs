using Phase3Databases.DatabaseModels;

namespace Phase3Databases;

public class CustomerClient : Client
{
    //private Phase3Context Phase3DB;
    private Customer user = null;
    
    public CustomerClient(string connstring) : base()
    {
        
        //Queries here? https://learn.microsoft.com/en-us/ef/core/querying/
        using (Phase3Context db = new Phase3Context(this.connstring))
        {
            
        }
        
        //if we cant find user, throw exception
        if (user == null) throw new UserNotFoundException("Couldn't find user with id " + this.uid);

    }

    public override void MainLoop()
    {
        throw new NotImplementedException();
    }
}