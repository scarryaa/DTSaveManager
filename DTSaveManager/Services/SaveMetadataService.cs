﻿using DTSaveManager.Models;
using DTSaveManager.Views.Custom_Controls;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;

namespace DTSaveManager.Services
{
    public class SaveMetadataService
    {
        private string _dtSaveDirectory;
        private string _neonSaveDirectory;
        private const string DTSM_FOLDER_NAME = "_dtsm";
        private const string SAVE_FILE_NAME = "DTSaveData.txt";
        public static Dictionary<string, int> DtSaveMetadata = new Dictionary<string, int>();
        public static Dictionary<string, int> NeonSplashSaveMetadata = new Dictionary<string, int>();
        public static string DtActiveFile;
        public static string NeonSplashActiveFile;

        static SaveMetadataService() { Instance = new SaveMetadataService(); }

        public static SaveMetadataService Instance { get; private set; }

        private SaveMetadataService()
        {
            // run initial setup functions
            // errors if steam is in offline mode?
            _dtSaveDirectory = ConfigService.GetDTSaveDirectory();
            if (_dtSaveDirectory == null)
            {
                _dtSaveDirectory = GetPathFromRegistryKeys(neonSplash: false);
                ConfigService.SetDTSaveDirectory(_dtSaveDirectory);
            }

            _neonSaveDirectory = ConfigService.GetNSSaveDirectory();
            if (_neonSaveDirectory == null && !ConfigService.GetNeonSplashDisabled().HasValue)
            {
                _neonSaveDirectory = GetPathFromRegistryKeys(neonSplash: true);
                ConfigService.SetNSSaveDirectory(_neonSaveDirectory);
            }

            if (_dtSaveDirectory != null)
            {
                InitializeFiles(false, _dtSaveDirectory, DtSaveMetadata);
                CopyActiveFile(_dtSaveDirectory, DtActiveFile);
            }

            if (_neonSaveDirectory != null)
            {
                InitializeFiles(true, _neonSaveDirectory, NeonSplashSaveMetadata);
                CopyActiveFile(_neonSaveDirectory, NeonSplashActiveFile);
            }
        }

        public string GetPathFromRegistryKeys(bool neonSplash)
        {
            string _steamActiveUser = "";
            string _steamInstallPath = "";

            try
            {
                string _steamInstallPathReg = @"SOFTWARE\WOW6432Node\Valve\Steam";
                RegistryKey _installKey = Registry.LocalMachine.OpenSubKey(_steamInstallPathReg);

                if (_installKey != null)
                {
                    Object o = _installKey.GetValue("InstallPath");
                    if (o != null)
                    {
                        _steamInstallPath = o.ToString();
                    }
                    else
                    {
                        throw new Exception(@"Registry Key 'Computer\HKEY_CURRENT_USER\SOFTWARE\WOW6432Node\Valve\Steam\InstallPath' has no value.");
                    }
                }

                string _steamActiveUserReg = @"SOFTWARE\Valve\Steam\ActiveProcess";
                RegistryKey _userKey = Registry.CurrentUser.OpenSubKey(_steamActiveUserReg);

                if (_userKey != null)
                {
                    Object o = _userKey.GetValue("ActiveUser");
                    if (o != null)
                    {
                        _steamActiveUser = o.ToString();
                        if (_steamActiveUser == "0")
                        {
                            throw new Exception(@"Registry Key 'Computer\HKEY_LOCAL_MACHINE\SOFTWARE\Valve\Steam\ActiveProcess\ActiveUser' is 0.");
                        }
                    }
                    else
                    {
                        throw new Exception(@"Registry Key 'Computer\HKEY_LOCAL_MACHINE\SOFTWARE\Valve\Steam\ActiveProcess\ActiveUser' has no value.");
                    }
                }

                // check if both folders exist
                string _dtPath = _steamInstallPath + @"\userdata\" + _steamActiveUser + @"\" + "1325900" + @"\remote";
                string _neonSplashPath = _steamInstallPath + @"\userdata\" + _steamActiveUser + @"\" + "1747890" + @"\remote";

                if (!Directory.Exists(_dtPath)) _dtPath = null;
                if (!Directory.Exists(_neonSplashPath)) _neonSplashPath = null;

                if (_dtPath == null && !neonSplash) return ShowManualDirectorySelection(neonSplash: false);
                if (_neonSplashPath == null && neonSplash) return ShowManualDirectorySelection(neonSplash: true);

                if (neonSplash) return _neonSplashPath;
                else return _dtPath;
            }
            catch
            {
                // check for GOG (DT only)
                if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + $@"\..\LocalLow\Fabraz\Demon Turf\{SAVE_FILE_NAME}"))
                {
                    return (Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\..\LocalLow\Fabraz\Demon Turf\");
                }
                else
                {
                    if (!neonSplash) return ShowManualDirectorySelection(neonSplash: false);
                    else if (neonSplash) return ShowManualDirectorySelection(neonSplash: true);
                    else return null;
                }
            }
        }

        private string ShowManualDirectorySelection(bool neonSplash)
        {
            if (!neonSplash)
            {
                PopupBox.Show("Could not find Demon Turf install directory. Please ensure that steam or GOG is running and the game is installed, or select a folder manually.");
                OpenFileDialog dtFileDialog = new OpenFileDialog();
                {
                    dtFileDialog.Filter = $"{SAVE_FILE_NAME}| *.txt";
                    dtFileDialog.FileName = SAVE_FILE_NAME;
                    dtFileDialog.FilterIndex = 1;
                    dtFileDialog.Title = "Select a Demon Turf install directory...";

                    if (dtFileDialog.ShowDialog() == true)
                    {
                        return (Path.GetDirectoryName(dtFileDialog.FileName));
                    }
                    else
                    {
                        return (null);
                    }
                }
            }
            else
            {
                PopupBox.Show("Could not find Neon Splash install directory. Please ensure that steam or GOG is running and the game is installed, or select a folder manually.");
                OpenFileDialog neonFileDialog = new OpenFileDialog();
                {
                    neonFileDialog.Filter = $"{SAVE_FILE_NAME}| *.txt";
                    neonFileDialog.FileName = SAVE_FILE_NAME;
                    neonFileDialog.FilterIndex = 1;
                    neonFileDialog.Title = "Select a Neon Splash install directory...";

                    if (neonFileDialog.ShowDialog() == true)
                    {
                        return (Path.GetDirectoryName(neonFileDialog.FileName));
                    }
                    else
                    {
                        ConfigService.SetNeonSplashDisabled(true);
                        return (null);
                    }
                }
            }
        }

        private void InitializeFiles(bool neonMode, string saveDirectory, Dictionary<string, int> saveMetadataList)
        {
            // check if _dtsm dir exists
            if (!Directory.Exists(saveDirectory + $@"\{DTSM_FOLDER_NAME}"))
                Directory.CreateDirectory(saveDirectory + @"\_dtsm");

            // check if DTSaveData exists and is in the _dtsm folder
            if (File.Exists(saveDirectory + $@"\{SAVE_FILE_NAME}") && !File.Exists(saveDirectory + $@"\{DTSM_FOLDER_NAME}" + $@"\{SAVE_FILE_NAME}"))
                File.Copy(saveDirectory + $@"\{SAVE_FILE_NAME}", saveDirectory + $@"\{DTSM_FOLDER_NAME}" + $@"\{SAVE_FILE_NAME}");

            // scan _dtsm folder
            foreach (string file in Directory.EnumerateFiles(saveDirectory + $@"\{DTSM_FOLDER_NAME}", "*.txt"))
            {
                string filename = Path.GetFileName(file);
                int.TryParse(RegexHelperService.GetNumberFromString(filename), out int idResult);
                if (idResult == 0) idResult = 1;
                saveMetadataList.Add(filename, idResult);
            }

            // check for saved active files, if not found, set to default
            (string, string) files = ConfigService.GetActiveFiles();
            if (files.Item1 != null && files.Item2 != null)
            {
                DtActiveFile = files.Item1;
                NeonSplashActiveFile = files.Item2;
            }
            else
            {
                DtActiveFile = SAVE_FILE_NAME;
                NeonSplashActiveFile = SAVE_FILE_NAME;
            }
        }

        public void ReinitializeFiles(bool neonMode)
        {
            if (neonMode)
            {
                _neonSaveDirectory = ConfigService.GetNSSaveDirectory();
                InitializeFiles(true, _neonSaveDirectory, NeonSplashSaveMetadata);
                CopyActiveFile(_neonSaveDirectory, NeonSplashActiveFile);
            }
            else
            {
                _dtSaveDirectory = ConfigService.GetDTSaveDirectory();
                InitializeFiles(false, _dtSaveDirectory, DtSaveMetadata);
                CopyActiveFile(_dtSaveDirectory, DtActiveFile);
            }
        }

        public void CopyActiveFile(string saveDirectory, string activeFile)
        {
            // copy active files on program open, ensuring data is up to date
            File.Copy(saveDirectory + $@"\{SAVE_FILE_NAME}", saveDirectory + $@"\{DTSM_FOLDER_NAME}\" + activeFile, true);
        }

        public Dictionary<string, int> GetSaveMetadataDict(bool isNeonMode)
        {
            return isNeonMode ? NeonSplashSaveMetadata : DtSaveMetadata;
        }

        public string GetFilePath(bool isNeonMode, string fileName)
        {
            return isNeonMode ? _neonSaveDirectory + $@"\{DTSM_FOLDER_NAME}\" + fileName : 
                _dtSaveDirectory + $@"\{DTSM_FOLDER_NAME}\" + fileName;
        }

        public string GetActiveFile(bool isNeonMode)
        {
            return isNeonMode ? NeonSplashActiveFile : DtActiveFile;
        }

        public void SetActiveFile(bool isNeonMode, string fileName)
        {
            if (isNeonMode)
            {
                CopyActiveFile(_neonSaveDirectory, NeonSplashActiveFile);
                NeonSplashActiveFile = fileName;
                File.Copy(_neonSaveDirectory + $@"\{DTSM_FOLDER_NAME}\" + fileName, _neonSaveDirectory + $@"\{SAVE_FILE_NAME}", true);
            }
            else
            {
                CopyActiveFile(_dtSaveDirectory, DtActiveFile);
                DtActiveFile = fileName;
                File.Copy(_dtSaveDirectory + $@"\{DTSM_FOLDER_NAME}\" + fileName, _dtSaveDirectory + $@"\{SAVE_FILE_NAME}", true);
            }
                
            ConfigService.SetActiveFiles((DtActiveFile, NeonSplashActiveFile));
        }

        public bool RenameFile(bool isNeonMode, string fileName, string newFileName)
        {
            if (CheckIfFileExists(newFileName, isNeonMode)) return false;

            var isValid = !string.IsNullOrEmpty(newFileName.Replace(".txt", "")) &&
              newFileName.IndexOfAny(Path.GetInvalidFileNameChars()) < 0;

            if (!isValid) return false;

            if (isNeonMode)
            {
                NeonSplashSaveMetadata.Remove(fileName);
                NeonSplashSaveMetadata.Add(newFileName, ReturnCurrentFileNumber(newFileName, isNeonMode, rename: true));
            }
            else
            {
                DtSaveMetadata.Remove(fileName);
                DtSaveMetadata.Add(newFileName, ReturnCurrentFileNumber(newFileName, isNeonMode, rename: true));
            }

            string oldPath = GetFilePath(isNeonMode, fileName);
            File.Move(oldPath, GetFilePath(isNeonMode, newFileName));
            File.Delete(oldPath);

            return true;
        }

        public void RemoveFile(bool isNeonMode, string fileName)
        {
            if (isNeonMode) NeonSplashSaveMetadata.Remove(fileName);
            else DtSaveMetadata.Remove(fileName);
            
            File.Delete(GetFilePath(isNeonMode, fileName));
        }

        public KeyValuePair<string, int>? DuplicateFile(bool isNeonMode, string fileName)
        {
            int newId = ReturnCurrentFileNumber(fileName, isNeonMode);
            string newFileName;

            if (isNeonMode)
            {
                newFileName = fileName.Replace($".txt", $"").Replace($" ({NeonSplashSaveMetadata[fileName]})", $"") + $" ({newId}).txt";
                if (NeonSplashSaveMetadata.Any(m => m.Key == newFileName)) return null;
                NeonSplashSaveMetadata.Add(newFileName, newId);
            }
            else
            {
                newFileName = fileName.Replace($".txt", $"").Replace($" ({DtSaveMetadata[fileName]})", $"") + $" ({newId}).txt";
                if (DtSaveMetadata.Any(m => m.Key == newFileName)) return null;
                DtSaveMetadata.Add(newFileName, newId);
            }

            File.Copy(GetFilePath(isNeonMode, fileName), GetFilePath(isNeonMode, newFileName), true);
            return new KeyValuePair<string, int>(newFileName, newId);
        }

        public KeyValuePair<string, int> FindSaveMetadataByFilename(bool isNeonMode, string filename)
        {
            if (isNeonMode) return NeonSplashSaveMetadata.First(m => m.Key == filename);
            else return DtSaveMetadata.First(m => m.Key == filename);
        }

        public string GetDirectory(bool isNeonMode)
        {
            return isNeonMode ? _neonSaveDirectory : _dtSaveDirectory;
        }

        private int ReturnCurrentFileNumber(string fileName, bool isNeonMode, bool rename = false)
        {
            Dictionary<string, int> data = isNeonMode ? NeonSplashSaveMetadata : DtSaveMetadata;
            bool hasNumber = RegexHelperService.GetStringHasNumber(fileName);
            string number = "";
            int digits = 1;
            if (hasNumber)
            {
                number = RegexHelperService.GetNumberFromString(fileName);
                digits = number.Count(char.IsDigit);
            }

            string fileSearchName = fileName.Substring(0, fileName.Length - (hasNumber ? (8 + digits - 1) : 4));

            var list = data.Where(m => m.Key.Substring(0, m.Key.Length - 
                (RegexHelperService.GetStringHasNumber(m.Key) ? (8 + RegexHelperService.GetNumberFromString(m.Key).Count(char.IsDigit) - 1) : 4)) == fileSearchName).OrderBy(m => m.Value).ToList();

            if (list.Count == 1 && rename) return 1;

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Value != i + 1) return i + 1;
            }

            return list.Count + 1;
        }

        private bool CheckIfFileExists(string newFilename, bool isNeonMode)
        {
            Dictionary<string, int> data = isNeonMode ? NeonSplashSaveMetadata : DtSaveMetadata;
            var list = data.Where(m => m.Key == newFilename).OrderBy(m => m.Value).ToList();
            if (list.Count > 0) return true;
            else return false;
        }
    }
}
