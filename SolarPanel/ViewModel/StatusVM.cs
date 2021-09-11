using Prism.Commands;
using SolarPanel.Model;
using SolarPanel.ViewModel.Commands;
using SolarPanel.ViewModel.Helpers;
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
    class StatusVM : INotifyPropertyChanged
    {
        public DelegateCommand LoadedCommand { get; set; }

        private Weather weather;

        public Weather Weather
        {
            get { return weather; }
            set 
            { 
                weather = value;
                OnPropertyChanged("Weather");
            }
        }

        private SystemMonitor systemMonitor;

        public SystemMonitor SystemMonitor
        {
            get { return systemMonitor; }
            set 
            { 
                systemMonitor = value;
                OnPropertyChanged("SystemMonitor");
            }
        }

        private Position position;

        public Position Position
        {
            get { return position; }
            set 
            { 
                position = value;
                OnPropertyChanged("Position");
            }
        }

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


        public StatusVM()
        {
            LoadedCommand = new DelegateCommand(RunAsyncMethods);
            

            maxHumidity = Properties.Settings.Default.MaxHumidity;
            maxTemperature = Properties.Settings.Default.MaxTemperature;

            
            

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

        public event PropertyChangedEventHandler PropertyChanged;

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

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
