using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GSB_GPI.Models;

[Table("Equipment")]
public class Equipment
{
	[Key]
	[Column("id")]
	public int? Id { get; set; }

	[Column("id_gsb")]
	public string IdGsb { get; set; }

	[Column("brand")]
	public string Brand { get; set; }

	[Column("model")]
	public string Model { get; set; }

	[Column("purchase_date")]
	public DateTime PurchaseDate { get; set; }

	[Column("rental")]
	public bool Rental { get; set; }

	[Column("guarantee")]
	public bool Guarantee { get; set; }

	[Column("provider")]
	public string Provider { get; set; }

    [NotMapped]
    public virtual string Category => "Équipement";

    public Equipment(){}

    public Equipment(int id, string idGsb, string brand, string model, DateTime purchaseDate, bool rental, bool guarantee, string provider, DateTime createdAt)
    {
        Id = id;
        IdGsb = idGsb;
        Brand = brand;
        Model = model;
        PurchaseDate = purchaseDate;
        Rental = rental;
        Guarantee = guarantee;
        Provider = provider;
    }
}
