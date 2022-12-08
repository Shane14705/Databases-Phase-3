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
        switch ((UserType) (Client.OptionsMenu("Customer", "Employee", "Courier")))
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






