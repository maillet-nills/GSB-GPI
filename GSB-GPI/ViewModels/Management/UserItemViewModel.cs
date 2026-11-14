using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GSB_GPI.Data;

namespace GSB_GPI.ViewModels;

public partial class UserItemViewModel : ViewModelBase
{
    public event Action? Closed;

    public string[] Roles { get; } =
    {
        "Client",
        "Technicien N1",
        "Technicien N1.5",
        "Technicien N2",
        "Technicien N3"
    };

    [ObservableProperty] private string _lastName = string.Empty;
    [ObservableProperty] private string _firstName = string.Empty;
    [ObservableProperty] private string _email = string.Empty;
    [ObservableProperty] private string _password = string.Empty;
    [ObservableProperty] private string _confirmPassword = string.Empty;
    [ObservableProperty] private string _selectedRole = "Client";
    [ObservableProperty] private string _errorMessage = string.Empty;

    [RelayCommand]
    private void Save()
    {
        if (string.IsNullOrWhiteSpace(LastName) ||
            string.IsNullOrWhiteSpace(FirstName) ||
            string.IsNullOrWhiteSpace(Email) ||
            string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "Tous les champs sont obligatoires.";
            return;
        }

        if (!Email.Contains('@'))
        {
            ErrorMessage = "L'adresse email n'est pas valide.";
            return;
        }

        if (Password != ConfirmPassword)
        {
            ErrorMessage = "Les mots de passe ne correspondent pas.";
            return;
        }

        bool created = DbToolBox.Register(LastName, FirstName, Email, Password, ConfirmPassword, SelectedRole);

        if (!created)
        {
            ErrorMessage = "Un utilisateur avec cet email existe déjà.";
            return;
        }

        Closed?.Invoke();
    }

    [RelayCommand]
    private void Cancel()
    {
        Closed?.Invoke();
    }
}