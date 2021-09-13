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
        private RelayCommand _page1Command;
        public RelayCommand Page1Command
        {
            get
            {
                return _page1Command
                    ?? (_page1Command = new RelayCommand(
                    () =>
                    {
                        _navigationService.NavigateTo("SettingsView");
                    }));
            }
        }
        private RelayCommand _page2Command;

        public RelayCommand Page2Command
        {
            get
            {
                return _page2Command
                       ?? (_page2Command = new RelayCommand(
                           () =>
                           {
                               _navigationService.NavigateTo("Page2");
                           }));
            }
        }
        //Navigation end

        private int maxHumidity;

        public int MaxHumidity
        {
            get { return maxHumidity; }
            set
            {
                maxHumidity = value;
                OnPropertyChanged("MaxHumidity");
            }
        }

        private int maxTemperature;

        public int MaxTemperature
        {
            get { return maxTemperature; }
            set
            {
                maxTemperature = value;
                OnPropertyChanged("MaxTemperature");
            }
        }

        public SettingsVM(IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;

            maxHumidity = Properties.Settings.Default.MaxHumidity;
            maxTemperature = Properties.Settings.Default.MaxTemperature;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
