using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GSB_GPI.Data;

namespace GSB_GPI.ViewModels;

public partial class LoginViewModel : ViewModelBase
{
    public event Action? RegisterRequested;
    public event Action? LoginSuceeded;

    [ObservableProperty]
    private string _email = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;


    [RelayCommand]
    private void Login()
    {
	    using (var context = new GsbGPIDbContext())
	    {

		    if (context.Login(Email, Password))
		    {
			    LoginSuceeded?.Invoke();
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
