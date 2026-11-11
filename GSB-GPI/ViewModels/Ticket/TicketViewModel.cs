using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GSB_GPI.Models;

namespace GSB_GPI.ViewModels;

public partial class TicketViewModel : ViewModelBase
{
    public event Action? CreateRequested;
    public ObservableCollection<Ticket> Items { get; } = new();
    
    public string[] Levels { get; } = { "Tous", "N1", "N1.5", "N2", "N3" };
    
    [ObservableProperty] private string _selectedLevel = "Tous";

    [RelayCommand]
    private void Create()
    {
        CreateRequested?.Invoke();
    }

    [RelayCommand]
    private void Edit(Ticket item){}
}