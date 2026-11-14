using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GSB_GPI.Data;
using GSB_GPI.Models;

namespace GSB_GPI.ViewModels;

public partial class ShellViewModel : ViewModelBase
{
    public event Action? LogoutRequested;

    [ObservableProperty] private bool _isTechnician = DbToolBox.IsTechnician();
    [ObservableProperty] private bool _isAdmin = DbToolBox.IsAdmin();

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsDashboardActive))]
    [NotifyPropertyChangedFor(nameof(IsInventoryActive))]
    [NotifyPropertyChangedFor(nameof(IsIncidentsActive))]
    private ViewModelBase _currentPage;

    public ShellViewModel()
    {
        if (IsTechnician)
        {
            CurrentPage = new DashboardViewModel();
        }
        else
        {
            CurrentPage = new InventoryViewModel();
        }
    }

    public bool IsDashboardActive
    {
        get { return CurrentPage is DashboardViewModel; }
    }

    public bool IsInventoryActive
    {
        get { return CurrentPage is InventoryViewModel; }
    }

    public bool IsIncidentsActive
    {
        get { return CurrentPage is TicketViewModel; }
    }

    public bool IsManagementActive
    {
        get { return CurrentPage is UserManagementViewModel; }
    }

    [RelayCommand]
    private void ShowDashboard()
    {
        CurrentPage = new DashboardViewModel();
    }

    [RelayCommand]
    private void ShowInventory()
    {
        InventoryViewModel inventoryViewModel = new InventoryViewModel();
        inventoryViewModel.CreateRequested += () => ShowInventoryItem();
        inventoryViewModel.EditRequested += ShowInventoryItem;
        CurrentPage = inventoryViewModel;
    }

    private void ShowInventoryItem(Equipment? equipment = null)
    {
        InventoryItemViewModel inventoryItemViewModel = new InventoryItemViewModel(equipment);
        inventoryItemViewModel.CloseRequested += ShowInventory;
        CurrentPage = inventoryItemViewModel;
    }

    [RelayCommand]
    private void ShowTicket()
    {
        TicketViewModel ticketViewModel = new TicketViewModel();
        ticketViewModel.CreateRequested += () => ShowTicketItem();
        ticketViewModel.EditRequested += ShowTicketItem;
        CurrentPage = ticketViewModel;
    }
    
    private void ShowTicketItem(Ticket? ticket = null)
    {
        TicketItemViewModel ticketItemViewModel = new TicketItemViewModel(ticket);
        ticketItemViewModel.Closed += ShowTicket;
        CurrentPage = ticketItemViewModel;
    }
    
    [RelayCommand]
    private void ShowUsers()
    {
        UserManagementViewModel userManagementViewModel = new UserManagementViewModel();
        userManagementViewModel.CreateRequested += ShowUserItem;
        CurrentPage = userManagementViewModel;
    }

    private void ShowUserItem()
    {
        UserItemViewModel userItemViewModel = new UserItemViewModel();
        userItemViewModel.Closed += ShowUsers;
        CurrentPage = userItemViewModel;
    }

    [RelayCommand]
    private void Logout()
    {
        LogoutRequested?.Invoke();
    }
}