using DTSaveManager.Models;
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
        public static List<SaveMetadata> DtSaveMetadata = new List<SaveMetadata>();
        public static List<SaveMetadata> NeonSplashSaveMetadata = new List<SaveMetadata>();

        static SaveMetadataService() { Instance = new SaveMetadataService(); }

        public static SaveMetadataService Instance { get; private set; }

        private SaveMetadataService()
        {
            // run initial setup functions
            // errors if steam is in offline mode?
            (_dtSaveDirectory, _neonSaveDirectory) = GetPathFromRegistryKeys();

            if (_dtSaveDirectory != null)
            {
                InitializeFiles(false, _dtSaveDirectory, DtSaveMetadata);
                CopyActiveFiles(_dtSaveDirectory, DtSaveMetadata);
                SetActive(_dtSaveDirectory, DtSaveMetadata);
            }

            if (_neonSaveDirectory != null)
            {
                InitializeFiles(true, _neonSaveDirectory, NeonSplashSaveMetadata);
                CopyActiveFiles(_neonSaveDirectory, NeonSplashSaveMetadata);
                SetActive(_neonSaveDirectory, NeonSplashSaveMetadata);
            }
        }

        public List<SaveMetadata> GetSaveMetadata(bool isNeonMode)
        {
            return isNeonMode ? NeonSplashSaveMetadata : DtSaveMetadata;
        }

        public bool Rename(bool isNeonMode, SaveMetadata saveMetadata, string newFilename)
        {
            string oldPath = saveMetadata.Path;
            if (CheckIfFileExists(newFilename, isNeonMode))
            {
                return false;
            }

            saveMetadata.Path = saveMetadata.Path.Replace(saveMetadata.Filename, newFilename + ".txt");
            saveMetadata.Filename = newFilename + ".txt";
            saveMetadata.Id = ReturnCurrentFileNumber(saveMetadata, isNeonMode);

            File.Copy(oldPath, saveMetadata.Path, true);
            File.WriteAllText(saveMetadata.Path.Replace(".txt", ".dtsm"), JsonConvert.SerializeObject(saveMetadata));
            RemoveMetadataFileOnly(isNeonMode, oldPath);
            return true;
        }

        public void RemoveMetadata(bool isNeonMode, SaveMetadata saveMetadata)
        {
            if (!isNeonMode)
            {
                saveMetadata = DtSaveMetadata.First(m => m.Filename == saveMetadata.Filename);
                DtSaveMetadata.Remove(saveMetadata);
            }
            else
            {
                saveMetadata = NeonSplashSaveMetadata.First(m => m.Filename == saveMetadata.Filename);
                NeonSplashSaveMetadata.Remove(saveMetadata);
            }

            File.Delete(saveMetadata.Path);
            File.Delete(saveMetadata.Path.Replace(".txt", ".dtsm"));
        }

        public void RemoveMetadataFileOnly(bool isNeonMode, string path)
        {
            File.Delete(path);
            File.Delete(path.Replace(".txt", ".dtsm"));
        }

        public SaveMetadata DuplicateMetadata(bool isNeonMode, SaveMetadata saveMetadata)
        {
            bool fileAlreadyExists = false;
            int newId = ReturnCurrentFileNumber(saveMetadata, isNeonMode: isNeonMode);
            string newFileName;

            saveMetadata = isNeonMode ? NeonSplashSaveMetadata.First(m => m.Filename == saveMetadata.Filename) :
                saveMetadata = DtSaveMetadata.First(m => m.Filename == saveMetadata.Filename);
            newFileName = saveMetadata.Filename.Replace($".txt", $"").Replace($" ({saveMetadata.Id})", $"") + $" ({newId}).txt";
            fileAlreadyExists = isNeonMode ? NeonSplashSaveMetadata.Any(m => m.Filename == newFileName) :
                DtSaveMetadata.Any(m => m.Filename == newFileName);

            if (fileAlreadyExists || newId == 0) return null;

            SaveMetadata newSaveMetadata = new SaveMetadata();
            newSaveMetadata.Filename = newFileName;
            newSaveMetadata.Path = isNeonMode ? _neonSaveDirectory + "\\_dtsm\\" + newFileName : _dtSaveDirectory + "\\_dtsm\\" + newFileName;
            newSaveMetadata.Active = false;
            newSaveMetadata.Id = newId;

            if (!isNeonMode) DtSaveMetadata.Add(newSaveMetadata);
            else NeonSplashSaveMetadata.Add(newSaveMetadata);
            File.Copy(saveMetadata.Path, newSaveMetadata.Path, true);
            File.WriteAllText(newSaveMetadata.Path.Replace(".txt", ".dtsm"), JsonConvert.SerializeObject(newSaveMetadata));
            return newSaveMetadata;
        }

        public SaveMetadata FindSaveMetadataByFilename(bool isNeonMode, string filename)
        {
            if (isNeonMode)
                return NeonSplashSaveMetadata.First(m => m.Filename == filename);
            else
                return DtSaveMetadata.First(m => m.Filename == filename);
        }

        public string GetDirectory(bool isNeonMode)
        {
            return isNeonMode ? _neonSaveDirectory : _dtSaveDirectory;
        }
        private (string, string) GetPathFromRegistryKeys()
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
                string _dtPath = _steamInstallPath + @"\userdata\" + _steamActiveUser + @"\" + "1747890" + @"\remote";
                string _neonSplashPath = _steamInstallPath + @"\userdata\" + _steamActiveUser + @"\" + "1325900" + @"\remote";

                if (!Directory.Exists(_dtPath)) _dtPath = null;
                if (!Directory.Exists(_neonSplashPath)) _neonSplashPath = null;

                return (_dtPath, _neonSplashPath);
            }
            catch
            {
                // check for GOG
                if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\..\LocalLow\Fabraz\Demon Turf\DTSaveData.txt"))
                {
                    return (Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\..\LocalLow\Fabraz\Demon Turf\", null);
                }
                else
                {
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    {
                        openFileDialog.Filter = "DTSaveData.txt|*.txt";
                        openFileDialog.FileName = "DTSaveData.txt";
                        openFileDialog.FilterIndex = 1;

                        if (openFileDialog.ShowDialog() == true)
                        {
                            return (Path.GetDirectoryName(openFileDialog.FileName), null);
                        }
                        else
                        {
                            return (Environment.CurrentDirectory, null);
                        }
                    }
                }
            }
        }

        private void InitializeFiles(bool neonMode, string saveDirectory, List<SaveMetadata> saveMetadataList)
        {
            // move files to _dtsm folder
            Directory.CreateDirectory(saveDirectory + @"\_dtsm");

            foreach (string file in Directory.EnumerateFiles(saveDirectory, "*.txt"))
            {
                string fileName = Path.GetFileName(file);

                if (file.Contains(@"\DTSaveData.txt"))
                {
                    if (!File.Exists(saveDirectory + @"\_dtsm\" + fileName))
                    {
                        File.Copy(file, saveDirectory + @"\_dtsm\" + fileName);
                    }
                }
                else if (!file.Contains(@"\output_log.txt"))
                {
                    File.Move(file, saveDirectory + @"\_dtsm\" + fileName);
                }
            }
            // scan _dtsm folder
            foreach (string file in Directory.EnumerateFiles(saveDirectory + "\\_dtsm", "*.txt"))
            {
                string filename = Path.GetFileName(file);

                // if inactive DTSaveData.txt exists, ignore it
                string jsonPath = filename.Replace(".txt", ".dtsm");
                SaveMetadata data;

                if (File.Exists(saveDirectory + @"\_dtsm\" + jsonPath))
                {
                    var x = File.ReadAllText(saveDirectory + @"\_dtsm\" + jsonPath);
                    data = JsonConvert.DeserializeObject<SaveMetadata>(x);
                }
                else
                {
                    data = new SaveMetadata()
                    {
                        Filename = filename,
                        Path = saveDirectory + @"\_dtsm\" + filename,
                        Active = filename == "DTSaveData.txt"
                    };
                    int.TryParse(RegexHelperService.GetNumberFromString(filename) ?? "1", out int idResult);
                    data.Id = idResult;
                    File.WriteAllText(saveDirectory + @"\_dtsm\" + jsonPath, JsonConvert.SerializeObject(data));
                }
                saveMetadataList.Add(data);
            }

            if (saveMetadataList.Where(m => m.Active == true).ToList().Count != 1)
            {
                if (File.Exists(saveDirectory + @"\DTSaveData.txt"))
                {
                    saveMetadataList.Where(m => m.Filename == "DTSaveData.txt").Single().Active = true;
                }
            }
        }

        private void CopyActiveFiles(string saveDirectory, List<SaveMetadata> saveMetadataList)
        {
            foreach (SaveMetadata saveMetadata in saveMetadataList)
            {
                if (saveMetadata.Active == true)
                {
                    File.Copy(saveDirectory + "\\DTSaveData.txt", saveDirectory + @"\_dtsm\" + saveMetadata.Filename, true);
                }
            }
        }

        private void SetActive(string saveDirectory, List<SaveMetadata> saveMetadataList)
        {
            if (saveMetadataList.Where(m => m.Active == true).ToList().Count != 1)
            {
                if (File.Exists(saveDirectory + @"\DTSaveData.txt"))
                {
                    saveMetadataList.Where(m => m.Filename == "DTSaveData.txt").First().Active = true;
                }
            }
        }

        private int ReturnCurrentFileNumber(SaveMetadata saveMetadata, bool isNeonMode)
        {
            List<SaveMetadata> data = isNeonMode ? NeonSplashSaveMetadata : DtSaveMetadata;
            bool hasNumber = RegexHelperService.GetStringHasNumber(saveMetadata.Filename);
            string fileSearchName = saveMetadata.Filename.Substring(0, saveMetadata.Filename.Length - (hasNumber ? 8 : 4));

            var list = data.FindAll(m => m.Filename.Contains(fileSearchName)).OrderBy(m => m.Id).ToList();

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Id != i + 1) return i + 1;
            }

            return list.Count + 1;
        }

        private bool CheckIfFileExists(string newFilename, bool isNeonMode)
        {
            List<SaveMetadata> data = isNeonMode ? NeonSplashSaveMetadata : DtSaveMetadata;
            var list = data.FindAll(m => m.Filename == newFilename + ".txt").OrderBy(m => m.Id).ToList();
            if (list.Count > 0) return true;
            else return false;
        }
    }
}
