// See https://aka.ms/new-console-template for more information

using Microsoft.EntityFrameworkCore;
using Phase3Databases.DatabaseModels;

Console.WriteLine("Hello, World!");

string connstring = File.OpenText("ConnectionString.txt")?.ReadLine();
Phase3Context Phase3DB = new Phase3Context(connstring);
