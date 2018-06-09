using FallaAPP.Helpers;
using FallaAPP.Models;
using FallaAPP.Services;
using GalaSoft.MvvmLight.Command;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace FallaAPP.ViewModels
{
    public class RegistroViewModel : BaseViewModel
    {
        #region Servicios
        private ApiService apiService;
        #endregion

        #region Atributos
        private bool isRunning;
        private bool isEnabled;
        private ImageSource imageSource;
        private MediaFile file;
        #endregion

        #region Propiedades
        public ImageSource ImageSource
        {
            get { return this.imageSource; }
            set { SetValue(ref this.imageSource, value); }
        }

        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set { SetValue(ref this.isEnabled, value); }
        }

        public bool IsRunning
        {
            get { return this.isRunning; }
            set { SetValue(ref this.isRunning, value); }
        }

        public string Nombre { get; set; }

        public string Apellidos { get; set; }

        public string Email { get; set; }

        public string Telefono { get; set; }

        public string Password { get; set; }

        public string Confirm { get; set; }
        #endregion

        #region Constructores
        public RegistroViewModel()
        {
            this.apiService = new ApiService();

            this.IsEnabled = true;
            this.ImageSource = "no_image";

            this.Registrarse();
        }
        #endregion

        #region Metodos
        public async void Registrarse()
        {
            byte[] imageArray = null;
            if (this.file != null)
            {
                imageArray = FilesHelper.ReadFully(this.file.GetStream());
            }

            var componente = new Componente
            {
                Apellidos = this.Apellidos,
                Email = this.Email,
                ImageArray = imageArray,
                Nombre = this.Nombre,
                Password = this.Password,
                Telefono = this.Telefono,
            };

        }
        #endregion

        #region Comandos
        public ICommand CambiarFotoCommand
        {
            get
            {
                return new RelayCommand(CambiarFoto);
            }
        }

        private async void CambiarFoto()
        {
            await CrossMedia.Current.Initialize();

            if (CrossMedia.Current.IsCameraAvailable &&
                CrossMedia.Current.IsTakePhotoSupported)
            {
                var source = await Application.Current.MainPage.DisplayActionSheet(
                    "Seleccionar Foto",
                    "Cancelar",
                    null,
                    "Desde Galeria",
                    "Desde Camara");

                if (source == "Cancelar")
                {
                    this.file = null;
                    return;
                }

                if (source == "Desde Camara")
                {
                    this.file = await CrossMedia.Current.TakePhotoAsync(
                        new StoreCameraMediaOptions
                        {
                            Directory = "Sample",
                            Name = "text.jpg",
                            PhotoSize = PhotoSize.Small,
                        }
                    );
                }
                else
                {
                    this.file = await CrossMedia.Current.PickPhotoAsync();
                }
            }
            else
            {
                this.file = await CrossMedia.Current.PickPhotoAsync();
            }

            if (this.file != null)
            {
                this.ImageSource = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    return stream;
                });
            }
        }

        public ICommand SalvarCommand
        {
            get
            {
                return new RelayCommand(Salvar);
            }
        }

        private async void Salvar()
        {
            if (string.IsNullOrEmpty(this.Nombre))
            {
                this.IsRunning = false;
                this.IsEnabled = true;

                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Introduzca su Nombre.",
                    "Aceptar");
                return;
            }

            if (string.IsNullOrEmpty(this.Apellidos))
            {
                this.IsRunning = false;
                this.IsEnabled = true;

                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Introduzca sus Apellidos.",
                    "Aceptar");
                return;
            }

            if (string.IsNullOrEmpty(this.Email))
            {
                this.IsRunning = false;
                this.IsEnabled = true;

                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Introduzca su Email.",
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
                    "Introduzca su Contraseña.",
                    "Aceptar");
                return;
            }

            if (this.Password.Length < 8 
                || this.Password.Length > 20)
            {
                this.IsRunning = false;
                this.IsEnabled = true;

                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "La Contraseña debe tener entre 8 y 20 caracteres.",
                    "Aceptar");
                return;
            }

            if (!RegexUtilities.ValidarPassword(this.Password))
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

            if (this.Password != this.Confirm)
            {
                this.IsRunning = false;
                this.IsEnabled = true;

                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Su Contraseña y la confirmación no coinciden.",
                    "Aceptar");
                return;
            }

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

            byte[] imagenArray = null;

            if (this.file != null)
            {
                imagenArray = FilesHelper.ReadFully(this.file.GetStream());
            }

            var componente = new Componente
            {
                Apellidos = this.Apellidos,
                Email = this.Email,
                ImageArray = imagenArray,
                Nombre = this.Nombre,
                Password = this.Password,
                Telefono = this.Telefono,
            };

            var apiBase = Application.Current.Resources["APIBase"].ToString();
            var response = await this.apiService.Post(
                apiBase,
                "/api",
                "/Componentes",
                componente
                );
        }
        #endregion
    }
}
