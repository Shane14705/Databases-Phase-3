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
    //Pass in an array of "options" and this function will print them niceley and return user input.
    public static int OptionsMenu(params string[] options)
    {
        int i;
        do
        {
            Console.WriteLine("Please enter one of the following numbers to select an option: ");
            for (i = 0; i < options.Length; i++)
            {
                Console.WriteLine((i+1) + ". " + options[i]);
            }

            int.TryParse(Console.ReadLine(), out i);
            Console.WriteLine("\n");
        } while (i < 1 || i > options.Length);

        return i;
    }
    public abstract void MainLoop();

}

public class UserNotFoundException : Exception
{
    public UserNotFoundException(string message) : base(message)
    {

    }
}

