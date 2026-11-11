using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GSB_GPI.Models;

[Table("Workstation")]
public class Workstation : Equipment
{
	[Column("processor")]
	public string Processor { get; set; }

	[Column("os")]
	public string Os { get; set; }

	[Column("ram")]
	public long? Ram { get; set; }

	[Column("disk_volume")]
	public int DiskVolume { get; set; }

	[Column("software")]
	public string[]? Software { get; set; }

    [NotMapped]
    public override string Category => "Poste de travail";

    public Workstation(){}

    public Workstation(int id, string idGsb, string brand, string model, DateTime purchaseDate, bool rental, bool guarantee, string provider, DateTime createdAt, string processor, string os, int ram, int diskVolume) : base(id, idGsb, brand, model, purchaseDate, rental, guarantee, provider, createdAt)
    {
        Processor = processor;
        Os = os;
        Ram = ram;
        DiskVolume = diskVolume;
    }
}
