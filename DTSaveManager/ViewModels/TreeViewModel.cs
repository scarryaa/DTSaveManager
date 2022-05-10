using DTSaveManager.DataTypes.Enums;
using DTSaveManager.Models;
using DTSaveManager.Services;
using DTSaveManager.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;

namespace DTSaveManager.ViewModels
{
    class TreeViewModel : ViewModelBase
    {
        private bool _isFocused;
        private bool _active;
        private ObservableCollection<string> _saveMetadata;
        private ObservableCollection<TreeViewItemViewModel> _treeViewItemViewModels;
        private ICollectionView _itemsView;

        public TreeViewModel(bool isNeonMode)
        {
            IsNeonMode = isNeonMode;
            UpdateSaveMetadata(IsNeonMode);

            TreeViewMouseDownCommand = new RelayCommand(c => TreeViewMouseDown());
        }
        public Action<object> showPopupAction { get; set; }
        public Action<object> hidePopupAction { get; set; }

        public bool IsNeonMode
        {
            get; set;
        }

        public bool IsFocused
        {
            get { return _isFocused; }
            set { _isFocused = value; OnPropertyChanged(); }
        }

        public bool Active
        {
            get { return _active; }
            set { 
                _active = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> SaveMetadata
        {
            get { return _saveMetadata; }
            set { _saveMetadata = value; OnPropertyChanged(); }
        }

        public ObservableCollection<TreeViewItemViewModel> TreeViewItemViewModels
        {
            get { return _treeViewItemViewModels; }
            set { _treeViewItemViewModels = value; OnPropertyChanged(); }
        }

        public ICollectionView ItemsView
        {
            get { return _itemsView; }
            set { _itemsView = value; OnPropertyChanged(); }
        }

        public ICommand TreeViewMouseDownCommand { get; set; }
        public ICommand ShowPopupCommand { get; set; }
        public ICommand HidePopupCommand { get; set; }

        public string GetDirectory()
        {
            return SaveMetadataService.Instance.GetDirectory(IsNeonMode);
        }

        public void UpdateSaveMetadata(bool neonMode)
        {
            TreeViewItemViewModels = new ObservableCollection<TreeViewItemViewModel>();
            string activeFile = SaveMetadataService.Instance.GetActiveFile(neonMode);
            foreach (KeyValuePair<string, int> item in SaveMetadataService.Instance.GetSaveMetadataDict(neonMode))
            {
                TreeViewItemViewModels.Add(new TreeViewItemViewModel
                {
                    Id = item.Value,
                    Active = item.Key == activeFile,
                    Filename = item.Key,
                    Path = SaveMetadataService.Instance.GetFilePath(neonMode, item.Key),
                    removeAction = treeItemVM => RemoveMetadata(treeItemVM.Filename),
                    duplicateAction = treeItemVM => DuplicateMetadata(treeItemVM, treeItemVM.Filename),
                    renameMetadataAction = (treeItemVM, obj) => RenameMetadata(treeItemVM, treeItemVM.Filename, obj),
                    changeActiveAction = treeItemVM => ChangeActive(treeItemVM, treeItemVM.Filename)
                });
            }

            ItemsView = CollectionViewSource.GetDefaultView(TreeViewItemViewModels);
            ItemsView.SortDescriptions.Add(new SortDescription("Active", ListSortDirection.Descending));
            ItemsView.SortDescriptions.Add(new SortDescription("RootDisplayName", ListSortDirection.Ascending));
            ItemsView.SortDescriptions.Add(new SortDescription("Id", ListSortDirection.Ascending));
        }

        private void TreeViewMouseDown()
        {
            IsFocused = false;
            IsFocused = true;
        }

        private void ChangeActive(TreeViewItemViewModel treeViewItemViewModel, string fileName)
        {
            SaveMetadataService.Instance.SetActiveFile(IsNeonMode, fileName);
            string activeFile = SaveMetadataService.Instance.GetActiveFile(IsNeonMode);
            foreach (TreeViewItemViewModel item in TreeViewItemViewModels)
            {
                item.Active = item.Filename == activeFile;
            }
            ItemsView.Refresh();
        }

        private void RemoveMetadata(string fileName)
        {
            SaveMetadataService.Instance.RemoveFile(IsNeonMode, fileName);
            TreeViewItemViewModels.Where(m => m.Filename == fileName).ToList().All(m => TreeViewItemViewModels.Remove(m));
            ItemsView.Refresh();
        }

        private void DuplicateMetadata(TreeViewItemViewModel treeViewItemViewModel, string fileName)
        {
            var result = SaveMetadataService.Instance.DuplicateFile(IsNeonMode, fileName);
            if (result == null) treeViewItemViewModel.DisplayMessage(MessageType.FileExistsMessage, timeout: true);
            else
            {
                treeViewItemViewModel.ClearMessage();
                string activeFile = SaveMetadataService.Instance.GetActiveFile(IsNeonMode);
                TreeViewItemViewModels.Add(new TreeViewItemViewModel
                {
                    Id = result.Value.Value,
                    Active = result.Value.Key == activeFile,
                    Filename = result.Value.Key,
                    Path = SaveMetadataService.Instance.GetFilePath(IsNeonMode, result.Value.Key),
                    removeAction = treeItemVM => RemoveMetadata(treeItemVM.Filename),
                    duplicateAction = treeItemVM => DuplicateMetadata(treeItemVM, treeItemVM.Filename),
                    renameMetadataAction = (treeItemVM, obj) => RenameMetadata(treeItemVM, treeItemVM.Filename, obj),
                    changeActiveAction = treeItemVM => ChangeActive(treeItemVM, treeItemVM.Filename)
                });
                ItemsView.Refresh();
            }
        }

        private bool RenameMetadata(TreeViewItemViewModel treeViewItemViewModel, string fileName, string newFilename)
        {
            bool result = SaveMetadataService.Instance.RenameFile(IsNeonMode, fileName, newFilename);
            if (!result)
            {
                treeViewItemViewModel.DisplayName = fileName.Replace(".txt", "");
                if (fileName.Replace(".txt", "") != newFilename) treeViewItemViewModel.DisplayMessage(MessageType.FileExistsMessage, timeout: true);
            }
            else
            {
                treeViewItemViewModel.ClearMessage();
                TreeViewItemViewModels.Where(m => m.Filename == fileName).First().Filename = newFilename;
                ItemsView.Refresh();
            }

            return result;
        }
    }
}
