using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GSB_GPI.Data;
using GSB_GPI.Models;

namespace GSB_GPI.ViewModels;

public partial class InventoryViewModel : ViewModelBase
{
    public event Action? CreateRequested;
    public event Action<Equipment>? EditRequested;
    public event Action? NoEquipmentsRequested;
    public ObservableCollection<Equipment> Items { get; set; } = new();

    public string[] Kinds { get; } = { "Tous", "Poste de travail", "Serveur", "Équipement réseau" };

    [ObservableProperty]
    private string _selectedKind = "Tous";

    [ObservableProperty] private bool _isTechnician = DbToolBox.IsTechnician();

    public IEnumerable<Equipment> FilteredItems =>
        SelectedKind == "Tous" ? Items : Items.Where(e => e.Category == SelectedKind);

    public bool IsEmpty => !FilteredItems.Any();

    partial void OnSelectedKindChanged(string value) => Refresh();

    public InventoryViewModel()
    {
        LoadData();
    }

    private void Refresh()
    {
        OnPropertyChanged(nameof(FilteredItems));
        OnPropertyChanged(nameof(IsEmpty));
    }

    public void LoadData()
    {
	    Items = new ObservableCollection<Equipment>(DbToolBox.LoadEquipments().ToList());

	    if (Items.Count == 0)
	    {
		    NoEquipmentsRequested?.Invoke();
	    }

    }

    [RelayCommand]
    private void Create()
    {
        CreateRequested?.Invoke();
    }

    [RelayCommand]
    private void Edit(Equipment? item = null)
    {
        EditRequested?.Invoke(item);
    }
}
