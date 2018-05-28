using FallaAPP.Models;
using FallaAPP.Services;
using FallaAPP.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace FallaAPP.ViewModels
{
    public class ActsViewModel : BaseViewModel
    {
        #region Servicios
        private ApiService apiService;
        #endregion

        #region Atributos
        private ObservableCollection<Act> acts;
        private bool isRefreshing;
        #endregion

        #region Propiedades
        public ObservableCollection<Act> Acts
        {
            get { return this.acts; }
            set { SetValue(ref this.acts, value); }
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

            var mainViewModel = MainViewModel.GetInstance();
            var response = await this.apiService.GetList<Act>(
                mainViewModel.BaseUrl,
                "/api",
                "/Acts",
                mainViewModel.TokenType,
                mainViewModel.Token);

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

            var list = (List<Act>)response.Result;
            this.Acts = new ObservableCollection<Act>(list);

            await Application.Current.MainPage.Navigation.PushAsync(new ActsPage());
            this.IsRefreshing = false;
        }
        #endregion
    }
}
