using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SolarPanel.ViewModel.Navigation;

namespace SolarPanel.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>

    public class MainViewModel : ViewModelBase
    {
        private IFrameNavigationService _navigationService;
        private RelayCommand _loadedCommand;            
        public RelayCommand LoadedCommand
        {
            get
            {
                return _loadedCommand
                    ?? (_loadedCommand = new RelayCommand(
                    () =>
                    {
                        _navigationService.NavigateTo("StatusView");
                    }));
            }
        }

        public MainViewModel(IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;
        }
        
    }
}   