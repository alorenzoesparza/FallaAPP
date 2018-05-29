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
    public class ActsViewModel : BaseViewModel
    {
        #region Servicios
        private ApiService apiService;
        #endregion

        #region Atributos
        private ObservableCollection<ActItemViewModel> acts;
        private bool isRefreshing;
        private string filter;
        private List<Act> actsList;
        #endregion

        #region Propiedades
        public ObservableCollection<ActItemViewModel> Acts
        {
            get { return this.acts; }
            set { SetValue(ref this.acts, value); }
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
        public ActsViewModel()
        {
            this.apiService = new ApiService();
            this.LoadActs();
        }

        #endregion

        #region Metodos
        private async void LoadActs()
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

            
            var response = await this.apiService.GetList<Act>(
                MainViewModel.GetInstance().BaseUrl,
                "/api",
                "/Acts",
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

            this.actsList = (List<Act>)response.Result;
            this.Acts = new ObservableCollection<ActItemViewModel>(
                this.ToItemViewModel());

            //await Application.Current.MainPage.Navigation.PushAsync(new ActsPage());
            this.IsRefreshing = false;
        }
        #endregion

        #region Metodos
        private IEnumerable<ActItemViewModel> ToItemViewModel()
        {
            return this.actsList.Select(a => new ActItemViewModel
            {
                ActoOficial = a.ActoOficial,
                Descripcion = a.Descripcion,
                FechaActo = a.FechaActo,
                HoraActo = a.HoraActo,
                IdAct = a.IdAct,
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
                return new RelayCommand(LoadActs);
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
                this.Acts = new ObservableCollection<ActItemViewModel>(
                    this.ToItemViewModel());
            }
            else
            {
                this.Acts = new ObservableCollection<ActItemViewModel>(
                    this.ToItemViewModel().
                    Where(
                        a => a.Descripcion.ToLower().Contains(this.Filter.ToLower()) || 
                             a.StrFecha.Contains(this.Filter)));
            }
        }
        #endregion
    }
}
