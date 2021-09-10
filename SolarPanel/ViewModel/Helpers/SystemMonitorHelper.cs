using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using SolarPanel.Model;

namespace SolarPanel.ViewModel.Helpers
{
    class SystemMonitorHelper
    {
        public static async Task<SystemMonitor> GetTemperature()
        {
            double averageTemperature = 0.0;
            int totalTemperatures = 0;
            SystemMonitor systemMonitor = new SystemMonitor();

            try
            {
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("root\\WMI",
                    "SELECT * FROM MSAcpi_ThermalZoneTemperature");

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    averageTemperature += (Convert.ToSingle(queryObj["CurrentTemperature"]) / 10) - 273.15;
                    totalTemperatures++;
                }
                averageTemperature /= totalTemperatures;
                systemMonitor.CPUTemperture = averageTemperature;
            }
            catch (ManagementException e)
            {
                //MessageBox.Show("An error occurred while querying for WMI data: " + e.Message);
                Console.WriteLine(e.Message);

            }
            return systemMonitor;
        }
    }
}
