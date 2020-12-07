using OouiSignalRSample.Core;
using System;
using System.Collections.Generic;

namespace OouiSignalRSample.Modules
{
    public class User : BindableBase
    {
        public string ClientId { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string TicketType { get; set; }

        public bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; OnPropertyChanged(); }
        }
        public string _lastMessage;
        public string LastMessage
        {
            get { return _lastMessage; }
            set { _lastMessage = value; OnPropertyChanged(); }
        }
        public DateTime _lastMessageTime;
        public DateTime LastMessageTime
        {
            get { return _lastMessageTime; }
            set { _lastMessageTime = value; OnPropertyChanged(); }
        }
        public int _unreadedMessageCount;
        public int UnreadedMessageCount
        {
            get { return _unreadedMessageCount; }
            set { _unreadedMessageCount = value; OnPropertyChanged(); }
        }
        public List<MessageDto> Messages { get; set; }
    }
    public class MessageDto
    {
        public string Message { get; set; }
        public DateTime MessageTime { get; set; }
        public bool IsUserMessage { get; set; }
    }
}
