using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
namespace GSB_GPI.ViewModels;

public partial class LoginViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _email = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;

    [RelayCommand]
    private void Login()
    {
    }

    [RelayCommand]
    private void CreateAccount()
    {
    }

    [RelayCommand]
    private void ForgotPassword()
    {
    }
}