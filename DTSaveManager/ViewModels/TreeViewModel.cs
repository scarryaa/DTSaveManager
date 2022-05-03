using DTSaveManager.DataTypes.Enums;
using DTSaveManager.Models;
using DTSaveManager.Services;
using DTSaveManager.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Windows.Input;

namespace DTSaveManager.ViewModels
{
    class TreeViewModel : ViewModelBase
    {
        private bool _isFocused;
        private bool _active;
        private ObservableCollection<SaveMetadata> _saveMetadata;
        private ObservableCollection<TreeViewItemViewModel> _treeViewItemViewModels;
        private CollectionView _itemsView;

        public TreeViewModel(bool isNeonMode)
        {
            IsNeonMode = isNeonMode;
            UpdateSaveMetadata(IsNeonMode);

            TreeViewMouseDownCommand = new RelayCommand(c => TreeViewMouseDown());
        }

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

        public ObservableCollection<SaveMetadata> SaveMetadata
        {
            get { return _saveMetadata; }
            set { _saveMetadata = value; OnPropertyChanged(); }
        }

        public ObservableCollection<TreeViewItemViewModel> TreeViewItemViewModels
        {
            get { return _treeViewItemViewModels; }
            set { _treeViewItemViewModels = value; OnPropertyChanged(); }
        }

        public CollectionView ItemsView
        {
            get { return _itemsView; }
            set { _itemsView = value; OnPropertyChanged(); }
        }

        public ICommand TreeViewMouseDownCommand { get; set; }

        public string GetDirectory()
        {
            return SaveMetadataService.Instance.GetDirectory(IsNeonMode);
        }

        public void UpdateSaveMetadata(bool neonMode)
        {
            SaveMetadata = new ObservableCollection<SaveMetadata>(SaveMetadataService.Instance.GetSaveMetadata(neonMode));
            TreeViewItemViewModels = new ObservableCollection<TreeViewItemViewModel>();
            foreach (SaveMetadata saveMetadata in SaveMetadata)
            {
                TreeViewItemViewModels.Add(new TreeViewItemViewModel
                {
                    Id = saveMetadata.Id,
                    Active = saveMetadata.Active,
                    Filename = saveMetadata.Filename,
                    Path = saveMetadata.Path,
                    removeAction = item => RemoveMetadata(saveMetadata),
                    duplicateAction = item => DuplicateMetadata(item, saveMetadata),
                    renameMetadataAction = item => RenameMetadata(item, saveMetadata, item.Filename),
                    changeActiveAction = item => ChangeActive(item, saveMetadata)
                });
            }

            ItemsView = new ListCollectionView(TreeViewItemViewModels)
            {
                SortDescriptions =
                {
                    new System.ComponentModel.SortDescription("RootDisplayName", System.ComponentModel.ListSortDirection.Ascending),
                    new System.ComponentModel.SortDescription("Id", System.ComponentModel.ListSortDirection.Ascending)
                }
            };
        }

        private void TreeViewMouseDown()
        {
            IsFocused = false;
            IsFocused = true;
        }

        private void ChangeActive(TreeViewItemViewModel treeViewItemViewModel, SaveMetadata saveMetadata)
        {
            foreach (TreeViewItemViewModel item in TreeViewItemViewModels)
            {
                item.Active = false;
            }
            treeViewItemViewModel.Active = true;
            SaveMetadataService.Instance.ChangeActive(IsNeonMode, saveMetadata);
            UpdateSaveMetadata(IsNeonMode);
        }

        private void RemoveMetadata(SaveMetadata saveMetadata)
        {
            SaveMetadataService.Instance.RemoveMetadata(IsNeonMode, saveMetadata);
            UpdateSaveMetadata(IsNeonMode);
        }

        private void DuplicateMetadata(TreeViewItemViewModel treeViewItemViewModel, SaveMetadata saveMetadata)
        {
            var result = SaveMetadataService.Instance.DuplicateMetadata(IsNeonMode, saveMetadata);
            if (result == null) treeViewItemViewModel.DisplayMessage(MessageType.FileExistsMessage, timeout: true);
            else
            {
                treeViewItemViewModel.ClearMessage();
                UpdateSaveMetadata(IsNeonMode);
            }
        }

        private void RenameMetadata(TreeViewItemViewModel treeViewItemViewModel, SaveMetadata saveMetadata, string newFilename)
        {
            bool result = SaveMetadataService.Instance.Rename(IsNeonMode, saveMetadata, newFilename);
            if (!result)
            {
                treeViewItemViewModel.DisplayName = saveMetadata.Filename.Replace(".txt", "");
                treeViewItemViewModel.DisplayMessage(MessageType.FileExistsMessage, timeout: true);
            }
            else
            {
                treeViewItemViewModel.ClearMessage();
                UpdateSaveMetadata(IsNeonMode);
            }
        }
    }
}
