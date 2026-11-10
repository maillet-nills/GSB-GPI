using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

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
        CurrentPage = new InventoryViewModel();
    }

    [RelayCommand]
    private void ShowTicket()
    {
        CurrentPage = new TicketViewModel();
    }

    [RelayCommand]
    private void Logout()
    {
        LogoutRequested?.Invoke();
    }
}