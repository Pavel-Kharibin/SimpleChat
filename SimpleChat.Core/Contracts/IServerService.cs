using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using SimpleChat.Core.Models;
using SimpleChat.Core.OperationResults;

namespace SimpleChat.Core.Contracts
{
    [ServiceContract(CallbackContract = typeof (IClientCallback))]
    public interface IServerService
    {
        [OperationContract]
        Task<LoginOperationResult> LoginAsync(string login, string password);

        [OperationContract]
        void Logout();

        [OperationContract]
        void LogoutUserAsync(int userId);

        [OperationContract]
        Task<LoginOperationResult> RegisterAsync(string userName, string login, string password);

        [OperationContract]
        Task<LoadMessagesOperationResult> LoadMessagesAsync(int? top = null);

        [OperationContract]
        Task<SendMessageOperationResult> SendMessageAsync(ChatMessage message, bool save = true);

        [OperationContract]
        IEnumerable<User> GetOnlineUsers();
    }
}