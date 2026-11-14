using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GSB_GPI.Data;
using GSB_GPI.Models;

namespace GSB_GPI.ViewModels;

public partial class InventoryItemViewModel : ViewModelBase
{
    private Equipment? _equipmentToEdit;
    public event Action? CloseRequested;

    public List<string> EquipmentTypes { get; } = new()
    {
        "Équipement réseau",
        "Serveur",
        "Poste de travail"
    };

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNetEquipment))]
    [NotifyPropertyChangedFor(nameof(IsServer))]
    [NotifyPropertyChangedFor(nameof(IsWorkstation))]
    private string _selectedType = "Serveur";

    public bool IsNetEquipment => SelectedType == "Équipement réseau";
    public bool IsServer => SelectedType == "Serveur";
    public bool IsWorkstation => SelectedType == "Poste de travail";

    [ObservableProperty] private string _idGsb;
    [ObservableProperty] private string _brand;
    [ObservableProperty] private string _model;
    [ObservableProperty] private System.DateTime _purchaseDate = System.DateTime.Now;
    [ObservableProperty] private bool _rental;
    [ObservableProperty] private bool _guarantee;
    [ObservableProperty] private string _provider;

    [ObservableProperty] private string _type;

    [ObservableProperty] private string _processor;
    [ObservableProperty] private string _version = "1.0.0";
    [ObservableProperty] private int _ram;
    [ObservableProperty] private int? _diskVolumeInt;
    [ObservableProperty] private string _os;

    [ObservableProperty] private User? _selectedUser;
    public List<User> UsersList { get; set; }

    public InventoryItemViewModel(Equipment?  equipment = null)
    {
        _equipmentToEdit = equipment;
	    LoadUsers();
        if (_equipmentToEdit != null)
        {
            IdGsb = _equipmentToEdit.IdGsb;
            Brand = _equipmentToEdit.Brand;
            Model = _equipmentToEdit.Model;
            PurchaseDate = _equipmentToEdit.PurchaseDate;
            Rental = _equipmentToEdit.Rental;
            Guarantee = _equipmentToEdit.Guarantee;
            Provider = _equipmentToEdit.Provider;
            SelectedUser = UsersList.FirstOrDefault(u => u.IdUser == _equipmentToEdit.User?.IdUser);

            if (_equipmentToEdit is NetEquipment net)
            {
                SelectedType = "Équipement réseau";
                Type = net.Type;
            }
            else if (_equipmentToEdit is Workstation ws)
            {
                SelectedType = "Poste de travail";
                Processor = ws.Processor;
                Ram = (int)ws.Ram;
                DiskVolumeInt = ws.DiskVolume;
                Os = ws.Os;
            }
            else if (_equipmentToEdit is Server srv)
            {
                SelectedType = "Serveur";
                Version = srv.Version;
                Processor = srv.Processor;
                Ram = (int)srv.Ram;
                DiskVolumeInt = srv.DiskVolume;
            }
        }
    }

    public void LoadUsers()
    {
	    UsersList = DbToolBox.getAllUsers();
    }

    [RelayCommand]
    private void Save()
    {
        //MessageBox à implémenter
        if (_equipmentToEdit == null)
        {
            DbToolBox.CreateEquipment(IdGsb,Brand,Model,PurchaseDate,Rental,Guarantee,Provider,Type,Processor,Version,Ram,DiskVolumeInt ?? 0,Os,SelectedType,SelectedUser);
            Console.WriteLine("Saved Successfully");
        }
        else
        {
            DbToolBox.UpdateEquipment(_equipmentToEdit, IdGsb, Brand, Model, Rental, Guarantee, Provider, SelectedUser,
                Type, Version, Processor, Ram, DiskVolumeInt ?? 0, Os);
            Console.WriteLine("Updated Successfully");
        }

        CloseRequested?.Invoke();
    }

    [RelayCommand]
    private void Delete()
    {
        DbToolBox.DeleteEquipment(_equipmentToEdit);
        CloseRequested?.Invoke();
    }

    [RelayCommand]
    private void Cancel()
    {
        CloseRequested?.Invoke();
    }
}
