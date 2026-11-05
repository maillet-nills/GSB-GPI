using System;

namespace GSB_GPI.Models;

public class Equipment
{
    public int Id { get; set; }
    public string IdGsb { get; set; }
    public string Brand  { get; set; }
    public string Model { get; set; }
    public DateTime PurchaseDate { get; set; }
    public bool Rental { get; set; }
    public bool Guarantee { get; set; }
    public string Provider { get; set; }
    public DateTime CreatedAt  { get; set; }

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
        CreatedAt = createdAt;
    }
}