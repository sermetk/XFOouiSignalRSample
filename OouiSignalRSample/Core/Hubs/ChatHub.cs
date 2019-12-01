using Microsoft.AspNetCore.SignalR;
using OouiSignalRSample.Modules;
using OouiSignalRSample.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace OouiSignalRSample.Core.Hubs
{
    public class ChatHub : Hub
    {
        public async override Task OnConnectedAsync()
        {
            var httpOptions = Context.GetHttpContext();
            if (!string.IsNullOrEmpty(httpOptions.Request.Headers["Supporter"].ToString()))
                return;

            if (ConnectedUsers.Current.BannedUsers.Contains(httpOptions.Request.Headers["PhoneNumber"].ToString()))
            {
                await Clients.Client(Context.ConnectionId).SendAsync("Banned");
                return;
            }

            await Task.Delay(50).ContinueWith(async c => await Clients.Client(Context.ConnectionId).SendAsync("Connected"));
            var oldUser = ConnectedUsers.Current.ActiveUsers.FirstOrDefault(x => x.PhoneNumber == httpOptions.Request.Headers["PhoneNumber"].ToString());
            if (oldUser != null)
            {
                oldUser.ClientId = Context.ConnectionId;
                return;
            }
            ConnectedUsers.Current.ActiveUsers.Add(
                new User
                {
                    ClientId = Context.ConnectionId,
                    Messages = new List<MessageDto>(),
                    Email = httpOptions.Request.Headers["Email"].ToString(),
                    PhoneNumber = httpOptions.Request.Headers["PhoneNumber"].ToString(),
                    TicketType = httpOptions.Request.Headers["TicketType"].ToString(),
                    UserName = httpOptions.Request.Headers["FirstName"].ToString(),
                    LastMessageTime = DateTime.UtcNow.AddHours(3)
                });
            await base.OnConnectedAsync();
        }
        public async override Task OnDisconnectedAsync(Exception exception)
        {
            var removedUser = ConnectedUsers.Current.ActiveUsers.FirstOrDefault(x => x.ClientId == Context.ConnectionId);
            if (ConnectedUsers.Current.ActiveUsers.Contains(removedUser))
                ConnectedUsers.Current.ActiveUsers.Remove(removedUser);
            await base.OnDisconnectedAsync(exception);
        }
        public void SendMessageToSupporter(string message)
        {
            var user = ConnectedUsers.Current.ActiveUsers.FirstOrDefault(x => x.ClientId == Context.ConnectionId);
            if (user != null)
            {
                user.Messages.Add(new MessageDto { Message = message, IsUserMessage = true, MessageTime = DateTime.Now });
                if (user.IsSelected)
                {
                    user.LastMessageTime = DateTime.UtcNow.AddHours(3);
                    user.LastMessage = message.Length > 10 ? message.Substring(0, 10) + "..." : message;
                    ConnectedUsers.Current.SelectedUserMessages.Clear();
                    ConnectedUsers.Current.SelectedUserMessages = ToObservableCollection(user.Messages);
                }
                else
                {
                    user.UnreadedMessageCount += 1;
                    user.LastMessageTime = DateTime.UtcNow.AddHours(3);
                    user.LastMessage = message.Length > 20 ? message.Substring(0, 20) + "..." : message;
                }
            }
        }
        public async void SendMessageToCustomer(string connectionId, string message)
        {
            await Clients.Client(connectionId).SendAsync("ReceiveMessage", message);
            var user = ConnectedUsers.Current.ActiveUsers.FirstOrDefault(x => x.ClientId == connectionId);
            if (user != null)
            {
                user.Messages.Add(new MessageDto { Message = message, MessageTime = DateTime.UtcNow.AddHours(3) });
                if (user.IsSelected)
                {
                    ConnectedUsers.Current.SelectedUserMessages.Clear();
                    ConnectedUsers.Current.SelectedUserMessages = ToObservableCollection(user.Messages);
                }
            }
        }
        private ObservableCollection<T> ToObservableCollection<T>(IEnumerable<T> _LinqResult)
        {
            return new ObservableCollection<T>(_LinqResult);
        }
        public async void BlockUser(string connectionId)
        {
            await Clients.Client(connectionId).SendAsync("Banned");
        }
    }
}
