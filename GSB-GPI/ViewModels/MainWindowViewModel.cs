using CommunityToolkit.Mvvm.ComponentModel;
using GSB_GPI.Models;

namespace GSB_GPI.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private ViewModelBase? _currentViewModel;

    public MainWindowViewModel()
    {
        ShowLogin();
    }

    private void ShowLogin()
    {
        LoginViewModel loginViewModel = new LoginViewModel();
        loginViewModel.RegisterRequested += ShowRegister;
        loginViewModel.LoginSuceeded += ShowShell;
        CurrentViewModel = loginViewModel;
    }

    private void ShowRegister()
    {
        RegisterViewModel registerViewModel = new RegisterViewModel();
        registerViewModel.LoginRequested += ShowLogin;
        CurrentViewModel = registerViewModel;
    }
    
    private void ShowShell()
    {
        ShellViewModel shellViewModel = new ShellViewModel();
        shellViewModel.LogoutRequested += ShowLogin;
        CurrentViewModel = shellViewModel;
    }

}
