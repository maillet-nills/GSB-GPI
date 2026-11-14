using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GSB_GPI.Data;
using GSB_GPI.Models;

namespace GSB_GPI.ViewModels;

public partial class TicketItemViewModel : ViewModelBase
{
	private Ticket? _ticketToEdit;
    public event Action? Closed;
    public Urgency[] Urgencies { get; } = Enum.GetValues<Urgency>();
    public State[]  States { get; } = Enum.GetValues<State>();
    public Level[] Levels { get; } = Enum.GetValues<Level>();

    public List<User> UsersList { get; set; }
    public List<Technician> TechniciansList { get; set; }
    public List<Equipment> EquipmentsList { get; set; }

    [ObservableProperty] private User _selectedUser;
    [ObservableProperty] private Technician _selectedTechnician;
    [ObservableProperty] private Equipment _selectedEquipment;

    [ObservableProperty] private string _subject = string.Empty;
    [ObservableProperty] private Urgency _selectedUrgency = Urgency.FAIBLE;
    [ObservableProperty] private State _selectedState = State.ENREGISTRE;
    [ObservableProperty] private Level _selectedLevel = Level.N1;
    
    [ObservableProperty]
    private bool _isTechnician = DbToolBox.IsTechnician();

    public TicketItemViewModel(Ticket? ticket = null)
    {
	    _ticketToEdit = ticket;
	    LoadData();
	    if (_ticketToEdit != null)
	    {
		    Console.WriteLine(_ticketToEdit.User == null);
		    Console.WriteLine(_ticketToEdit.Technician == null);
		    Console.WriteLine(_ticketToEdit.Equipment == null);
		    Console.WriteLine(UsersList.Count);
		    SelectedUser = UsersList.FirstOrDefault(u => u.IdUser == _ticketToEdit.User?.IdUser);
		    SelectedTechnician = TechniciansList.FirstOrDefault(t => t.IdUser == _ticketToEdit.Technician?.IdUser);
		    SelectedEquipment = EquipmentsList.FirstOrDefault(e => e.Id == _ticketToEdit.Equipment?.Id);
		    Subject = _ticketToEdit.Subject;
		    SelectedUrgency = _ticketToEdit.NvUrgency;
		    SelectedState = _ticketToEdit.State;
		    SelectedLevel = _ticketToEdit.InterventionLevel;
	    }else if (!IsTechnician)
	    {
		    SelectedUser = UsersList.FirstOrDefault(u => u.IdUser == UserSession.CurrentUser.IdUser);
		    SelectedTechnician = null;
	    }
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
	    //MessageBox à insérer.
	    if (_ticketToEdit == null)
	    {
		    DbToolBox.CreateTicket(Subject,SelectedUser,SelectedTechnician,SelectedUrgency,SelectedState,SelectedLevel,SelectedEquipment);
		    Console.WriteLine("Ticket created !");
	    }
	    else
	    {
		    DbToolBox.UpdateTicket(_ticketToEdit, Subject, SelectedEquipment, SelectedUser, SelectedTechnician, SelectedUrgency, SelectedState, SelectedLevel );
		    Console.WriteLine("Ticket updated !");
	    }
		    
        Closed?.Invoke();
    }

    [RelayCommand]
    private void Delete()
    {
	    DbToolBox.DeleteTicket(_ticketToEdit);
	    Closed?.Invoke();
    }

    [RelayCommand]
    private void Cancel()
    {
        Closed?.Invoke();
    }
}
