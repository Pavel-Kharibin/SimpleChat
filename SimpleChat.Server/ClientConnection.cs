using SimpleChat.Core.Contracts;
using SimpleChat.Core.Models;

namespace SimapleChat.Server
{
    public class ClientConnection
    {
        public IClientCallback Callback { get; set; }
        public User User { get; set; }
    }
}