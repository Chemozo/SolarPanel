using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Prism.Commands;
using SolarPanel.Model;
using SolarPanel.ViewModel.Commands;
using SolarPanel.ViewModel.Helpers;
using SolarPanel.ViewModel.Navigation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SolarPanel.Model.CurrentWeather;
using static SolarPanel.Model.SunPosition;


namespace SolarPanel.ViewModel
{
    public class StatusVM : ViewModelBase
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
        public DelegateCommand LoadedCommand { get; set; }

        private Weather weather;

        public Weather Weather
        {
            get { return weather; }
            set 
            { 
                weather = value;
                RaisePropertyChanged();
            }
        }

        private SystemMonitor systemMonitor;

        public SystemMonitor SystemMonitor
        {
            get { return systemMonitor; }
            set 
            { 
                systemMonitor = value;
                RaisePropertyChanged();
            }
        }

        private Position position;

        public Position Position
        {
            get { return position; }
            set 
            { 
                position = value;
                RaisePropertyChanged();
            }
        }

        private int maxHumidity;

        public int MaxHumidity
        {
            get { return Properties.Settings.Default.MaxHumidity; }
            set 
            { 
                maxHumidity = value;
                RaisePropertyChanged();
            }
        }

        private int maxTemperature;

        public int MaxTemperature
        {
            get { return Properties.Settings.Default.MaxTemperature; }
            set 
            { 
                maxTemperature = value;
                RaisePropertyChanged();
            }
        }


        public StatusVM(IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;

            LoadedCommand = new DelegateCommand(RunAsyncMethods);
            

            

            Console.WriteLine(Properties.Settings.Default.MaxTemperature);
            

            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
            {
                weather = new Weather
                {
                    RelativeHumidity = 5
                };

                systemMonitor = new SystemMonitor
                {
                    CPUTemperture = 10
                };

                position = new Position
                {
                    Altitude = 40.55
                };
                maxHumidity = Properties.Settings.Default.MaxHumidity;
            }
        }

        
        public async void RunAsyncMethods()
        {
            GetCPUTemperature();
            GetCurrentConditions();
            await Task.Delay(5000);
            MovePanel();
        }

        public async void GetCurrentConditions()
        {
            //Weather = await AccuWeatherAPI.GetHumidity();
            Weather = new Weather
            {
                RelativeHumidity = 5
            };
            await Task.Delay(30000);
            GetCurrentConditions();
        }
        
        public async void GetCPUTemperature()
        {
            SystemMonitor = await SystemMonitorHelper.GetTemperature();
            await Task.Delay(10000);
            GetCPUTemperature();
        }

        public async void MovePanel()
        {
            if (Weather.RelativeHumidity < MaxHumidity && SystemMonitor.CPUTemperture < MaxTemperature)
            {
                Position sunPosition = await PanelAngle.CalculateSunPosition(DateTime.Now, 194326, -991332);
                if (sunPosition.Altitude > 20 && sunPosition.Altitude < 160)
                {
                    Position aux = await PanelAngle.CalculateSunPosition(DateTime.Now, 194326, -991332);
                    aux.Altitude = Math.Round(aux.Altitude, 2);
                    Position = aux;
                    Console.WriteLine(Position.Altitude);
                }
                else if (sunPosition.Altitude < 20)
                {
                    Position = new Position
                    {
                        Altitude = 160
                    };
                }
            }
            await Task.Delay(5000);
            MovePanel();
        }
    }
}
