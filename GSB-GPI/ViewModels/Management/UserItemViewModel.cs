using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GSB_GPI.Data;
using GSB_GPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GSB_GPI.ViewModels;

public partial class UserItemViewModel : ViewModelBase
{
    public event Action? Closed;

    private readonly User? _user;
    private readonly bool _isEditMode;

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
    [ObservableProperty] private string _phonenumber = string.Empty;
    [ObservableProperty] private string _registernumber = string.Empty;
    [ObservableProperty] private string _password = string.Empty;
    [ObservableProperty] private string _confirmPassword = string.Empty;
    [ObservableProperty] private string _selectedRole = "Client";
    [ObservableProperty] private string _errorMessage = string.Empty;

    public UserItemViewModel()
    {
	    _isEditMode = false;
    }

    public UserItemViewModel(User? user)
    {
	    if (user == null)
	    {
		    _isEditMode = false;
		    return;
	    }

	    _user = user;
	    _isEditMode = true;

	    LastName = user.LastName;
	    FirstName = user.FirstName;
	    Email = user.Email;
	    Phonenumber = user.PhoneNumber ?? string.Empty;
	    Registernumber = user.Register ?? string.Empty;
	    SelectedRole = user.Role ?? "Client";
    }

    [RelayCommand]
    private void Save()
    {
        if (string.IsNullOrWhiteSpace(LastName) ||
            string.IsNullOrWhiteSpace(FirstName) ||
            string.IsNullOrWhiteSpace(Email) ||
            string.IsNullOrWhiteSpace(Phonenumber) ||
            string.IsNullOrWhiteSpace(Registernumber))
        {
            ErrorMessage = "Tous les champs sont obligatoires.";
            return;
        }

        if (!Email.Contains('@'))
        {
            ErrorMessage = "L'adresse email n'est pas valide.";
            return;
        }

        if (Phonenumber.Length != 10)
        {
	        ErrorMessage = "Ce numéro n'est pas valide.";
	        return;
        }

        if (!string.IsNullOrEmpty(Password) && Password != ConfirmPassword)
        {
            ErrorMessage = "Les mots de passe ne correspondent pas.";
            return;
        }

        if (!_isEditMode && string.IsNullOrWhiteSpace(Password))
        {
	        ErrorMessage = "Le mot de passe est obligatoire pour la création d'un compte.";
	        return;
        }

        bool created = DbToolBox.Register(LastName, FirstName, Email, Password, ConfirmPassword, SelectedRole, Phonenumber,Registernumber);

        if (!_isEditMode &&  !created)
        {
	        ErrorMessage = "Un utilisateur avec cet email existe déjà.";
	        return;
        }

        if (_isEditMode && _user != null)
        {
	        bool updated = DbToolBox.UpdateUser(_user, LastName, FirstName, Email,Password,SelectedRole, Phonenumber, Registernumber);

	        if (!updated)
	        {
		        ErrorMessage = "Erreur lors de la mise à jour.";
		        return;
	        }
        }

        Closed?.Invoke();
    }

    [RelayCommand]
    private void Delete()
    {
	    if (!_isEditMode || _user == null) return;

	    if (_user.IdUser == UserSession.CurrentUser.IdUser)
	    {
		    ErrorMessage = "Vous ne pouvez pas supprimer votre propre compte.";
		    return;
	    }

	    try
	    {
		    DbToolBox.DeleteUser(_user);
		    Closed?.Invoke();
	    }
	    catch (DbUpdateException)
	    {
		    ErrorMessage = "Suppression impossible : cet utilisateur est lié à des tickets ou du matériel.";
	    }
    }

    [RelayCommand]
    private void Cancel()
    {
        Closed?.Invoke();
    }





}
