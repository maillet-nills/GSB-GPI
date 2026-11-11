using System;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace GSB_GPI.ViewModels;

public partial class InventoryItemViewModel : ViewModelBase
{
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
    [ObservableProperty] private string _ram;
    [ObservableProperty] private int _diskVolumeInt;
    [ObservableProperty] private string _diskVolumeString;
    [ObservableProperty] private string _os;

    [RelayCommand]
    private void Save()
    {
        CloseRequested?.Invoke();
    }

    [RelayCommand]
    private void Cancel()
    {
        CloseRequested?.Invoke();
    }
}