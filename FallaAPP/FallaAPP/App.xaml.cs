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

            MainPage = new NavigationPage(new LoginPage());
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
