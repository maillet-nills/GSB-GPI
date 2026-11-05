using System;
using System.Collections.Generic;

namespace GSB_GPI.Models;

public class Server : Equipment
{
    public string Version { get; set; } = "1.0.0";
    public string Processor { get; set; }
    public int Ram { get; set; }
    public int DiskVolume { get; set; }
    public List<string> Software { get; set; } = new();
    
    public Server(int id, string idGsb, string brand, string model, DateTime purchaseDate, bool rental, bool guarantee, string provider, DateTime createdAt, string version, string processor, int ram, int diskVolume) : base(id, idGsb, brand, model, purchaseDate, rental, guarantee, provider, createdAt)
    {
        Version = version;
        Processor = processor;
        Ram = ram;
        DiskVolume = diskVolume;
    }
}