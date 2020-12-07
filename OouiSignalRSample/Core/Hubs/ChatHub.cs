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
    public class ChatHub : Hub<IChatHub>
    {
        #region Headers
        private const string PHONE_NUMBER = "PhoneNumber";
        private const string EMAIL = "Email";
        private const string TICKET_TYPE = "TicketType";
        private const string FIRST_NAME = "FirstName";
        #endregion

        public async override Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            if (ConnectedUsers.Current.BannedUsers.Contains(httpContext.Request.Headers[PHONE_NUMBER].FirstOrDefault()))
            { 
                return;
            }
            var oldUser = ConnectedUsers.Current.ActiveUsers.FirstOrDefault(x => x.PhoneNumber == httpContext.Request.Headers[PHONE_NUMBER].FirstOrDefault());
            if (oldUser != null)
            {
                oldUser.ClientId = Context.ConnectionId;
                return;
            }
            ConnectedUsers.Current.ActiveUsers.Add(new User
            {
                ClientId = Context.ConnectionId,
                Messages = new List<MessageDto>(),
                Email = httpContext.Request.Headers[EMAIL].FirstOrDefault(),
                PhoneNumber = httpContext.Request.Headers[PHONE_NUMBER].FirstOrDefault(),
                TicketType = httpContext.Request.Headers[TICKET_TYPE].FirstOrDefault(),
                UserName = httpContext.Request.Headers[FIRST_NAME].FirstOrDefault(),
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
            var httpContext = Context.GetHttpContext();
            if (ConnectedUsers.Current.BannedUsers.Contains(httpContext.Request.Headers[PHONE_NUMBER].FirstOrDefault()))
            {
                Clients.Client(Context.ConnectionId).ReceiveMessage("You are banned from server.");
                return;
            }
            var user = ConnectedUsers.Current.ActiveUsers.FirstOrDefault(x => x.ClientId == Context.ConnectionId);
            if (user != null)
            {
                user.Messages.Add(new MessageDto { Message = message, IsUserMessage = true, MessageTime = DateTime.Now });
                if (user.IsSelected)
                {
                    user.LastMessageTime = DateTime.UtcNow.AddHours(3);
                    user.LastMessage = message.Length > 10 ? message.Substring(0, 10) + "..." : message;
                    ConnectedUsers.Current.SelectedUserMessages.Clear();
                    ConnectedUsers.Current.SelectedUserMessages = new ObservableCollection<MessageDto>(user.Messages);
                }
                else
                {
                    user.UnreadedMessageCount += 1;
                    user.LastMessageTime = DateTime.UtcNow.AddHours(3);
                    user.LastMessage = message.Length > 20 ? message.Substring(0, 20) + "..." : message;
                }
            }
        }
    }
}
