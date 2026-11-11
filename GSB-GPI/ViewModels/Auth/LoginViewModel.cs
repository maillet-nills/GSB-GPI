using System;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GSB_GPI.Data;
using GSB_GPI.Models;

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
			    var user = context.Users.First(u => u.Email == Email);
			    UserSession.CurrentUser =  user;

			    LoginSuceeded?.Invoke();

		    }

		    else
		    {
			    //Message d'erreur
			    Console.WriteLine("Email or password is incorrect.");
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
