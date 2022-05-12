using System;
using System.Windows.Input;
using DTSaveManager.DataTypes.Enums;
using DTSaveManager.Services;
using DTSaveManager.Services.Interfaces;
using DTSaveManager.ViewModels.Base;
using DTSaveManager.Views.Custom_Controls;

namespace DTSaveManager.ViewModels
{
    class MainWindowViewModel : ViewModelBase
    {
        private ThemeType _currentTheme = ThemeService.GetCurrentTheme();
        private bool _neonModeDisabled = ConfigService.GetNeonSplashDisabled().Value;
        private TreeViewModel _demonTurfTreeViewModel;
        private TreeViewModel _neonSplashTreeViewModel;
        private ViewModelType _currentViewModel;
        private string _copyDirectoryPathText = "Copy path";
        private IClipboardService _clipboardService { get; }

        private TimerService _timerService;

        public MainWindowViewModel(IClipboardService clipboardService)
        {
            ThemeService.SetTheme(_currentTheme);
            _timerService = new TimerService();
            _clipboardService = clipboardService;

            DemonTurfTreeViewModel = new TreeViewModel(isNeonMode: false);
            NeonSplashTreeViewModel = new TreeViewModel(isNeonMode: true);

            CopyDirectoryPathCommand = new RelayCommand(c => CopyDirectoryPath((ViewModelType)c));
            ChangeStyleCommand = new RelayCommand(c => ChangeStyle());
            OptionsCommand = new RelayCommand(c => OpenOptions());
        }

        public ICommand CopyDirectoryPathCommand { get; set; }
        public ICommand ChangeStyleCommand { get; set; }
        public ICommand OptionsCommand { get; set; }

        public string WindowTitle { get; set; } = "Demon Turf Save Manager";

        public ThemeType CurrentTheme
        {
            get { return _currentTheme; }
            set
            {
                _currentTheme = value;
                OnPropertyChanged();
            }
        }

        public bool NeonModeDisabled
        {
            get { return _neonModeDisabled; }
            set
            {
                _neonModeDisabled = value;
                OnPropertyChanged();
            }
        }

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
            CurrentTheme = ThemeService.GetCurrentTheme();
        }

        private void OpenOptions()
        {
            PopupBox.Show("Could not find Demon Turf install directory. Please ensure that steam or GOG is running and the game is installed, or select folders manually.");
        }
    }
}
