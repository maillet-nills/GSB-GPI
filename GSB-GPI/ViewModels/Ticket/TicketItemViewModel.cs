using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GSB_GPI.Data;
using GSB_GPI.Models;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace GSB_GPI.ViewModels;

public partial class TicketItemViewModel : ViewModelBase
{
    public event Action? Closed;
    public Urgency[] Urgencies { get; } = Enum.GetValues<Urgency>();
    public State[]  States { get; } = Enum.GetValues<State>();
    public Level[] Levels { get; } = Enum.GetValues<Level>();

    public List<User> UsersList { get; set; }
    public List<Technician> TechniciansList { get; set; }
    public List<Equipment> EquipmentsList { get; set; }

    public User SelectedUser { get; set; }
    public Technician SelectedTechnician { get; set; }
    public Equipment SelectedEquipment { get; set; }

    [ObservableProperty] private string _subject = string.Empty;
    [ObservableProperty] private Urgency _selectedUrgency = Urgency.FAIBLE;
    [ObservableProperty] private State _selectedState = State.ENREGISTRE;
    [ObservableProperty] private Level _selectedLevel = Level.N1;

    public TicketItemViewModel()
    {
	    LoadData();
    }

    public void LoadData()
    {
	    UsersList = DbToolBox.getAllUsers();
	    TechniciansList = DbToolBox.getAllTechnicians();
	    EquipmentsList = DbToolBox.LoadEquipments();

    }

    [RelayCommand]
    private void Save()
    {
		    DbToolBox.CreateTicket(Subject,SelectedUser,SelectedTechnician,SelectedUrgency,SelectedState,SelectedLevel,SelectedEquipment);
		    Console.WriteLine("Ticket created !");
		    //MessageBox à insérer.
        Closed?.Invoke();
    }

    [RelayCommand]
    private void Cancel()
    {
        Closed?.Invoke();
    }
}
