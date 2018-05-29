using FallaAPP.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FallaAPP.ViewModels
{
    public class ActViewModel
    {
        #region Propiedades
        //private ActItemViewModel actItemViewModel;
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
