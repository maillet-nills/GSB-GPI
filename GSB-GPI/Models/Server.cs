using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GSB_GPI.Models;

[Table("Server")]
public class Server : Equipment
{
	[Column("version")]
	public string Version { get; set; } = "1.0.0";

	[Column("processor")]
	public string Processor { get; set; }

	[Column("ram")]
	public long? Ram { get; set; } // Attention en C# : un "int8" de PostgreSQL correspond à un "long" (Int64), pas un int !

	[Column("disk_volume")]
	public int DiskVolume { get; set; }

	[Column("software")]
	public string[]? Software { get; set; }

    [NotMapped]
    public override string Category => "Serveur";

    public Server(){}
    public Server(int id, string idGsb, string brand, string model, DateTime purchaseDate, bool rental, bool guarantee, string provider, DateTime createdAt, string version, string processor, int ram, int diskVolume,User user) : base(id, idGsb, brand, model, purchaseDate, rental, guarantee, provider, createdAt,user)
    {
        Version = version;
        Processor = processor;
        Ram = ram;
        DiskVolume = diskVolume;
    }
}
