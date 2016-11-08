using System.ServiceModel;
using SimpleChat.Core.Models;

namespace SimpleChat.Core.Contracts
{
    public interface IClientCallback
    {
        event OnLogoutCommandDelegate OnLogoutCommand;
        event OnNewMessageDelegate OnMessage;
        event OnUserStatusChangedDelegate OnUserStatusChanged;

        [OperationContract]
        void DoLogout(string message);

        [OperationContract]
        void SetMessage(ChatMessage message);

        [OperationContract]
        void UserLoggedIn(User user);

        [OperationContract]
        void UserLoggedOut(User user);
    }
}