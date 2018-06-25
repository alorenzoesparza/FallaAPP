using FallaAPP.Helpers;
using FallaAPP.Views;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using Xamarin.Forms;

namespace FallaAPP.ViewModels
{
    public class MenuItemViewModel
    {
        #region Propiedades
        public string Icono { get; set; }
        public string Titulo { get; set; }
        public string NombrePagina { get; set; }
        #endregion

        #region Comandos
        public ICommand NavigateCommand
        {
            get
            {
                return new RelayCommand(Navigate);
            }
        }

        private void Navigate()
        {
            App.Master.IsPresented = false;

            if (this.NombrePagina == "LoginPage")
            {
                Settings.IsRemembered = "false";
                var mainViewModel = MainViewModel.GetInstance();
                mainViewModel.Token = null;
                mainViewModel.Componente = null;

                Application.Current.MainPage = new NavigationPage(new LoginPage());
            }else if(this.NombrePagina == "MiPerfilPage")
            {
                MainViewModel.GetInstance().MiPerfil = new MiPerfilViewModel();
                App.Navigator.PushAsync(new MiPerfilPage());
            }
        }
        #endregion
    }
}
