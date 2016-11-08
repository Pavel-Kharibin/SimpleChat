using System;
using System.ServiceModel;
using SimpleChat.Core.Models;

namespace SimpleChat.Core.Contracts
{
    public interface IClientCallback
    {
        event EventHandler<OnLogoutCommandEventArgs> OnLogoutCommand;
        event EventHandler<OnNewMessageEventArgs> OnMessage;
        event EventHandler<OnUserStatusChangesEventArgs> OnUserStatusChanged;

        [OperationContract]
        void DoLogout(string message);

        [OperationContract]
        void SetMessage(ChatMessage message);

        [OperationContract]
        void UserLoggedIn(User user);

        [OperationContract]
        void UserLoggedOut(User user);
    }


    public class OnLogoutCommandEventArgs
    {
        public string Message { get; set; }
    }

    public class OnNewMessageEventArgs
    {
        public ChatMessage Message { get; set; }
    }

    public class OnUserStatusChangesEventArgs
    {
        public User User { get; set; }
        public bool IsOnline { get; set; }
    }
}