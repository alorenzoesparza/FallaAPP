using FallaAPP.Models;
using System;
using System.Collections.ObjectModel;

namespace FallaAPP.ViewModels
{
    public class MainViewModel
    {
        #region ViewModels
        public LoginViewModel Login { get; set; }
        public ActsViewModel Acts { get; set; }
        //public ActViewModel Act { get; set; }
        #endregion

        #region Propiedades
        public string BaseUrl { get; set; }
        public string ApiUrl { get; set; }
        public string Token { get; set; }
        public string TokenType { get; set; }
        public ObservableCollection<MenuItemViewModel> Menus { get; set; }
        #endregion

        #region Constructores
        public MainViewModel()
        {
            instance = this;

            BaseUrl = "http://api.antoniole.com";
            ApiUrl = "/api";
            this.Login = new LoginViewModel();
            this.LoadMenu();
        }
        #endregion

        #region Metodos
        private void LoadMenu()
        {
            this.Menus = new ObservableCollection<MenuItemViewModel>();

            this.Menus.Add(new MenuItemViewModel
            {
                Icono = "ic_configuracion",
                NombrePagina = "ConfigurationPage",
                Titulo = "Configuración",
            });
            this.Menus.Add(new MenuItemViewModel
            {
                Icono = "ic_salir",
                NombrePagina = "LoginPage",
                Titulo = "Cerrar Sesión",
            });
        }

        #endregion
        #region Singleton
        private static MainViewModel instance;

        public static MainViewModel GetInstance()
        {
            if (instance == null)
            {
                return new MainViewModel();
            }

            return instance;
        }
        #endregion
    }
}
