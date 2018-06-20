using FallaAPP.Helpers;
using FallaAPP.Models;
using FallaAPP.Services;
using FallaAPP.ViewModels;
using FallaAPP.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace FallaAPP
{
    public partial class App : Application
	{
        #region Propiedades
        public static NavigationPage Navigator { get; internal set; }
        #endregion

        #region Constructor
        public App()
        {
            InitializeComponent();

            if (string.IsNullOrEmpty(Settings.Token))
            {
                MainPage = new NavigationPage(new LoginPage());
            }
            else
            {
                var dataService = new DataService();
                var componente = dataService.First<ComponenteLocal>(false);
                
                var mainViewModel = MainViewModel.GetInstance();
                mainViewModel.Token = Settings.Token;
                mainViewModel.TokenType = Settings.TokenType;
                mainViewModel.Componente = componente;
                mainViewModel.Eventos = new EventosViewModel();

                MainPage = new MasterPage();
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
