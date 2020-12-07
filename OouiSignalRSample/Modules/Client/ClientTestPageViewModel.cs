using Microsoft.AspNetCore.SignalR.Client;
using OouiSignalRSample.Core;
using OouiSignalRSample.Core.Base;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OouiSignalRSample.Modules.Test
{
    public class ClientTestPageViewModel : BaseViewModel
    {
        #region Properties
        private ObservableCollection<MessageDto> _messages;
        public ObservableCollection<MessageDto> Messages
        {
            get { return _messages; }
            set { _messages = value; OnPropertyChanged(); }
        }
        private string _message;
        public string Message
        {
            get { return _message; }
            set { _message = value; OnPropertyChanged(); }
        }
        private readonly HubConnection HubConnection;
        #endregion

        #region Commands
        public ICommand SendMessageCommand { get; set; }
        #endregion

        public ClientTestPageViewModel()
        {
            Messages = new ObservableCollection<MessageDto>();
            var url = "http://localhost:49796/chat";
            var random = new Random();
            HubConnection = new HubConnectionBuilder().WithUrl(url, options =>
            {
                options.Headers["Email"] = "test@test.com";
                options.Headers["PhoneNumber"] = random.Next(555555555, 595555555).ToString();
                options.Headers["TicketType"] = "Suggestion";
                options.Headers["FirstName"] = "Smith " + random.Next(1, 999);
            }).Build();
            Init();
        }
        public override void Init()
        {
            InitCommands();
            Task.Run(async () => await Connect());
        }
        public override void InitCommands()
        {
            base.InitCommands();
            SendMessageCommand = new CustomCommand(async () =>
            {
                await SendMessage();
            });
        }
        private async Task Connect()
        {
            await HubConnection.StartAsync();
            HubConnection.On<string>("ReceiveMessage", (message) =>
            {
                var receivedMessage = new MessageDto { Message = message, MessageTime = DateTime.Now };
                Messages.Add(receivedMessage);
            });
        }
        private async Task SendMessage()
        {
            if (string.IsNullOrEmpty(Message))
                return;
            await HubConnection.InvokeAsync("SendMessageToSupporter", Message);
            Messages.Add(new MessageDto { Message = Message, IsUserMessage = true, MessageTime = DateTime.Now });
            Message = string.Empty;
        }
    }
}
