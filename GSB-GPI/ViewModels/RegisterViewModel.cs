using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GSB_GPI.Data;

namespace GSB_GPI.ViewModels;

public partial class RegisterViewModel : ViewModelBase
{
    public event Action? LoginRequested;

    [ObservableProperty]
    private string _email = string.Empty;

    [ObservableProperty]
    private string? _selectedRole;

    [ObservableProperty]
    private string _password = string.Empty;

    [ObservableProperty]
    private string _confirmPassword = string.Empty;

    public ObservableCollection<string> Roles { get; } = new ObservableCollection<string>
    {
        "Client",
        "Technicien N1",
        "Technicien N1.5",
        "Technicien N2",
        "Technicien N3"
    };



    [RelayCommand]
    private void Register()
    {
	    Console.WriteLine("Méthode epassée");
	    using (var context = new GsbGPIDbContext())
	    {
		    bool success = context.Register(Email, Password, ConfirmPassword, SelectedRole);

		    if (success)
		    {
			    Console.WriteLine("Successfully registered!");
			    LoginRequested?.Invoke();
		    }

		    else
		    {
			    //Message d'erreur
		    }

	    }
    }

    [RelayCommand]
    private void GoToLogin()
    {
        LoginRequested?.Invoke();
    }
}
