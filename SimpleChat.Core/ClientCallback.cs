using System.ServiceModel;
using SimpleChat.Core.Contracts;
using SimpleChat.Core.Models;

namespace SimpleChat.Core
{
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class ClientCallback : IClientCallback
    {
        public event OnLogoutCommandDelegate OnLogoutCommand;
        public event OnNewMessageDelegate OnMessage;
        public event OnUserStatusChangedDelegate OnUserStatusChanged;
        
        public void DoLogout(string message)
        {
            OnLogoutCommand?.Invoke(this, new OnLogoutCommandEventArgs {Message = message});
        }

        public void SetMessage(ChatMessage message)
        {
            OnMessage?.Invoke(this, new OnNewMessageEventArgs {Message = message});
        }

        public void UserLoggedIn(User user)
        {
            OnUserStatusChanged?.Invoke(this, new UserStatusChangesEventArgs {User = user, IsOnline = true});
        }

        public void UserLoggedOut(User user)
        {
            OnUserStatusChanged?.Invoke(this, new UserStatusChangesEventArgs { User = user, IsOnline = false });
        }
    }

    public delegate void OnLogoutCommandDelegate(object sender, OnLogoutCommandEventArgs e);

    public class OnLogoutCommandEventArgs
    {
        public string Message { get; set; }
    }

    public delegate void OnNewMessageDelegate(object sender, OnNewMessageEventArgs e);

    public class OnNewMessageEventArgs
    {
        public ChatMessage Message { get; set; }
    }

    public delegate void OnUserStatusChangedDelegate(object sender, UserStatusChangesEventArgs e);

    public class UserStatusChangesEventArgs
    {
        public User User { get; set; }
        public bool IsOnline { get; set; }
    }
}