using FallaAPP.ViewModels;

namespace FallaAPP.Infrastructure
{
    public class InstanceLocator
    {
        #region Propiedades
        public MainViewModel Main { get; set; }

        #endregion

        #region Constructores
        public InstanceLocator()
        {
            this.Main = new MainViewModel();
        }
        #endregion
    }
}
