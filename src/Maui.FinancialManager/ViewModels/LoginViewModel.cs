using System.Text;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maui.FinancialManager.Services;
using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;

namespace Maui.FinancialManager.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    const string USE_BIOMETRIC = "useBiometric";
    private readonly AuthenticationService _authenticationService;

    public LoginViewModel(AuthenticationService authenticationService)
    {
        this.Initialize();
        this._authenticationService = authenticationService;
    }

    public string Version { get; set; } = $"{AppInfo.Current.Name} - {AppInfo.Version}";

    [ObservableProperty]
    string userLogin;

    [ObservableProperty]
    bool hasBiometricAuthentication;

    [ObservableProperty]
    string userPassword;

    [RelayCommand]
    async void Login()
    {
        var token = await this._authenticationService.Authenticate(this.UserLogin, this.UserPassword);
        if (string.IsNullOrEmpty(token))
        {
            _ = Shell.Current.DisplayAlert("Erro", "Usuário e/ou senha inválidos", "Ok");
        } else
        {
            Preferences.Set(nameof(this.UserLogin), this.UserLogin);
            Preferences.Set(nameof(this.UserPassword), this.UserPassword);
            Preferences.Set("Token", token);
            this.UserPassword = null;
            _ = Shell.Current.GoToAsync("//medicinesearch");
        }
    }

    [RelayCommand]
    async void LoginByFaceId()
    {
        var dialogConfig = new AuthenticationRequestConfiguration("My App", "Authenticate by faceid")
        {
            CancelTitle = "Cancelar",
            FallbackTitle = "Voltar",
            AllowAlternativeAuthentication = true,
            ConfirmationRequired = true
        };

        var authResult = await CrossFingerprint.Current.AuthenticateAsync(dialogConfig);
        if (authResult.Authenticated)
        {
            this.UserPassword = Preferences.Get(nameof(this.UserPassword), string.Empty);
            this.LoginCommand.Execute(null);
        }
    }

    async void Initialize()
    {
        if (Preferences.ContainsKey(nameof(this.UserLogin)))
            this.UserLogin = Preferences.Get(nameof(this.UserLogin), string.Empty);

        if (Preferences.Get(USE_BIOMETRIC, false))
        {
            var hasBiometric = await CrossFingerprint.Current.GetAvailabilityAsync();
            this.HasBiometricAuthentication = hasBiometric == FingerprintAvailability.Available;

            if (this.HasBiometricAuthentication && !string.IsNullOrEmpty(this.UserLogin))
                this.LoginByFaceIdCommand.Execute(null);
        }
    }
}

