using System;

namespace GSB_GPI.Models;

public class NetEquipment : Equipment
{
    public string Type { get; set; }

    public NetEquipment(int id, string idGsb, string brand, string model, DateTime purchaseDate, bool rental, bool guarantee, string provider, DateTime createdAt, string type) : base(id, idGsb, brand, model, purchaseDate, rental, guarantee, provider, createdAt)
    {
        this.Type = type;
    }
}