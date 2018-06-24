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
    public class MiPerfilViewModel : BaseViewModel
    {
        #region Servicios
        private ApiService apiService;
        private DataService dataService;
        #endregion

        #region Atributos
        private bool isRunning;
        private bool isEnabled;
        private ImageSource imageSource;
        private MediaFile file;
        #endregion

        #region Propiedades
        public ComponenteLocal Componente { get; set; }
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
        #endregion

        #region Constructores
        public MiPerfilViewModel()
        {
            this.apiService = new ApiService();
            this.dataService = new DataService();

            this.Componente = MainViewModel.GetInstance().Componente;
            this.ImageSource = this.Componente.FotoFullPath;
            this.isEnabled = true;
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
            if (string.IsNullOrEmpty(this.Componente.Nombre))
            {
                this.IsRunning = false;
                this.IsEnabled = true;

                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Introduzca su Nombre.",
                    "Aceptar");
                return;
            }

            if (string.IsNullOrEmpty(this.Componente.Apellidos))
            {
                this.IsRunning = false;
                this.IsEnabled = true;

                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Introduzca sus Apellidos.",
                    "Aceptar");
                return;
            }

            if (string.IsNullOrEmpty(this.Componente.Email))
            {
                this.IsRunning = false;
                this.IsEnabled = true;

                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Introduzca su Email.",
                    "Aceptar");
                return;
            }

            if (!RegexUtilities.ValidarEmail(this.Componente.Email))
            {
                this.IsRunning = false;
                this.IsEnabled = true;

                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Introduzca un Email Valido.",
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
                    "Aceptar");
                return;
            }

            byte[] imagenArray = null;

            if (this.file != null)
            {
                imagenArray = FilesHelper.ReadFully(this.file.GetStream());
            }

            var componenteDomain = Converter.ToComponenteDomain(this.Componente, imagenArray);

            var apiBase = Application.Current.Resources["APIBase"].ToString();
            var response = await this.apiService.ModificarComponente(
                apiBase,
                "/api",
                "/Componentes/ModificarComponente",
                MainViewModel.GetInstance().TokenType,
                MainViewModel.GetInstance().Token,
                componenteDomain
                );

            if (!response.IsSuccess)
            {
                this.isRunning = false;
                this.isEnabled = true;

                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Aceptar");
                return;
            }

            await Application.Current.MainPage.DisplayAlert(
                "Confirmación",
                "Perfil modifícado correctamente.",
                "Aceptar");

            var componenteApi = await this.apiService.GetComponentePorEmail(
                apiBase,
                "/api",
                "/Componentes/GetComponentePorEmail",
                MainViewModel.GetInstance().TokenType,
                MainViewModel.GetInstance().Token,
                this.Componente.Email
                );
            
            var componenteLocal = Converter.ToComponenteLocal(componenteApi);

            MainViewModel.GetInstance().Componente = componenteLocal;
            this.dataService.Update(componenteLocal);

            this.isRunning = false;
            this.isEnabled = true;

            await App.Navigator.PopAsync();
        }
        #endregion
    }
}
