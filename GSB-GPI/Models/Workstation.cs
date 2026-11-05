using System;
using System.Collections.Generic;

namespace GSB_GPI.Models;

public class Workstation : Equipment
{
    public string Processor { get; set; }
    public string Os { get; set; }
    public string Ram { get; set; }
    public string DiskVolume { get; set; }
    public List<string> Software { get; set; } = new();

    public Workstation(int id, string idGsb, string brand, string model, DateTime purchaseDate, bool rental, bool guarantee, string provider, DateTime createdAt, string processor, string os, string ram, string diskVolume) : base(id, idGsb, brand, model, purchaseDate, rental, guarantee, provider, createdAt)
    {
        Processor = processor;
        Os = os;
        Ram = ram;
        DiskVolume = diskVolume;
    }
}