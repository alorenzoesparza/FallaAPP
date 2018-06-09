using FallaAPP.Models;
using FallaAPP.Services;
using FallaAPP.Views;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace FallaAPP.ViewModels
{
    public class EventosViewModel : BaseViewModel
    {
        #region Servicios
        private ApiService apiService;
        #endregion

        #region Atributos
        private ObservableCollection<EventoItemViewModel> eventos;
        private bool isRefreshing;
        private string filter;
        private List<Evento> EventosList;
        #endregion

        #region Propiedades
        public ObservableCollection<EventoItemViewModel> Eventos
        {
            get { return this.eventos; }
            set { SetValue(ref this.eventos, value); }
        }

        public string Filter
        {
            get { return this.filter; }
            set
            {
                SetValue(ref this.filter, value);
                this.Search();
            }
        }

        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { SetValue(ref this.isRefreshing, value); }
        }

        #endregion

        #region Constructor
        public EventosViewModel()
        {
            this.apiService = new ApiService();
            this.LoadEventos();
        }

        #endregion

        #region Metodos
        private async void LoadEventos()
        {
            this.IsRefreshing = true;
            var connection = await this.apiService.CheckConnection();

            if (!connection.IsSuccess)
            {
                this.isRefreshing = false;

                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    connection.Message,
                    "Accept");
                return;
            }

            var apiBase = Application.Current.Resources["APIBase"].ToString();

            var response = await this.apiService.GetList<Evento>(
                apiBase,
                "/api",
                "/Eventos",
                MainViewModel.GetInstance().TokenType,
                MainViewModel.GetInstance().Token);

            if (!response.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Aceptar");
                await Application.Current.MainPage.Navigation.PopAsync();
                return;
            }

            this.EventosList = (List<Evento>)response.Result;
            this.Eventos = new ObservableCollection<EventoItemViewModel>(
                this.ToItemViewModel());
           
            this.IsRefreshing = false;
        }
        #endregion

        #region Metodos
        private IEnumerable<EventoItemViewModel> ToItemViewModel()
        {
            return this.EventosList.Select(a => new EventoItemViewModel
            {
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

        #region Comandos
        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(LoadEventos);
            }
        }

        public ICommand SearchCommand
        {
            get
            {
                return new RelayCommand(Search);
            }
        }

        private void Search()
        {
            if (string.IsNullOrEmpty(this.Filter))
            {
                this.Eventos = new ObservableCollection<EventoItemViewModel>(
                    this.ToItemViewModel());
            }
            else
            {
                this.Eventos = new ObservableCollection<EventoItemViewModel>(
                    this.ToItemViewModel().
                    Where(
                        a => a.Descripcion.ToLower().Contains(this.Filter.ToLower()) || 
                             a.StrFecha.Contains(this.Filter)));
            }
        }
        #endregion
    }
}
