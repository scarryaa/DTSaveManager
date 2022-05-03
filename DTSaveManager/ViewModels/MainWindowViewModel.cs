using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using DTSaveManager.DataTypes.Enums;
using DTSaveManager.Services;
using DTSaveManager.Services.Interfaces;
using DTSaveManager.ViewModels.Base;

namespace DTSaveManager.ViewModels
{
    class MainWindowViewModel : ViewModelBase
    {
        private TreeViewModel _demonTurfTreeViewModel;
        private TreeViewModel _neonSplashTreeViewModel;
        private ViewModelType _currentViewModel;
        private string _copyDirectoryPathText = "Copy path";
        private IClipboardService _clipboardService { get; }

        private TimerService _timerService;

        public MainWindowViewModel(IClipboardService clipboardService)
        {
            _timerService = new TimerService();
            _clipboardService = clipboardService;

            DemonTurfTreeViewModel = new TreeViewModel(isNeonMode: false);
            NeonSplashTreeViewModel = new TreeViewModel(isNeonMode: true);

            CopyDirectoryPathCommand = new RelayCommand(c => CopyDirectoryPath((ViewModelType)c));
            ChangeStyleCommand = new RelayCommand(c => ChangeStyle());
        }

        public ICommand CopyDirectoryPathCommand { get; set; }
        public ICommand ChangeStyleCommand { get; set; }

        public string WindowTitle { get; set; } = "Demon Turf Save Manager";

        public TreeViewModel DemonTurfTreeViewModel
        {
            get { return _demonTurfTreeViewModel; }
            set
            {
                _demonTurfTreeViewModel = value;
                OnPropertyChanged();
            }
        }

        public TreeViewModel NeonSplashTreeViewModel
        {
            get { return _neonSplashTreeViewModel; }
            set
            {
                _neonSplashTreeViewModel = value;
                OnPropertyChanged();
            }
        }

        public ViewModelType CurrentViewModel
        {
            get { return _currentViewModel; }
            set
            {
                _currentViewModel = value;
                OnPropertyChanged();
            }
        }

        public string CopyDirectoryPathText
        {
            get { return _copyDirectoryPathText; }
            set
            {
                _copyDirectoryPathText = value;
                OnPropertyChanged();
            }
        }

        private void CopyDirectoryPath(ViewModelType vmType)
        {
            if (vmType == ViewModelType.DemonTurf) _clipboardService.SetText(DemonTurfTreeViewModel.GetDirectory());
            else if (vmType == ViewModelType.NeonSplash) _clipboardService.SetText(NeonSplashTreeViewModel.GetDirectory());
            CopyDirectoryPathText = "Path copied!";
            _timerService.DoTaskAfterDelay(3000, new Action(() => CopyDirectoryPathText = "Copy path"));
        }

        private void ChangeStyle()
        {
            ThemeService.ChangeTheme();
        }
    }
}
