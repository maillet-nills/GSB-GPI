using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GSB_GPI.Models;

[Table("NetEquipment")]
public class NetEquipment : Equipment
{
	[Column("type")]
    public string Type { get; set; }

    [NotMapped]
    public override string Category => "Équipement réseau";

    public NetEquipment(){}
    public NetEquipment(int id, string idGsb, string brand, string model, DateTime purchaseDate, bool rental, bool guarantee, string provider, DateTime createdAt, string type,User user) : base(id, idGsb, brand, model, purchaseDate, rental, guarantee, provider, createdAt,user)
    {
        this.Type = type;
    }
}
