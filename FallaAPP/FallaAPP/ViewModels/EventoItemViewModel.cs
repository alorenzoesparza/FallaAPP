using FallaAPP.Models;
using FallaAPP.Views;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace FallaAPP.ViewModels
{
    public class EventoItemViewModel : Evento
    {
        #region Comandos
        public ICommand SelectEventoCommand
        {
            get
            {
                return new RelayCommand(SelectEvento);
            }
        }

        private async void SelectEvento()
        {
            MainViewModel.GetInstance().Evento = new EventoViewModel(this);
            await App.Navigator.PushAsync(new EventoPage());
            return;
        }
        #endregion
    }
}
