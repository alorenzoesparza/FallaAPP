using FallaAPP.Helpers;
using FallaAPP.Models;
using FallaAPP.Services;
using FallaAPP.ViewModels;
using FallaAPP.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace FallaAPP
{
    public partial class App : Application
	{
        #region Propiedades
        public static NavigationPage Navigator { get; internal set; }
        public static MasterPage Master { get; internal set; }
        #endregion

        #region Constructor
        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTUyOEAzMTM2MmUzMjJlMzBBSThRWGFtUlBydHF1Zy9NWkJEMno0ai9HZENvRTZHcmNua0NVeE1FQ1ZVPQ==;MTUyOUAzMTM2MmUzMjJlMzBiWWEyK0VOWDVMRjI5ZTdvSHZYRVUyZVY4d3NMN2owR25xRTh2aHhzTHVrPQ==");

            InitializeComponent();

            if (Settings.IsRemembered == "true")
            {
                var dataService = new DataService();
                var componente = dataService.First<ComponenteLocal>(false);
                var token = dataService.First<TokenResponse>(false);

                if (token != null && token.Expires > DateTime.Now)
                {
                    var mainViewModel = MainViewModel.GetInstance();
                    mainViewModel.Token = token;
                    mainViewModel.Componente = componente;
                    mainViewModel.Eventos = new EventosViewModel();

                    MainPage = new MasterPage();
                }
                else
                {
                    MainPage = new NavigationPage(new LoginPage());
                }

            }
            else
            {
                MainPage = new NavigationPage(new LoginPage());
            }
        }
        #endregion

        #region Metodos
        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
        #endregion
    }
}
