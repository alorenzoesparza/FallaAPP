using FallaAPP.Helpers;
using FallaAPP.Services;
using FallaAPP.Views;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using Xamarin.Forms;

namespace FallaAPP.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        #region Servicios
        private ApiService apiService;
        #endregion

        #region Atributos
        private string email;
        private string password;
        private bool isRunning;
        private bool isEnabled;
        #endregion

        #region Propiedades
        public string Email
        {
            get { return this.email; }
            set { SetValue(ref this.email, value); }
        }

        public string Password
        {
            get { return this.password; }
            set { SetValue(ref this.password, value); }
        }

        public bool IsRunning
        {
            get { return this.isRunning; }
            set { SetValue(ref this.isRunning, value); }
        }

        public bool IsRemembered { get; set; }

        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set { SetValue(ref this.isEnabled, value); }
        }

        #endregion

        #region Constructor
        public LoginViewModel()
        {
            apiService = new ApiService();

            this.IsRemembered = true;
            this.IsEnabled = true;
            this.IsRunning = false;

            this.Email = "alorenzoesparza@ono.com";
            this.Password = "Antonio1.";
        }
        #endregion

        #region Comandos
        public ICommand LoginCommand
        {
            get
            {
                return new RelayCommand(Login);
            }
        }

        private async void Login()
        {
            if (string.IsNullOrEmpty(this.Email))
            {
                this.IsRunning = false;
                this.IsEnabled = true;

                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must enter an Email",
                    "Accept");
                return;
            }

            if (string.IsNullOrEmpty(this.Password))
            {
                this.IsRunning = false;
                this.IsEnabled = true;

                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must enter a password.",
                    "Accept");
                this.Password = string.Empty;
                return;
            }

            // Activar el ActivityIndicator (IsRunning) y desactivar Botones
            this.IsRunning = true;
            this.IsEnabled = false;

            var connection = await this.apiService.CheckConnection();

            if (!connection.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;

                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    connection.Message,
                    "Accept");
                return;
            }

            var token = await this.apiService.GetToken(
                "http://api.antoniole.com",
                this.Email,
                this.Password);

            if (token == null)
            {
                this.IsRunning = false;
                this.IsEnabled = true;

                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "The service is not ready. Retry later.",
                    "Accept");
                return;
            }

            if (string.IsNullOrEmpty(token.AccessToken))
            {
                this.IsRunning = false;
                this.IsEnabled = true;

                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    token.ErrorDescription,
                    "Aceptar");
                this.Password = string.Empty;
                return;
            }

            var mainViewModel = MainViewModel.GetInstance();

            mainViewModel.Token = token.AccessToken;
            mainViewModel.TokenType = token.TokenType;
            
            if (this.IsRemembered)
            {
                // Guardar en persistencia el Token
                Settings.Token = token.AccessToken;
                Settings.TokenType = token.TokenType;
            }

            mainViewModel.Acts = new ActsViewModel();

            //await Application.Current.MainPage.Navigation.PushAsync(new ActsPage());
            Application.Current.MainPage = new MasterPage();

            this.IsRunning = false;
            this.IsEnabled = true;
            this.Email = string.Empty;
            this.Password = string.Empty;
        }

        public ICommand RegisterCommand { get; set; }
        #endregion
    }
}
