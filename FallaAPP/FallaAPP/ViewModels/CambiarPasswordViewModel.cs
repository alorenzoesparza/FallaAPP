using FallaAPP.Helpers;
using FallaAPP.Services;
using GalaSoft.MvvmLight.Command;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace FallaAPP.ViewModels
{
    public class CambiarPasswordViewModel : BaseViewModel
    {
        #region Servicios
        private ApiService apiService;
        private DataService dataService { get; set; }
        #endregion

        #region Atributos
        private bool isRunning;
        private bool isEnabled;
        #endregion

        #region Propiedades
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

        public string ActualPassword { get; set; }
        public string NuevoPassword { get; set; }
        public string Confirmacion { get; set; }
        #endregion

        #region Constructores
        public CambiarPasswordViewModel()
        {
            this.apiService = new ApiService();
            this.isEnabled = true;
        }
        #endregion

        #region Comandos
        public ICommand CambiarPasswordCommand
        {
            get
            {
                return new RelayCommand(CambiarPassword);
            }
        }

        private async void CambiarPassword()
        {
            if (string.IsNullOrEmpty(this.ActualPassword))
            {
                this.IsRunning = false;
                this.IsEnabled = true;

                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Introduzca su Contraseña.",
                    "Aceptar");
                return;
            }

            if (this.ActualPassword.Length < 8
                || this.ActualPassword.Length > 20)
            {
                this.IsRunning = false;
                this.IsEnabled = true;

                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "La Contraseña debe tener entre 8 y 20 caracteres.",
                    "Aceptar");
                return;
            }

            if (!RegexUtilities.ValidarPassword(this.ActualPassword))
            {
                this.IsRunning = false;
                this.IsEnabled = true;

                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Las contraseñas deben tener al menos un carácter que no sea una letra ni un dígito, " +
                    "al menos una letra en minúscula ('a'-'z') y al menos una letra en mayúscula ('A'-'Z').",
                    "Aceptar");
                return;
            }

            if (this.ActualPassword != MainViewModel.GetInstance().Componente.Password)
            {
                this.IsRunning = false;
                this.IsEnabled = true;

                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "La contraseña introducida como actual no es valida.",
                    "Aceptar");
                return;
            }

            if (string.IsNullOrEmpty(this.NuevoPassword))
            {
                this.IsRunning = false;
                this.IsEnabled = true;

                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Introduzca su nueva Contraseña.",
                    "Aceptar");
                return;
            }

            if (this.NuevoPassword.Length < 8
                || this.NuevoPassword.Length > 20)
            {
                this.IsRunning = false;
                this.IsEnabled = true;

                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "La nueva Contraseña debe tener entre 8 y 20 caracteres.",
                    "Aceptar");
                return;
            }

            if (!RegexUtilities.ValidarPassword(this.NuevoPassword))
            {
                this.IsRunning = false;
                this.IsEnabled = true;

                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Las contraseñas deben tener al menos un carácter que no sea una letra ni un dígito, " +
                    "al menos una letra en minúscula ('a'-'z') y al menos una letra en mayúscula ('A'-'Z').",
                    "Aceptar");
                return;
            }

            if (this.NuevoPassword != this.Confirmacion)
            {
                this.IsRunning = false;
                this.IsEnabled = true;

                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "La nueva Contraseña y la confirmación no coinciden.",
                    "Aceptar");
                return;
            }

            IsRunning = true;
            IsEnabled = false;


        }
        #endregion
    }
}
