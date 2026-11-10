using CommunityToolkit.Mvvm.ComponentModel;

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
        CurrentViewModel = loginViewModel;
    }

    private void ShowRegister()
    {
        RegisterViewModel registerViewModel = new RegisterViewModel();
        registerViewModel.LoginRequested += ShowLogin;
        CurrentViewModel = registerViewModel;
    }

}
