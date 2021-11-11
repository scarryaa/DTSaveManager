using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using DTSaveManager;

namespace DTSaveManager
{
    public partial class DTSaveManagerGUI : Form
    {
        private List<SaveMetadata> _saveMetadata;
        public DTSaveManagerGUI()
        {
            InitializeComponent();
        }

        private void Initialization(object sender, EventArgs e)
        {
            _saveMetadata = new List<SaveMetadata>();
            string _saveDirectory = GetPathFromRegistryKeys();
            InitializeFiles(_saveDirectory);
        }

        private void InitializeFiles(string _saveDirectory)
        {
            foreach (string file in Directory.EnumerateFiles(_saveDirectory, "*.txt"))
            {
                string fileName = Path.GetFileName(file);
                string jsonPath = file.Replace(".txt", ".dtsm");
                SaveMetadata data;

                if (File.Exists(jsonPath))
                {
                    data = JsonConvert.DeserializeObject<SaveMetadata>(File.ReadAllText(jsonPath));
                }
                else
                {
                    data = new SaveMetadata()
                    {
                        fileName = fileName,
                        nickName = "",
                        path = file,
                        active = false
                    };
                    File.WriteAllText(jsonPath, JsonConvert.SerializeObject(data));
                }
                _saveMetadata.Add(data);
            }
        }

        private string GetPathFromRegistryKeys()
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
                    } else
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
                    } else
                    {
                        throw new Exception(@"Registry Key 'Computer\HKEY_LOCAL_MACHINE\SOFTWARE\Valve\Steam\ActiveProcess\ActiveUser' has no value.");
                    }
                }

                return _steamInstallPath + @"\userdata\" + _steamActiveUser + @"\1325900\remote";
            }
            catch (Exception ex)
            {
                // error, user has to navigate path their own
                return ex.Message;
            }
        }
    }
}
