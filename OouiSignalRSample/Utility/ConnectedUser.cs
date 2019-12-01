using OouiSignalRSample.Core;
using OouiSignalRSample.Modules;
using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace OouiSignalRSample.Utility
{
    public class ConnectedUsers : BindableBase
    {
        public static ConnectedUsers Current = new ConnectedUsers();
        private ObservableCollection<User> _activeUsers = new ObservableCollection<User>();
        public ObservableCollection<User> ActiveUsers
        {
            get { return _activeUsers; }
            set { _activeUsers = value; OnPropertyChanged(); }
        }
        public ObservableCollection<MessageDto> _selectedUserMessages;
        public ObservableCollection<MessageDto> SelectedUserMessages
        {
            get { return _selectedUserMessages; }
            set { _selectedUserMessages = value; OnPropertyChanged(); }
        }
        public List<string> BannedUsers = new List<string>();
    }
}
