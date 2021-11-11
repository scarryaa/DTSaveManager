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
using System.Diagnostics;

namespace DTSaveManager
{
    public partial class DTSaveManagerGUI : Form
    {
        private List<SaveMetadata> _saveMetadata;
        private bool _initialized = false;
        private string _saveDirectory;
        public DTSaveManagerGUI()
        {
            InitializeComponent();
        }

        private void Initialization(object sender, EventArgs e)
        {
            _initialized = false;
            Initializer();
        }

        private void Initializer()
        {
            _initialized = false;

            _saveMetadata = new List<SaveMetadata>();
            _saveDirectory = GetPathFromRegistryKeys();
            InitializeFiles(_saveDirectory);

            InitializeTreeView();

            _setActive.Enabled = false;
            _rename.Enabled = false;
            _name.Enabled = false;
            _name.Text = "";
            _duplicate.Enabled = false;
            _import.Enabled = false;
            _export.Enabled = false;
            _remove.Enabled = false;

            saveFileList.SelectedNode = null;
            _initialized = true;

            if (_saveMetadata.FindAll(m => m.active == true).Count != 1)
            {
                _saveMetadata.Find(m => m.fileName == "DTSaveData.txt").active = true;
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
                    }
                    else
                    {
                        throw new Exception(@"Registry Key 'Computer\HKEY_LOCAL_MACHINE\SOFTWARE\Valve\Steam\ActiveProcess\ActiveUser' has no value.");
                    }
                }

                return _steamInstallPath + @"\userdata\" + _steamActiveUser + @"\1325900\remote";
            }
            catch
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "DTSaveData.txt|*.txt";
                    openFileDialog.FileName = "DTSaveData.txt";
                    openFileDialog.FilterIndex = 1;

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        return Path.GetDirectoryName(openFileDialog.FileName);
                    } else
                    {
                        return Application.StartupPath;
                    }
                }
            }
        }

        private void InitializeFiles(string _saveDirectory)
        {
            // move files to _dtsm folder
            foreach (string file in Directory.EnumerateFiles(_saveDirectory, "*.txt"))
            {
                string fileName = Path.GetFileName(file);
                Directory.CreateDirectory(_saveDirectory + @"\_dtsm");

                if (file.Contains(@"\DTSaveData.txt"))
                {
                    if (!File.Exists(_saveDirectory + @"\_dtsm\" + fileName))
                    {
                        File.Copy(file, _saveDirectory + @"\_dtsm\" + fileName);
                    }
                } else
                {
                    File.Move(file, _saveDirectory + @"\_dtsm\" + fileName);
                }
            }
            // scan _dtsm folder
            foreach (string file in Directory.EnumerateFiles(_saveDirectory + "\\_dtsm", "*.txt"))
            {
                string fileName = Path.GetFileName(file);

                // if inactive DTSaveData.txt exists, ignore it
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
                        path = _saveDirectory + @"\_dtsm\" + fileName,
                        active = fileName == "DTSaveData.txt"
                    };
                    File.WriteAllText(jsonPath, JsonConvert.SerializeObject(data));
                }
                _saveMetadata.Add(data);
            }

            if (_saveMetadata.FindAll(m => m.active == true).Count != 1)
            {
                _saveMetadata.Find(m => m.fileName == "DTSaveData.txt").active = true;
            }
        }

        private void InitializeTreeView()
        {
            saveFileList.Nodes.Clear();
            _saveMetadata.ForEach(data => {

                TreeNode node = new TreeNode()
                {
                    Name = data.fileName,
                    Tag = (object)data,
                    Text = data.nickName.Length > 0 && data.nickName != data.fileName ? "\"" + data.nickName + "\" (" + data.fileName + ")" : data.fileName,
                    Checked = data.active,
                };

                saveFileList.Nodes.Add(node);

            });
        }

        private void ResetChanges(object sender, EventArgs e)
        {
            Initializer();
        }

        private void SaveListSelectionChanged(object sender, TreeViewEventArgs e)
        {
            if (!_initialized) return;

            SaveMetadata data = (SaveMetadata)e.Node.Tag;

            _setActive.Enabled = !data.active;
            _rename.Enabled = true;
            _name.Enabled = true;
            _name.Text = data.nickName.Length > 0 ? data.nickName : data.fileName;
            _duplicate.Enabled = true;
            _import.Enabled = true;
            _export.Enabled = true;
            _remove.Enabled = true;
        }

        private void RenameSelection(object sender, EventArgs e)
        {
            SaveMetadata data = (SaveMetadata)saveFileList.SelectedNode.Tag;
            data.nickName = _name.Text;
            saveFileList.SelectedNode.Text = data.nickName.Length > 0 && data.nickName != data.fileName ? "\"" + data.nickName + "\" (" + data.fileName + ")" : data.fileName;

            InitializeTreeView();
        }

        private void SetActive(object sender, EventArgs e)
        {
            _initialized = false;
            foreach(TreeNode node in saveFileList.Nodes)
            {
                // Disable all (other) nodes
                node.Checked = false;
                SaveMetadata _data = (SaveMetadata)node.Tag;
                _data.active = false;
            }

            SaveMetadata data = (SaveMetadata)saveFileList.SelectedNode.Tag;
            data.active = true;
            saveFileList.SelectedNode.Checked = true;
            _setActive.Enabled = false;
            _initialized = true;

            InitializeTreeView();
        }

        private void DuplicateSelection(object sender, EventArgs e)
        {
            SaveMetadata data = (SaveMetadata)saveFileList.SelectedNode.Tag;
            string oldPath = data.path;

            SaveMetadata newData = new SaveMetadata();

            newData.fileName = data.fileName.Replace(".txt", "_duplicate.txt");
            newData.path = data.path.Replace(".txt", "_duplicate.txt");
            newData.active = false;
            newData.nickName = data.nickName.Length > 0 ? data.nickName + "_duplicate" : "";

            if (!File.Exists(newData.path))
            {
                File.Copy(oldPath, newData.path, true);
                File.WriteAllText(newData.path.Replace(".txt", ".dtsm"), JsonConvert.SerializeObject(newData));

                _saveMetadata.Add(newData);
                InitializeTreeView();
            } else
            {
                // throw error
                MessageBox.Show("File already exists!");
            }
        }

        private void WriteChanges(object sender, EventArgs e)
        {
            _saveMetadata.ForEach(data => {
                File.WriteAllText(data.path.Replace(".txt", ".dtsm"), JsonConvert.SerializeObject(data));
            });

            foreach (TreeNode node in saveFileList.Nodes)
            {
                // check if file is marked for removal
                if (File.Exists(node.Tag.ToString()))
                {
                    File.Delete(node.Tag.ToString());
                    File.Delete(node.Tag.ToString().Replace(".txt", ".dtsm"));
                }
                else
                {
                    SaveMetadata _data = (SaveMetadata)node.Tag;
                    if (_data.active && node.Checked)
                    {
                        if (File.Exists(_saveDirectory + @"\DTSaveData.txt"))
                        {
                            File.Copy(_saveDirectory + @"\_dtsm\" + _data.fileName, _saveDirectory + @"\DTSaveData.txt", true);
                        }
                    }
                }

            }

            InitializeTreeView();
        }

        private void CheckDisabler(object sender, TreeViewCancelEventArgs e)
        {
            if (!_initialized) return;
            e.Cancel = true;
        }

        private void ImportSave(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = Application.StartupPath;
                openFileDialog.Filter = "DTSaveData.txt|*.txt";
                openFileDialog.FileName = "DTSaveData.txt";
                openFileDialog.FilterIndex = 1;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    SaveMetadata data = new SaveMetadata()
                    {
                        fileName = openFileDialog.SafeFileName,
                        nickName = "",
                        path = _saveDirectory + @"\_dtsm\" + openFileDialog.SafeFileName,
                        active = false,
                    };

                    if (!File.Exists(data.path))
                    {
                        File.Copy(openFileDialog.FileName, data.path, true);
                        File.WriteAllText(data.path.Replace(".txt", ".dtsm"), JsonConvert.SerializeObject(data));

                        _saveMetadata.Add(data);
                        InitializeTreeView();
                    }
                    else
                    {
                        // throw error
                        MessageBox.Show("File already exists!");
                    }
                }
            }
        }

        private void ExportSave(object sender, EventArgs e)
        {
            using (var saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.InitialDirectory = Application.StartupPath;
                saveFileDialog.Filter = "DTSaveData.txt|*.txt";
                saveFileDialog.FileName = "DTSaveData.txt";
                saveFileDialog.FilterIndex = 1;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllBytes(saveFileDialog.FileName, File.ReadAllBytes(((SaveMetadata)saveFileList.SelectedNode.Tag).path));
                }
            }

            InitializeTreeView();
        }

        private void RemoveSelection(object sender, EventArgs e)
        {
            MessageBox.Show("The save file is marked for removal. If you really want to remove this save file, click on 'Apply Changes' now. To cancel the removal, either click 'Reset Changes' or initiate any other action, such as Set Active, Rename, Duplicate, Import or Export.");
            saveFileList.SelectedNode.Text = "[*remove] " + saveFileList.SelectedNode.Text;
            saveFileList.SelectedNode.Tag = (object)((SaveMetadata)saveFileList.SelectedNode.Tag).path;
        }

        private void OpenSaveDirectory(object sender, EventArgs e)
        {
            Process.Start(_saveDirectory);
        }
    }
}
