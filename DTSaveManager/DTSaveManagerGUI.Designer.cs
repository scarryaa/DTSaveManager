
namespace DTSaveManager
{
    partial class DTSaveManagerGUI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DTSaveManagerGUI));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.saveFileList = new System.Windows.Forms.TreeView();
            this._export = new System.Windows.Forms.Button();
            this._import = new System.Windows.Forms.Button();
            this._duplicate = new System.Windows.Forms.Button();
            this._name = new System.Windows.Forms.TextBox();
            this._rename = new System.Windows.Forms.Button();
            this._applyChanges = new System.Windows.Forms.Button();
            this._resetChanges = new System.Windows.Forms.Button();
            this._setActive = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.saveFileList);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this._export);
            this.splitContainer1.Panel2.Controls.Add(this._import);
            this.splitContainer1.Panel2.Controls.Add(this._duplicate);
            this.splitContainer1.Panel2.Controls.Add(this._name);
            this.splitContainer1.Panel2.Controls.Add(this._rename);
            this.splitContainer1.Panel2.Controls.Add(this._applyChanges);
            this.splitContainer1.Panel2.Controls.Add(this._resetChanges);
            this.splitContainer1.Panel2.Controls.Add(this._setActive);
            this.splitContainer1.Size = new System.Drawing.Size(622, 291);
            this.splitContainer1.SplitterDistance = 401;
            this.splitContainer1.TabIndex = 0;
            // 
            // saveFileList
            // 
            this.saveFileList.CheckBoxes = true;
            this.saveFileList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.saveFileList.Location = new System.Drawing.Point(0, 0);
            this.saveFileList.Name = "saveFileList";
            this.saveFileList.Size = new System.Drawing.Size(401, 291);
            this.saveFileList.TabIndex = 0;
            this.saveFileList.BeforeCheck += new System.Windows.Forms.TreeViewCancelEventHandler(this.CheckDisabler);
            this.saveFileList.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.SaveListSelectionChanged);
            // 
            // _export
            // 
            this._export.Enabled = false;
            this._export.Location = new System.Drawing.Point(4, 163);
            this._export.Name = "_export";
            this._export.Size = new System.Drawing.Size(211, 23);
            this._export.TabIndex = 7;
            this._export.Text = "Export";
            this._export.UseVisualStyleBackColor = true;
            this._export.Click += new System.EventHandler(this.ExportSave);
            // 
            // _import
            // 
            this._import.Enabled = false;
            this._import.Location = new System.Drawing.Point(3, 134);
            this._import.Name = "_import";
            this._import.Size = new System.Drawing.Size(211, 23);
            this._import.TabIndex = 6;
            this._import.Text = "Import";
            this._import.UseVisualStyleBackColor = true;
            this._import.Click += new System.EventHandler(this.ImportSave);
            // 
            // _duplicate
            // 
            this._duplicate.Enabled = false;
            this._duplicate.Location = new System.Drawing.Point(3, 85);
            this._duplicate.Name = "_duplicate";
            this._duplicate.Size = new System.Drawing.Size(211, 23);
            this._duplicate.TabIndex = 5;
            this._duplicate.Text = "Duplicate";
            this._duplicate.UseVisualStyleBackColor = true;
            this._duplicate.Click += new System.EventHandler(this.DuplicateSelection);
            // 
            // _name
            // 
            this._name.Enabled = false;
            this._name.Location = new System.Drawing.Point(4, 59);
            this._name.Name = "_name";
            this._name.Size = new System.Drawing.Size(210, 20);
            this._name.TabIndex = 4;
            // 
            // _rename
            // 
            this._rename.Enabled = false;
            this._rename.Location = new System.Drawing.Point(3, 34);
            this._rename.Name = "_rename";
            this._rename.Size = new System.Drawing.Size(211, 23);
            this._rename.TabIndex = 3;
            this._rename.Text = "Rename";
            this._rename.UseVisualStyleBackColor = true;
            this._rename.Click += new System.EventHandler(this.RenameSelection);
            // 
            // _applyChanges
            // 
            this._applyChanges.Location = new System.Drawing.Point(3, 236);
            this._applyChanges.Name = "_applyChanges";
            this._applyChanges.Size = new System.Drawing.Size(211, 23);
            this._applyChanges.TabIndex = 2;
            this._applyChanges.Text = "Apply Changes";
            this._applyChanges.UseVisualStyleBackColor = true;
            this._applyChanges.Click += new System.EventHandler(this.WriteChanges);
            // 
            // _resetChanges
            // 
            this._resetChanges.Location = new System.Drawing.Point(3, 265);
            this._resetChanges.Name = "_resetChanges";
            this._resetChanges.Size = new System.Drawing.Size(211, 23);
            this._resetChanges.TabIndex = 1;
            this._resetChanges.Text = "Reset Changes";
            this._resetChanges.UseVisualStyleBackColor = true;
            this._resetChanges.Click += new System.EventHandler(this.ResetChanges);
            // 
            // _setActive
            // 
            this._setActive.Enabled = false;
            this._setActive.Location = new System.Drawing.Point(3, 5);
            this._setActive.Name = "_setActive";
            this._setActive.Size = new System.Drawing.Size(211, 23);
            this._setActive.TabIndex = 0;
            this._setActive.Text = "Set Active";
            this._setActive.UseVisualStyleBackColor = true;
            this._setActive.Click += new System.EventHandler(this.SetActive);
            // 
            // DTSaveManagerGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(622, 291);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DTSaveManagerGUI";
            this.Text = "Demon Turf Save Manager [by Nitro]";
            this.Load += new System.EventHandler(this.Initialization);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView saveFileList;
        private System.Windows.Forms.Button _setActive;
        private System.Windows.Forms.Button _duplicate;
        private System.Windows.Forms.TextBox _name;
        private System.Windows.Forms.Button _rename;
        private System.Windows.Forms.Button _applyChanges;
        private System.Windows.Forms.Button _resetChanges;
        private System.Windows.Forms.Button _export;
        private System.Windows.Forms.Button _import;
    }
}

