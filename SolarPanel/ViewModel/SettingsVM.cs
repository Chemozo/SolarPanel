using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SolarPanel.ViewModel.Navigation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarPanel.ViewModel
{
    public class SettingsVM : ViewModelBase
    {
        //Navigation
        private IFrameNavigationService _navigationService;
        private string _myProperty = "MainPageText";
        public string MainPageText
        {
            get
            {
                return _myProperty;
            }

            set
            {
                if (_myProperty == value)
                {
                    return;
                }

                _myProperty = value;
                RaisePropertyChanged();
            }
        }
        
        private RelayCommand _status;

        public RelayCommand Status
        {
            get
            {
                return _status
                       ?? (_status = new RelayCommand(
                           () =>
                           {
                               _navigationService.NavigateTo("StatusView");
                           }));
            }
        }
        //Navigation end

        private int maxHumidity;

        public int MaxHumidity
        {
            get { return Properties.Settings.Default.MaxHumidity; }
            set
            {
                Properties.Settings.Default.MaxHumidity = value;
                Properties.Settings.Default.Save();
                RaisePropertyChanged();
            }
        }

        private int maxTemperature;

        public int MaxTemperature
        {
            get { return Properties.Settings.Default.MaxTemperature; }
            set
            {
                Properties.Settings.Default.MaxTemperature = value;
                Properties.Settings.Default.Save();
                RaisePropertyChanged();
            }
        }

        public SettingsVM(IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;
            
            //maxHumidity = Properties.Settings.Default.MaxHumidity;
            //maxTemperature = Properties.Settings.Default.MaxTemperature;
        }
    }
}
