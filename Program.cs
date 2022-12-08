// See https://aka.ms/new-console-template for more information

using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using Phase3Databases;
using Phase3Databases.DatabaseModels;


string connstring = File.OpenText("ConnectionString.txt")?.ReadLine();


Client client = null;

while (true)
{
    Console.WriteLine("\nPlease select your account type to login!\n");
    try
    {
        switch ((UserType) (OptionsMenu("Customer", "Employee", "Courier")))
        {
                case UserType.Customers:
                    client = new CustomerClient(connstring);
                    break;

                case UserType.Employees:
                    client = new EmployeeClient(connstring);
                    break;
                
                case UserType.Couriers:
                    client = new CourierClient(connstring);
                    break;
                default:
                    throw new ArgumentException("Invalid usertype given!");
        }
        client.MainLoop();
    }
    catch (UserNotFoundException e)
    {
        Console.WriteLine(e.Message + "\n");
    }

}




//Pass in an array of "options" and this function will print them niceley and return user input.
int OptionsMenu(params string[] options)
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

