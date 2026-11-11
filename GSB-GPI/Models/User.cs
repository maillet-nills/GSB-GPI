using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GSB_GPI.Models;

[Table("User")]
public class User
{
	[Key]
	[Column("idUser")]
    public int IdUser { get; set; }

    [Column("last_name")]
    public string? LastName { get; set; }

    [Column("first_name")]
    public string? FirstName { get; set; }

    [Column("address")]
    public string? Address { get; set; }

    [Column("register")]
    public string? Register { get; set; }

    [Column("hiring_date")]
    public DateTime ?HiringDate { get; set; }

    [Column("regions")]
    public List<String>? Regions { get; set; } = new();

    [Column("email")]
    public string Email { get; set; }

    [Column("hashed_password")]
    public string HashedPassword { get; set; }

    [Column("is_supervisor")]
    public bool ?IsSupervisor { get; set; }

    [Column("role")]
    public string Role { get; set; }

    public User(){}

    public User(int idUser, string lastName, string firstName, string address, string register, DateTime hiringDate, List<string> regions,string email, string hashedPassword, bool isSupervisor,string role)
    {
        IdUser = idUser;
        LastName = lastName;
        FirstName = firstName;
        Address = address;
        Register = register;
        HiringDate = hiringDate;
        Regions = regions;
        Email = email;
        HashedPassword = hashedPassword;
        IsSupervisor = isSupervisor;
        Role = role;
    }

    public override string ToString()
    {
	    return $"{IdUser} : {LastName} {FirstName}";
    }
}
