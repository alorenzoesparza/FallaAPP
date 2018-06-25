using FallaAPP.Models;
using System;
using System.Collections.ObjectModel;

namespace FallaAPP.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        #region Atributos
        private ComponenteLocal componente;
        #endregion

        #region ViewModels
        public LoginViewModel Login { get; set; }
        public EventosViewModel Eventos { get; set; }
        public EventoViewModel Evento { get; set; }
        public RegistroViewModel Registro { get; set; }
        public MiPerfilViewModel MiPerfil { get; set; }
        public CambiarPasswordViewModel CambiarPassword { get; set; }
        #endregion

        #region Propiedades
        public TokenResponse Token { get; set; }
        public ObservableCollection<MenuItemViewModel> Menus { get; set; }
        public ComponenteLocal Componente
        {
            get { return this.componente; }
            set { SetValue(ref this.componente, value); }
        }
        #endregion

        #region Constructores
        public MainViewModel()
        {
            instance = this;

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
                NombrePagina = "MiPerfilPage",
                Titulo = "Mí Perfil",
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
