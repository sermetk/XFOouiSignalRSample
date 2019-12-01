using Microsoft.AspNetCore.SignalR.Client;
using OouiSignalRSample.Core;
using OouiSignalRSample.Core.Base;
using OouiSignalRSample.Modules.Test;
using OouiSignalRSample.Utility;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OouiSignalRSample.Modules.Support
{
    public class ChatPageViewModel : BaseViewModel
    {
        #region Properties
        private string _message;
        public string Message
        {
            get { return _message; }
            set { _message = value; OnPropertyChanged(); }
        }
        private HubConnection HubConnection;
        private string SelectedConnectionId;
        public string Name => "Supporter Alia";
        public string BannedUserMobilePhone { get; set; }
        #endregion

        #region CustomCommands

        public ICommand SendMessageCommand { get; set; }
        public ICommand SelectedUserCommand { get; set; }
        public ICommand UserInfoCommand { get; set; }
        public ICommand TestCommand { get; set; }
        public ICommand BanUserCommand { get; set; }
        public ICommand OpenBanCommand { get; set; }
        #endregion

        #region Services
        #endregion

        #region Methods
        public ChatPageViewModel()
        {
            Init();
        }
        public async override void Init()
        {
            InitCommands();
            await LoadData(Connect);
        }
        public override void InitCommands()
        {
            base.InitCommands();
            SelectedUserCommand = new CustomCommand((e) =>
            {
                if (e is User user)
                {
                    ConnectedUsers.Current.SelectedUserMessages = ToObservableCollection(ConnectedUsers.Current.ActiveUsers.FirstOrDefault(x => x == user).Messages);
                    foreach (var item in ConnectedUsers.Current.ActiveUsers)
                    {
                        if (item == user)
                        {
                            item.IsSelected = true;
                            item.UnreadedMessageCount = 0;
                            SelectedConnectionId = item.ClientId;
                        }
                        else
                        {
                            item.IsSelected = false;
                        }
                    }
                }
            });
            UserInfoCommand = new CustomCommand(async (e) =>
            {
                if (e is User user)
                {
                    await RootPage.DisplayAlert("Information",
                        $"User: {user.UserName}\n" +
                        $"Reason: {user.TicketType}\n" +
                        $"E-Mail: {user.Email}\n" +
                        $"Phone: {user.PhoneNumber}\n" +
                        $"ClientId: {user.ClientId}\n",
                        "Tamam");
                }
            });
            SendMessageCommand = new CustomCommand(async () =>
            {
                if (string.IsNullOrEmpty(SelectedConnectionId))
                    await RootPage.DisplayAlert("Warning", $"You must select at least a client to send messages.", "Okay");
                else
                    await SendMessage();
            });
            TestCommand = new CustomCommand(async () =>
            {
                await RootPage.PushAsync(new TestPage());
            });
            BanUserCommand = new CustomCommand(async (e) =>
            {
                if (SelectedConnectionId == null)
                    return;
                var user = ConnectedUsers.Current.ActiveUsers.FirstOrDefault(x => x.ClientId == SelectedConnectionId);
                if (user == null)
                    return;
                var dialog = await RootPage.DisplayAlert("Warning", $"Do you want block to user {user.UserName}?", "Yes", "No");
                if (dialog)
                {
                    ConnectedUsers.Current.BannedUsers.Add(user.PhoneNumber);
                    await HubConnection.InvokeAsync("BlockUser", SelectedConnectionId);
                }
            });
            OpenBanCommand = new CustomCommand(async () =>
            {
                if (string.IsNullOrEmpty(BannedUserMobilePhone))
                    return;
                var bannedUser = ConnectedUsers.Current.BannedUsers.FirstOrDefault(x => x == BannedUserMobilePhone);
                if (!string.IsNullOrEmpty(bannedUser))
                {
                    ConnectedUsers.Current.BannedUsers.Remove(bannedUser);
                    await RootPage.DisplayAlert("Information", "The selected user is unblocked.", "Okay");
                }
                else
                {
                    await RootPage.DisplayAlert("Information", "User not found.", "Okay");
                }
            });
        }
        private ObservableCollection<T> ToObservableCollection<T>(IEnumerable<T> _LinqResult)
        {
            return new ObservableCollection<T>(_LinqResult);
        }
        #endregion

        #region Api Calls
        private async Task Connect()
        {
            var url = "http://localhost:49795/chat";
            HubConnection = new HubConnectionBuilder().WithUrl(url, options =>
            {
                options.Headers["Supporter"] = "Yes";
            }).Build();
            await HubConnection.StartAsync();
        }

        private async Task SendMessage()
        {
            if (string.IsNullOrEmpty(Message) || string.IsNullOrEmpty(SelectedConnectionId))
                return;
            await HubConnection.InvokeAsync("SendMessageToCustomer", SelectedConnectionId, Message);
            Message = string.Empty;
        }
        #endregion

    }
}
