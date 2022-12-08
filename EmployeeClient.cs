﻿using Phase3Databases.DatabaseModels;

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
    public override void MainLoop()
    {
        throw new NotImplementedException();
    }
}