using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GSB_GPI.Models;

[Table("Workstation")]
public class Workstation : Equipment
{
    public string Processor { get; set; }
    public string Os { get; set; }
    public string Ram { get; set; }
    public string DiskVolume { get; set; }
    public List<string> Software { get; set; } = new();
    
    [NotMapped]
    public override string Category => "Poste de travail";

    public Workstation(int id, string idGsb, string brand, string model, DateTime purchaseDate, bool rental, bool guarantee, string provider, DateTime createdAt, string processor, string os, string ram, string diskVolume) : base(id, idGsb, brand, model, purchaseDate, rental, guarantee, provider, createdAt)
    {
        Processor = processor;
        Os = os;
        Ram = ram;
        DiskVolume = diskVolume;
    }
}
