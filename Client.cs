namespace Phase3Databases;

public abstract class Client
{
    protected string connstring;
    //private Phase3Context Phase3DB;
    protected int uid = -1;

    public Client(string connstring)
    {
        this.connstring = connstring;
        //Get User ID
        do
        {
            Console.WriteLine("Please enter your user ID: \n"); 
        }
        while (!int.TryParse(Console.ReadLine(), out this.uid)) ;
    }
    public abstract void MainLoop();

}

public class UserNotFoundException : Exception
{
    public UserNotFoundException(string message) : base(message)
    {
        
    }
}