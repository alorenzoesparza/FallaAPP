﻿using FallaAPP.Helpers;
using FallaAPP.Services;
using FallaAPP.Views;
using GalaSoft.MvvmLight.Command;
using System;
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

            this.Email = "mariam@gmail.com";
            this.Password = "Mariam1*";
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
                    "Debes introducir un correo electrónico.",
                    "Aceptar");
                return;
            }

            if (!RegexUtilities.ValidarEmail(this.Email))
            {
                this.IsRunning = false;
                this.IsEnabled = true;

                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Introduzca un Email Valido.",
                    "Aceptar");
                return;
            }

            if (string.IsNullOrEmpty(this.Password))
            {
                this.IsRunning = false;
                this.IsEnabled = true;

                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Debes introducir una contraseña.",
                    "Aceptar");
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
                    "Aceptar");
                return;
            }

            var apiBase = Application.Current.Resources["APIBase"].ToString();

            var token = await this.apiService.GetToken(
                apiBase,
                this.Email,
                this.Password);

            if (token == null)
            {
                this.IsRunning = false;
                this.IsEnabled = true;

                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "El servicio no está listo. Reintentetelo más tarde.",
                    "Aceptar");
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

            var componente = await this.apiService.GetComponentePorEmail(
                apiBase, 
                "/api",
                "/Componentes/GetComponentePorEmail", 
                this.Email);

            var mainViewModel = MainViewModel.GetInstance();

            mainViewModel.Token = token.AccessToken;
            mainViewModel.TokenType = token.TokenType;
            mainViewModel.Componente = componente;
            
            if (this.IsRemembered)
            {
                // Guardar en persistencia el Token
                Settings.Token = token.AccessToken;
                Settings.TokenType = token.TokenType;
            }

            mainViewModel.Eventos = new EventosViewModel();
            Application.Current.MainPage = new MasterPage();

            this.IsRunning = false;
            this.IsEnabled = true;
            this.Email = string.Empty;
            this.Password = string.Empty;
        }

        public ICommand RegistroCommand
        {
            get
            {
                return new RelayCommand(Registro);
            }
        }

        private async void Registro()
        {
            MainViewModel.GetInstance().Registro = new RegistroViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new RegistroPage());
        }
        #endregion
    }
}
