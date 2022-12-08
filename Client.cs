using Phase3Databases.DatabaseModels;

namespace Phase3Databases;

public class Client
{
    private UserType clientType;
    private string connstring;
    private Phase3Context Phase3DB;
    private int uid = -1;
    
    public Client(UserType logintype, string connstring)
    {
        //May eventually want to switch the check for valid enum to be here instead
        clientType = logintype;
        Phase3DB = new Phase3Context(connstring);
        
        
        //Get User ID
        do
        {
            Console.WriteLine("Please enter your user ID: \n"); 
        }
        while (!int.TryParse(Console.ReadLine(), out uid)) ;
        
        //Queries here? https://learn.microsoft.com/en-us/ef/core/querying/
        
    }
}