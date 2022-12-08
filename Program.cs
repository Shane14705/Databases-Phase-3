// See https://aka.ms/new-console-template for more information

using Microsoft.EntityFrameworkCore;
using Phase3Databases;
using Phase3Databases.DatabaseModels;


Console.WriteLine("Hello, World!");

string connstring = File.OpenText("ConnectionString.txt")?.ReadLine();


while (true)
{
    Console.WriteLine("Please login! Are you a customer, an employee, or a courier?\n");
    Client myclient = new Client((UserType) (OptionsMenu("Customer", "Employee", "Courier")), connstring);
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


Console.WriteLine(OptionsMenu("Hello", "World", "This", "Is", "A", "Test!"));
