using Phase3Databases.DatabaseModels;

namespace Phase3Databases;

public class CourierClient : Client
{
    private Courier user = null;
    
    public CourierClient(string connstring) : base()
    {
        
        //Queries here? https://learn.microsoft.com/en-us/ef/core/querying/
        using (Phase3Context db = new Phase3Context(this.connstring))
        {
            //Find user here
            
        }
        
        if (user == null) throw new UserNotFoundException("Couldn't find user with id " + this.uid);
        
    }
    public override void MainLoop()
    {
        throw new NotImplementedException();
    }
}