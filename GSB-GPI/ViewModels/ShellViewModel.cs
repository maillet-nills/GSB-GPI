using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GSB_GPI.Models;

namespace GSB_GPI.ViewModels;

public partial class ShellViewModel : ViewModelBase
{
    public event Action? LogoutRequested;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsDashboardActive))]
    [NotifyPropertyChangedFor(nameof(IsInventoryActive))]
    [NotifyPropertyChangedFor(nameof(IsIncidentsActive))]
    private ViewModelBase _currentPage = new DashboardViewModel();

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

    [RelayCommand]
    private void ShowDashboard()
    {
        CurrentPage = new DashboardViewModel();
    }

    [RelayCommand]
    private void ShowInventory()
    {
        InventoryViewModel inventoryViewModel = new InventoryViewModel();
        inventoryViewModel.CreateRequested += ShowInventoryItem;
        CurrentPage = inventoryViewModel;
    }

    private void ShowInventoryItem()
    {
        InventoryItemViewModel inventoryItemViewModel = new InventoryItemViewModel();
        inventoryItemViewModel.CloseRequested += ShowInventory;
        CurrentPage = inventoryItemViewModel;
    }

    [RelayCommand]
    private void ShowTicket()
    {
        TicketViewModel ticketViewModel = new TicketViewModel();
        ticketViewModel.CreateRequested += ShowTicketItem;
        CurrentPage = ticketViewModel;
    }
    
    private void ShowTicketItem()
    {
        TicketItemViewModel ticketItemViewModel = new TicketItemViewModel();
        ticketItemViewModel.Closed += ShowTicket;
        CurrentPage = ticketItemViewModel;
    }

    [RelayCommand]
    private void Logout()
    {
        LogoutRequested?.Invoke();
    }
}