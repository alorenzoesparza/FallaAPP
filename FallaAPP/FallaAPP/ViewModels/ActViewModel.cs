using FallaAPP.Models;
using System.Collections.ObjectModel;

namespace FallaAPP.ViewModels
{
    public class ActViewModel : BaseViewModel
    {
        #region Propiedades
        public Act Act { get; set; }
        #endregion

        #region Constructor
        public ActViewModel(Act act)
        {
            this.Act = act;
        }
        #endregion
    }
}
