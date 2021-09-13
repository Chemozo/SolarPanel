/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:SolarPanel"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
//using Microsoft.Practices.ServiceLocation;
using CommonServiceLocator;
using SolarPanel.ViewModel.Navigation;
using System;

namespace SolarPanel.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<StatusVM>();
            SimpleIoc.Default.Register<SettingsVM>();

            //SimpleIoc.Default.Register<MainViewModel>();
            SetupNavigation();
        }

        private static void SetupNavigation()
        {
            var navigationService = new FrameNavigationService();
            navigationService.Configure("StatusView", new Uri("../View/Status.xaml", UriKind.Relative));
            navigationService.Configure("SettingsView", new Uri("../View/Settings.xaml", UriKind.Relative));

            SimpleIoc.Default.Register<IFrameNavigationService>(() => navigationService);
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }
        
        public SettingsVM SettingsVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SettingsVM>();
            }
        }

        public StatusVM StatusVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<StatusVM>();
            }
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}