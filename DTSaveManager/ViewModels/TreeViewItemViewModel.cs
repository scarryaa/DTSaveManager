using DTSaveManager.DataTypes.Enums;
using DTSaveManager.Services;
using DTSaveManager.ViewModels.Base;
using System;
using System.Windows.Input;

namespace DTSaveManager.ViewModels
{
    class TreeViewItemViewModel : ViewModelBase
    {
        private bool _showPopup = true;
        private bool _isEditing = false;
        private bool _isFocused = false;
        private bool _showMessage = false;
        private bool _isLocked = false;
        private MessageType _currentMessageType;
        private string _messageText;
        private int _id;
        private bool _active;
        private string _filename;
        private string _displayName;
        private string _rootDisplayName;
        private string _path;

        private TimerService _timerService;

        public TreeViewItemViewModel()
        {
            _timerService = new TimerService();

            DisplayMessageCommand = new RelayCommand(c => DisplayMessage((MessageType)((object[])c)[0], (bool)((object[])c)[1]));
            ClearMessageCommand = new RelayCommand(c => ClearMessage());
            RemoveCommand = new RelayCommand(c => RemoveMetadata());
            DuplicateCommand = new RelayCommand(c => DuplicateMetadata());
            FocusTextBoxCommand = new RelayCommand(c => FocusTextBox());
            RenameMetadataCommand = new RelayCommand(c => RenameMetadata((string)c));
            ChangeActiveCommand = new RelayCommand(c => ChangeActive());
            DisplayPopupCommand = new RelayCommand(c => DisplayPopup());
            HidePopupCommand = new RelayCommand(c => HidePopup());
            LockCommand = new RelayCommand(c => LockItem());
            UnlockCommand = new RelayCommand(c => UnlockItem());
        }

        public Action<TreeViewItemViewModel> removeAction { get; set; }
        public Action<TreeViewItemViewModel> duplicateAction { get; set; }
        public Func<TreeViewItemViewModel, string, bool> renameMetadataAction { get; set; }
        public Action<TreeViewItemViewModel> changeActiveAction { get; set; }
        public Action<object> showPopupAction { get; set; }
        public Action<object> hidePopupAction { get; set; }

        public bool ShowPopup
        {
            get { return _showPopup; }
            set
            {
                _showPopup = value;
                OnPropertyChanged();
            }
        }
        public bool IsEditing
        {
            get { return _isEditing; }
            set
            {
                _isEditing = value;
                OnPropertyChanged();
            }
        }
        public bool IsFocused
        {
            get { return _isFocused; }
            set
            {
                _isFocused = value;
                OnPropertyChanged();
            }
        }

        public bool ShowMessage
        {
            get { return _showMessage; }
            set
            {
                _showMessage = value;
                OnPropertyChanged();
            }
        }

        public bool IsLocked
        {
            get { return _isLocked; }
            set
            {
                _isLocked = value;
                OnPropertyChanged();
            }
        }

        public MessageType CurrentMessageType
        {
            get { return _currentMessageType; }
            set
            {
                _currentMessageType = value;
                OnPropertyChanged();
            }
        }

        public string MessageText
        {
            get { return _messageText; }
            set
            {
                _messageText = value;
                OnPropertyChanged();
            }
        }
        
        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        public bool Active
        {
            get { return _active; }
            set
            {
                _active = value;
                OnPropertyChanged();
            }
        }

        public string Filename
        {
            get { return _filename; }
            set
            {
                _filename = value;
                DisplayName = _filename.Replace(".txt", "");
                OnPropertyChanged();
            }
        }

        public string DisplayName
        {
            get { return _displayName; }
            set
            {
                _displayName = value;
                RootDisplayName = DisplayName.Substring(0, DisplayName.Length -
                        (RegexHelperService.GetStringHasNumber(DisplayName) ? 4 : 0));
                OnPropertyChanged();
            }
        }

        public string RootDisplayName
        {
            get { return _rootDisplayName; }
            set
            {
                _rootDisplayName = value;
                OnPropertyChanged();
            }
        }

        public string Path
        {
            get { return _path; }
            set
            {
                _path = value;
                OnPropertyChanged();
            }
        }

        private void SetMessageText(MessageType? messageType = null)
        {
            switch (messageType)
            {
                case MessageType.DeletionMessage:
                    CurrentMessageType = MessageType.DeletionMessage;
                    MessageText = "Press again to confirm deletion."; 
                    break;
                case MessageType.FileExistsMessage: 
                    CurrentMessageType = MessageType.FileExistsMessage;    
                    MessageText = "File name already taken."; 
                    break;
                default:
                    CurrentMessageType = MessageType.FileExistsMessage;
                    MessageText = ""; 
                    break;
            }
        }

        public ICommand DisplayMessageCommand { get; set; }
        public ICommand ClearMessageCommand { get; set; }
        public ICommand RemoveCommand { get; set; }
        public ICommand DuplicateCommand { get; set; }
        public ICommand FocusTextBoxCommand { get; set; }
        public ICommand RenameMetadataCommand { get; set; }
        public ICommand RadioButtonLeftMouseDownCommand { get; set; }
        public ICommand RadioButtonLeftMouseUpCommand { get; set; }
        public ICommand ChangeActiveCommand { get; set; }
        public ICommand DisplayPopupCommand { get; set; }
        public ICommand HidePopupCommand { get; set; }
        public ICommand LockCommand { get; set; }
        public ICommand UnlockCommand { get; set; }

        public void DisplayMessage(MessageType messageType, bool timeout)
        {
            if (messageType == MessageType.DeletionMessage) _timerService.CancelTask();

            if (timeout) _timerService.DoTaskAfterDelay(2000, new Action(() => ClearMessage()));
            SetMessageText(messageType);
            ShowMessage = true;
        }

        public void ClearMessage()
        {
            SetMessageText();
            ShowMessage = false;
        }

        private void RemoveMetadata()
        {
            ShowMessage = false;

            if (removeAction != null)
                removeAction.Invoke(this);
        }

        private void DuplicateMetadata()
        {
            if (duplicateAction != null)
                duplicateAction.Invoke(this);
        }

        private void ChangeActive()
        {
            if (changeActiveAction != null)
                changeActiveAction.Invoke(this);
        }

        private void DisplayPopup()
        {
            ShowPopup = true;
        }

        private void HidePopup()
        {
            ShowPopup = false;
        }

        private void LockItem()
        {
            IsLocked = true;
            ConfigService.LockFile(Filename);
        }

        private void UnlockItem()
        {
            IsLocked = false;
            ConfigService.UnlockFile(Filename);
        }

        public void FocusTextBox()
        {
            IsEditing = true;
            IsFocused = true;
        }

        private void RenameMetadata(string newFileName)
        {
            newFileName += ".txt";
            bool success = false;
            if (!IsEditing) return;

            if (renameMetadataAction != null)
                success = (bool)renameMetadataAction.Invoke(this, newFileName);

            if (newFileName != Filename && success)
                Filename = newFileName;

            IsEditing = false;
            IsFocused = false;
        }
    }
}
