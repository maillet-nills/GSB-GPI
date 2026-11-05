using System;
using System.Collections.Generic;

namespace GSB_GPI.Models;

public class User
{
    public int IdUser { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string Address { get; set; }
    public string Register { get; set; }
    public DateTime HiringDate { get; set; }
    public List<String> Regions { get; set; } = new();
    public string HashedPassword { get; set; }
    public bool IsSupervisor { get; set; }
    
    
    public User(int idUser, string lastName, string firstName, string address, string register, DateTime hiringDate, List<string> regions, string hashedPassword, bool isSupervisor)
    {
        IdUser = idUser;
        LastName = lastName;
        FirstName = firstName;
        Address = address;
        Register = register;
        HiringDate = hiringDate;
        Regions = regions;
        HashedPassword = hashedPassword;
        IsSupervisor = isSupervisor;
    }
}