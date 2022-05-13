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
        private TreeViewModel _demonTurfTreeViewModel;
        private TreeViewModel _neonSplashTreeViewModel;
        private bool _neonSplashDisabled;
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
            MainWindowContentRenderedCommand = new RelayCommand(c => MainWindowContentRendered());
        }

        public ICommand CopyDirectoryPathCommand { get; set; }
        public ICommand ChangeStyleCommand { get; set; }
        public ICommand OptionsCommand { get; set; }
        public ICommand MainWindowContentRenderedCommand { get; set; }

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

        public bool NeonSplashDisabled
        {
            get { return _neonSplashDisabled; }
            set
            {
                _neonSplashDisabled = value;
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
            var result = SettingsPage.Show();
            if (result.Item2 != null && result.Item2 != "No save directory found...")
                NeonSplashDisabled = false;
        }

        private void MainWindowContentRendered()
        {
            WindowService.Instance.SetMainWindowIsRendered();
            if (ConfigService.GetNeonSplashDisabled().HasValue)
                NeonSplashDisabled = ConfigService.GetNeonSplashDisabled().Value;
            else
                NeonSplashDisabled = false;
        }
    }
}
