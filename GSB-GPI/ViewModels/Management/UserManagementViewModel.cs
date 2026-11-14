using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GSB_GPI.Data;
using GSB_GPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GSB_GPI.ViewModels;

public partial class UserManagementViewModel : ViewModelBase
{
    public event Action? CreateRequested;

    public ObservableCollection<User> Items { get; set; } = new();

    public string[] Roles { get; } = { "Tous", "Client", "Technicien", "Admin" };

    [ObservableProperty] private string _selectedRole = "Tous";
    [ObservableProperty] private string _errorMessage = string.Empty;

    public IEnumerable<User> FilteredItems =>
        SelectedRole == "Tous" ? Items : Items.Where(u => u.Role.Contains(SelectedRole));

    public bool IsEmpty => !FilteredItems.Any();

    partial void OnSelectedRoleChanged(string value) => Refresh();

    public UserManagementViewModel()
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
        Items = new ObservableCollection<User>(DbToolBox.getAllUsers());
    }

    [RelayCommand]
    private void Create()
    {
        CreateRequested?.Invoke();
    }

    [RelayCommand]
    private void Delete(User user)
    {
        if (user.IdUser == UserSession.CurrentUser.IdUser)
        {
            ErrorMessage = "Vous ne pouvez pas supprimer votre propre compte.";
            Console.WriteLine("Delete refused: a user cannot delete their own account.");
            return;
        }

        try
        {
            DbToolBox.DeleteUser(user);
        }
        catch (DbUpdateException)
        {
            ErrorMessage = "Suppression impossible : cet utilisateur est lié à des tickets ou à du matériel.";
            Console.WriteLine($"Delete failed: user {user.IdUser} is still referenced by tickets or equipment.");
            return;
        }

        ErrorMessage = string.Empty;
        Items.Remove(user);
        Refresh();
        Console.WriteLine($"User {user.IdUser} deleted successfully.");
    }
}