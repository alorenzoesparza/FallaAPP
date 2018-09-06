using FallaAPP.Models;
using FallaAPP.Services;
using FallaAPP.Views;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace FallaAPP.ViewModels
{
    public class EventoViewModel : BaseViewModel
    {
        #region Servicios
        private ApiService apiService;
        #endregion

        #region Atributos
        public string textoBoton;
        public string colorBoton;
        public bool isRefreshing;
        public Evento evento;
        public List<Evento> EventosList;
        #endregion

        #region Propiedades
        public ObservableCollection<EventoItemViewModel> Eventos { get; set; }

        public Evento Evento
        {
            get { return this.evento; }
            set { SetValue(ref this.evento, value); }
        }

        public string TextoBoton
        {
            get { return this.textoBoton; }
            set { SetValue(ref this.textoBoton, value); }
        }

        public string ColorBoton
        {
            get { return this.colorBoton; }
            set { SetValue(ref this.colorBoton, value); }
        }

        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { SetValue(ref this.isRefreshing, value); }
        }
        #endregion

        #region Constructor
        public EventoViewModel(Evento Evento)
        {
            this.apiService = new ApiService();

            this.Evento = Evento;

            if (Evento.Apuntado)
            {
                this.TextoBoton = "Quitar";
                this.ColorBoton = "#A6212C";
            }
            else
            {
                this.TextoBoton = "Apuntar";
                // Otro verde #3D5C3A
                this.ColorBoton = "#638C3C";
            }
        }
        #endregion

        #region Comandos
        public ICommand SelectBotonCommand
        {
            get
            {
                return new RelayCommand(SelectBoton);
            }
        }

        private void SelectBoton()
        {
            var color = ColorBoton;
            var textoBoton = TextoBoton;
            if (textoBoton == "Quitar")
            {
                this.Delete();
            }

            if (textoBoton == "Apuntar")
            {
                Apuntar();
            }
        }
        #endregion

        #region Metodos
        public async void Apuntar()
        {
            var connection = await this.apiService.CheckConnection();

            if (!connection.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    connection.Message,
                    "Accept");
                return;
            }

            var asistenciaEvento = new AsistenciaEvento
            {
                AsistenciaEventoId = 0,
                ComponenteId = MainViewModel.GetInstance().Componente.ComponenteId,
                EsInfantil = MainViewModel.GetInstance().Componente.EsInfantil,
                IdEvento = Evento.IdEvento,
                Precio = Evento.Precio,
            };

            var mainViewModel = MainViewModel.GetInstance();

            var apiBase = Application.Current.Resources["APIBase"].ToString();

            var response = await this.apiService.Post(
                apiBase,
                "/api",
                "/AsistenciaEventos",
                mainViewModel.Token.TokenType,
                mainViewModel.Token.AccessToken,
                asistenciaEvento);

            if (!response.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Aceptar");

                Application.Current.MainPage = new MasterPage();
                return;
            }

            Evento.AsistenciaEventoId = response.Result.GetHashCode();
            Evento.Apuntado = true;

            var response2 = await this.apiService.GetList<Evento>(
                apiBase,
                "/api",
                "/Eventos",
                mainViewModel.Token.TokenType,
                mainViewModel.Token.AccessToken,
                mainViewModel.Componente.ComponenteId);

            this.EventosList = (List<Evento>)response2.Result;

            this.Eventos = new ObservableCollection<EventoItemViewModel>(
                ToItemViewModel());


            var result = response.Result;

            this.TextoBoton = "Quitar";
            this.ColorBoton = "#A6212C";
        }

        public async void Delete()
        {
            var connection = await this.apiService.CheckConnection();

            if (!connection.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    connection.Message,
                    "Accept");
                return;
            }

            var mainViewModel = MainViewModel.GetInstance();

            var asistenciaEvento = new AsistenciaEvento
            {
                AsistenciaEventoId = Evento.AsistenciaEventoId,
                ComponenteId = mainViewModel.Componente.ComponenteId,
                EsInfantil = mainViewModel.Componente.EsInfantil,
                IdEvento = Evento.IdEvento,
                Precio = Evento.Precio,
            };

            var apiBase = Application.Current.Resources["APIBase"].ToString();
            var response = await this.apiService.Delete(
                apiBase,
                "/api",
                "/AsistenciaEventos",
                mainViewModel.Token.TokenType,
                mainViewModel.Token.AccessToken,
                asistenciaEvento);

            if (!response.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Aceptar");

                Application.Current.MainPage = new MasterPage();
                return;
            }

            Evento.Apuntado = false;
            Evento.AsistenciaEventoId = 0;

            var response2 = await this.apiService.GetList<Evento>(
                apiBase,
                "/api",
                "/Eventos",
                mainViewModel.Token.TokenType,
                mainViewModel.Token.AccessToken,
                mainViewModel.Componente.ComponenteId);

            this.EventosList = (List<Evento>)response2.Result;

            this.Eventos = new ObservableCollection<EventoItemViewModel>(
                ToItemViewModel());

            mainViewModel.Evento = new EventoViewModel(Evento);

            this.TextoBoton = "Apuntar";
            // Otro verde #3D5C3A
            this.ColorBoton = "#638C3C";
        }

        private IEnumerable<EventoItemViewModel> ToItemViewModel()
        {
            return this.EventosList.Select(a => new EventoItemViewModel
            {
                Apuntado = a.Apuntado,
                AsistenciaEventoId = a.AsistenciaEventoId,
                EventoOficial = a.EventoOficial,
                Descripcion = a.Descripcion,
                FechaEvento = a.FechaEvento,
                HoraEvento = a.HoraEvento,
                IdEvento = a.IdEvento,
                Imagen = a.Imagen,
                Imagen500 = a.Imagen500,
                PagInicio = a.PagInicio,
                Precio = a.Precio,
                PrecioInfantiles = a.PrecioInfantiles,
                Titular = a.Titular,
                YaEfectuado = a.YaEfectuado,
            });
        }
        #endregion
    }
}
