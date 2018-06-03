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
    public class ActItemViewModel : Act
    {
        #region Comandos
        public ICommand SelectActCommand
        {
            get
            {
                return new RelayCommand(SelectAct);
            }
        }

        private async void SelectAct()
        {
            MainViewModel.GetInstance().Act = new ActViewModel(this);
            await App.Navigator.PushAsync(new ActPage());
            return;
        }
        #endregion
    }
}
