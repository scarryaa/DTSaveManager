using DTSaveManager.DataTypes.Enums;
using DTSaveManager.Services;
using DTSaveManager.ViewModels.Base;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DTSaveManager.ViewModels
{
    class SettingsWindowViewModel : ViewModelBase
    {
        private string _dtSaveDirectory;
        private string _neonSaveDirectory;
        private bool _neonDirectoryFound;
        private ThemeType _currentTheme = ThemeService.GetCurrentTheme();

        public SettingsWindowViewModel()
        {
            DtSaveDirectory = ConfigService.GetDTSaveDirectory();
            NeonSaveDirectory = ConfigService.GetNSSaveDirectory();
            NeonDirectoryFound = true;

            if (NeonSaveDirectory == null)
            {
                NeonDirectoryFound = false;
                NeonSaveDirectory = "No save directory found...";
            }

            ChangeDTDirectoryCommand = new RelayCommand(c => ChangeDTDirectory());
            ChangeNSDirectoryCommand = new RelayCommand(c => ChangeNSDirectory());
            DetectDTDirectoryCommand = new RelayCommand(c => DetectDTDirectory());
            DetectNSDirectoryCommand = new RelayCommand(c => DetectNSDirectory());
            NavigateCommand = new RelayCommand(c => Navigate((string)c));
        }

        public ICommand ChangeDTDirectoryCommand { get; set; }
        public ICommand ChangeNSDirectoryCommand { get; set; }
        public ICommand DetectDTDirectoryCommand { get; set; }
        public ICommand DetectNSDirectoryCommand { get; set; }
        public ICommand NavigateCommand { get; set; }

        public string DtSaveDirectory
        {
            get { return _dtSaveDirectory; }
            set
            {
                _dtSaveDirectory = value;
                OnPropertyChanged();
            }
        }

        public string NeonSaveDirectory
        {
            get { return _neonSaveDirectory; }
            set
            {
                _neonSaveDirectory = value;
                OnPropertyChanged();
            }
        }

        public bool NeonDirectoryFound
        {
            get { return _neonDirectoryFound; }
            set
            {
                _neonDirectoryFound = value;
                OnPropertyChanged();
            }
        }

        public ThemeType CurrentTheme
        {
            get { return _currentTheme; }
            set
            {
                _currentTheme = value;
                OnPropertyChanged();
            }
        }

        public string WindowTitle { get; set; } = "Settings";

        private void ChangeDTDirectory()
        {
            OpenFileDialog dtFileDialog = new OpenFileDialog();
            {
                dtFileDialog.Filter = "DTSaveData.txt| *.txt";
                dtFileDialog.FileName = "DTSaveData.txt";
                dtFileDialog.FilterIndex = 1;
                dtFileDialog.Title = "Select a Demon Turf install directory...";

                if (dtFileDialog.ShowDialog() == true)
                {
                    string result = Path.GetDirectoryName(dtFileDialog.FileName);
                    ConfigService.SetDTSaveDirectory(result);
                    DtSaveDirectory = result;
                }
                else
                {
                    return;
                }
            }
        }

        private void ChangeNSDirectory()
        {
            OpenFileDialog neonFileDialog = new OpenFileDialog();
            {
                neonFileDialog.Filter = "DTSaveData.txt| *.txt";
                neonFileDialog.FileName = "DTSaveData.txt";
                neonFileDialog.FilterIndex = 1;
                neonFileDialog.Title = "Select a Neon Splash install directory...";

                if (neonFileDialog.ShowDialog() == true)
                {
                    string result = Path.GetDirectoryName(neonFileDialog.FileName);
                    ConfigService.SetNSSaveDirectory(result);
                    ConfigService.SetNeonSplashDisabled(false);
                    NeonDirectoryFound = true;
                    NeonSaveDirectory = result;
                }
                else
                {
                    return;
                }
            }
        }

        private void DetectDTDirectory()
        {
            string result = SaveMetadataService.Instance.GetPathFromRegistryKeys(false);
            if (result != null)
            {
                ConfigService.SetDTSaveDirectory(result);
                DtSaveDirectory = result;
            }
        }

        private void DetectNSDirectory()
        {
            string result = SaveMetadataService.Instance.GetPathFromRegistryKeys(true);
            if (result != null)
            {
                ConfigService.SetNSSaveDirectory(result);
                ConfigService.SetNeonSplashDisabled(false);
                NeonDirectoryFound = true;
                NeonSaveDirectory = result;
            }
        }

        private void Navigate(string website)
        {
            NavigationService.Navigate(website);
        }
    }
}
