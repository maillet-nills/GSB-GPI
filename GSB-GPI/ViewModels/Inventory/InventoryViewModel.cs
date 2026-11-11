using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GSB_GPI.Models;

namespace GSB_GPI.ViewModels;

public partial class InventoryViewModel : ViewModelBase
{
    public event Action? CreateRequested;
    public ObservableCollection<Equipment> Items { get; } = new();

    public string[] Kinds { get; } = { "Tous", "Poste de travail", "Serveur", "Équipement réseau" };

    [ObservableProperty]
    private string _selectedKind = "Tous";

    public IEnumerable<Equipment> FilteredItems =>
        SelectedKind == "Tous" ? Items : Items.Where(e => e.Category == SelectedKind);

    public bool IsEmpty => !FilteredItems.Any();

    partial void OnSelectedKindChanged(string value) => Refresh();

    private void Refresh()
    {
        OnPropertyChanged(nameof(FilteredItems));
        OnPropertyChanged(nameof(IsEmpty));
    }

    [RelayCommand]
    private void Create()
    {
        CreateRequested?.Invoke();
    }

    [RelayCommand]
    private void Edit(Equipment item)
    {
    }
}