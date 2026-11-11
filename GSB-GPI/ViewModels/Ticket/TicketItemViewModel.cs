using System;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GSB_GPI.Models;

namespace GSB_GPI.ViewModels;

public partial class TicketItemViewModel : ViewModelBase
{
    public event Action? Closed;
    public Urgency[] Urgencies { get; } = Enum.GetValues<Urgency>();
    public State[]  States { get; } = Enum.GetValues<State>();
    public Level[] Levels { get; } = Enum.GetValues<Level>();

    [ObservableProperty] private string _subject = string.Empty;
    [ObservableProperty] private string _client = string.Empty; // à modifier
    [ObservableProperty] private string _technician = string.Empty; // à modifier
    [ObservableProperty] private Urgency _selectedUrgency = Urgency.FAIBLE;
    [ObservableProperty] private State _selectedState = State.ENREGISTRE;
    [ObservableProperty] private Level _selectedLevel = Level.N1;

    [RelayCommand]
    private void Save()
    {
        Closed?.Invoke();
    }

    [RelayCommand]
    private void Cancel()
    {
        Closed?.Invoke();
    }
}