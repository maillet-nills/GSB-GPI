using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GSB_GPI.Data;
using GSB_GPI.Models;

namespace GSB_GPI.ViewModels;

public partial class TicketViewModel : ViewModelBase
{
    public event Action? CreateRequested;
    public event Action<Ticket>? EditRequested;
    //public event Action? NoTicketsRequested;
    public ObservableCollection<Ticket> Items { get; set; } = new();

    public string[] Levels { get; } = { "Tous", "N1", "N1_5", "N2", "N3" };

    [ObservableProperty] private string _selectedLevel = "Tous";
    
    [ObservableProperty] private bool _isTechnician = DbToolBox.IsTechnician();
    
    public IEnumerable<Ticket> FilteredItems =>
        SelectedLevel == "Tous" ? Items : Items.Where(e => e.InterventionLevel.ToString() == SelectedLevel);

    public bool IsEmpty => !FilteredItems.Any();

    partial void OnSelectedLevelChanged(string value) => Refresh();

    public TicketViewModel()
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

        Items = new ObservableCollection<Ticket>(DbToolBox.LoadTickets());

        if (Items.Count == 0)
        {
	        //NoTicketsRequested?.Invoke();
        }

    }

    [RelayCommand]
    private void Create()
    {
        CreateRequested?.Invoke();
    }

    [RelayCommand]
    private void Edit(Ticket item)
    {
        EditRequested?.Invoke(item);
    }
}
