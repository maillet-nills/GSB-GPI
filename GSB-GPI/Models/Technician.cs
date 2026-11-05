using System;
using System.Collections.Generic;

namespace GSB_GPI.Models;

public class Technician : User
{
    public Level InterventionLevel { get; set; }
    public string Formation { get; set; }
    public List<string> Skills { get; set; } = new();

    public Technician(int idUser, string lastName, string firstName, string address, string register, DateTime hiringDate, List<string> regions, string hashedPassword, bool isSupervisor, Level interventionLevel, string formation, List<string> skills) : base(idUser, lastName, firstName, address, register, hiringDate, regions, hashedPassword, isSupervisor)
    {
        InterventionLevel = interventionLevel;
        Formation = formation;
        Skills = skills;
    }
}