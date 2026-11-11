using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GSB_GPI.Models;

[Table("Technician")]
public class Technician : User
{
	[Column("intervention_level")]
    public Level? InterventionLevel { get; set; }

    [Column("education")]
    public string? Formation { get; set; }

    [Column("skills")]
    public List<string>? Skills { get; set; } = new();

    public Technician(){}

    public Technician(int idUser, string lastName, string firstName, string address, string register, DateTime hiringDate, List<string> regions,string email, string hashedPassword, bool isSupervisor, Level interventionLevel, string formation, List<string> skills, string role) : base(idUser, lastName, firstName, address, register, hiringDate, regions, email, hashedPassword, isSupervisor,role)
    {
        InterventionLevel = interventionLevel;
        Formation = formation;
        Skills = skills;
    }

    public override string ToString()
    {
	    return $"{IdUser} : {LastName} {FirstName}";
    }
}
