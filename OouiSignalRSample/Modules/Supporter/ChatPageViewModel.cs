using Microsoft.AspNetCore.SignalR;
using OouiSignalRSample.Core;
using OouiSignalRSample.Core.Base;
using OouiSignalRSample.Core.Hubs;
using OouiSignalRSample.Modules.Test;
using OouiSignalRSample.Utility;
using System;
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
        private string SelectedConnectionId;
        public string Name => "Supporter Alia";
        public string BannedUserMobilePhone { get; set; }
        #endregion

        #region Commands
        public ICommand SendMessageCommand { get; set; }
        public ICommand SelectedUserCommand { get; set; }
        public ICommand UserInfoCommand { get; set; }
        public ICommand ClientTestPageCommand { get; set; }
        public ICommand BanUserCommand { get; set; }
        public ICommand OpenBanCommand { get; set; }
        #endregion

        #region Services
        private readonly IHubContext<ChatHub, IChatHub> hubContext;
        #endregion

        #region Methods

        public ChatPageViewModel(IHubContext<ChatHub, IChatHub> hubContext)
        {
            this.hubContext = hubContext;
            Init();
        }

        public override void Init()
        {
            InitCommands();
        }
        public override void InitCommands()
        {
            base.InitCommands();
            SelectedUserCommand = new CustomCommand((e) =>
            {
                if (e is User user)
                {
                    ConnectedUsers.Current.SelectedUserMessages = new ObservableCollection<MessageDto>(ConnectedUsers.Current.ActiveUsers.FirstOrDefault(x => x == user).Messages);
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
            ClientTestPageCommand = new CustomCommand(async () =>
            {
                await RootPage.PushAsync(new ClientTestPage());
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
        #endregion

        #region Api Calls
        private async Task SendMessage()
        {
            if (string.IsNullOrEmpty(Message) || string.IsNullOrEmpty(SelectedConnectionId))
                return;
            var user = ConnectedUsers.Current.ActiveUsers.FirstOrDefault(x => x.ClientId == SelectedConnectionId);
            if (user != null)
            {
                user.Messages.Add(new MessageDto { Message = Message, MessageTime = DateTime.UtcNow.AddHours(3) });
                if (user.IsSelected)
                {
                    ConnectedUsers.Current.SelectedUserMessages.Clear();
                    ConnectedUsers.Current.SelectedUserMessages = new ObservableCollection<MessageDto>(user.Messages);
                }
            }
            await hubContext.Clients.Client(SelectedConnectionId).ReceiveMessage(Message);
            Message = string.Empty;
        }
        #endregion

    }
}
