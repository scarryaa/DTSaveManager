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

namespace DTSaveManager
{
    public partial class DTSaveManagerGUI : Form
    {
        public DTSaveManagerGUI()
        {
            InitializeComponent();
        }

        private void Initialization(object sender, EventArgs e)
        {

            string _saveDirectory = GetPathFromRegistryKeys();
            Console.WriteLine(_saveDirectory);

            foreach (string file in Directory.EnumerateFiles(_saveDirectory, "*.txt"))
            {
                if (File.Exists(file.Replace(".txt", ".json"))) {
                } else
                {
                    //InitializeJson(file);
                }
                string contents = File.ReadAllText(file.Replace("txt", "json"));
                string fileName = Path.GetFileName(file);
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
