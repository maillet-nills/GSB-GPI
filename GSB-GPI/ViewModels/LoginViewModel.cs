using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GSB_GPI.Data;

namespace GSB_GPI.ViewModels;

public partial class LoginViewModel : ViewModelBase
{
    public event Action? RegisterRequested;

    [ObservableProperty]
    private string _email = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;


    [RelayCommand]
    private void Login()
    {
	    using (var context = new GsbGPIDbContext())
	    {
		    bool success = context.Login(Email, Password);

		    if (success)
		    {
			    //RegisterRequested?.Invoke(); Test
			   //Dashboard
		    }

		    else
		    {
			    //Message d'erreur
		    }

	    }

    }

    [RelayCommand]
    private void CreateAccount()
    {
        RegisterRequested?.Invoke();
    }

    [RelayCommand]
    private void ForgotPassword()
    {
    }
}
