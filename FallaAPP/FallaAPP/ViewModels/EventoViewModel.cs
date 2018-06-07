using FallaAPP.Models;
using System.Collections.ObjectModel;

namespace FallaAPP.ViewModels
{
    public class EventoViewModel : BaseViewModel
    {
        #region Propiedades
        public Evento Evento { get; set; }
        #endregion

        #region Constructor
        public EventoViewModel(Evento Evento)
        {
            this.Evento = Evento;
        }
        #endregion
    }
}
