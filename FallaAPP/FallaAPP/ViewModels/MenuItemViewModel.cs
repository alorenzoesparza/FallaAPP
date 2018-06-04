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
            if (this.NombrePagina == "LoginPage")
            {
                Settings.Token = string.Empty;
                Settings.TokenType = string.Empty;
                var mainViewModel = MainViewModel.GetInstance();
                mainViewModel.Token = string.Empty;
                mainViewModel.TokenType = string.Empty;

                Application.Current.MainPage = new LoginPage();
            }
        }
        #endregion
    }
}
