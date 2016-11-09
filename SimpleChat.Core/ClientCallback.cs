using System;
using System.ServiceModel;
using SimpleChat.Core.Contracts;
using SimpleChat.Core.Models;

namespace SimpleChat.Core
{
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class ClientCallback : IClientCallback
    {
        public event EventHandler<OnLogoutCommandEventArgs> OnLogoutCommand;
        public event EventHandler<OnNewMessageEventArgs> OnMessage;
        public event EventHandler<OnUserStatusChangesEventArgs> OnUserStatusChanged;
        public event EventHandler OnServerStopped;

        public void DoLogout(string message)
        {
             OnLogoutCommand?.Invoke(this, new OnLogoutCommandEventArgs { Message = message });
        }

        public void SetMessage(ChatMessage message)
        {
            OnMessage?.Invoke(this, new OnNewMessageEventArgs {Message = message});
        }

        public void UserLoggedIn(User user)
        {
            OnUserStatusChanged?.Invoke(this, new OnUserStatusChangesEventArgs {User = user, IsOnline = true});
        }

        public void UserLoggedOut(User user)
        {
            OnUserStatusChanged?.Invoke(this, new OnUserStatusChangesEventArgs { User = user, IsOnline = false });
        }

        public void DoServerStopped()
        {
            OnServerStopped?.Invoke(this, EventArgs.Empty);
        }
    }
}