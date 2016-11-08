using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using SimpleChat.Core.Contracts;
using SimpleChat.Core.Enums;
using SimpleChat.Core.Models;
using SimpleChat.Core.OperationResults;
using SimpleChat.Data.Repository;

namespace SimapleChat.Server
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single,
        IncludeExceptionDetailInFaults = true)]
    public class ServerService : IServerService
    {
        private readonly ChatMessagesRepository _chatMessagesRepository = new ChatMessagesRepository();

        private readonly ConcurrentDictionary<string, ClientConnection> _clients =
            new ConcurrentDictionary<string, ClientConnection>();

        private readonly UsersRepository _usersRepository = new UsersRepository();

        public async Task<LoginOperationResult> LoginAsync(string login, string password)
        {
            var clientCallBack = OperationContext.Current.GetCallbackChannel<IClientCallback>();

            if (_clients.Keys.Any(k => string.Equals(k, login, StringComparison.InvariantCultureIgnoreCase)))
            {
                var clientInfo = _clients.First(c => string.Equals(c.Key, login, StringComparison.InvariantCultureIgnoreCase));

                ClientConnection connectionToRemove;
                if (_clients.TryRemove(login, out connectionToRemove))
                {
                    clientInfo.Value.Callback.DoLogout("Ваша учетная запись использована на другом компьютере!");
                }
            }

            var user = await _usersRepository.LoginAsync(login, password);

            if (user == null)
                return new LoginOperationResult
                {
                    OperationResult = OperationResult.Failure,
                    Message = "Не верный логин или пароль."
                };

            var newClientConnection = new ClientConnection
            {
                Callback = clientCallBack,
                User = user
            };

            if (_clients.TryAdd(login, newClientConnection))
            {
                foreach (var conn in _clients.Where(c => c.Value != newClientConnection))
                {
                    conn.Value.Callback.UserLoggedIn(newClientConnection.User);
                }

                return new LoginOperationResult
                {
                    User = newClientConnection.User
                };
            }

            return new LoginOperationResult
            {
                OperationResult = OperationResult.Failure,
                Message = "Не удалось зврегистрировать клиентское соединение."
            };
        }

        public void Logout()
        {
            var callback = OperationContext.Current.GetCallbackChannel<IClientCallback>();
            if (_clients.All(c => c.Value.Callback != callback)) return;

            var clientInfo = _clients.First(c => c.Value.Callback == callback);

            ClientConnection clientConnection;
            if (_clients.TryRemove(clientInfo.Value.User.Login, out clientConnection))
            {
                foreach (var conn in _clients.Where(c => c.Value != clientConnection))
                {
                    conn.Value.Callback.UserLoggedOut(clientConnection.User);
                }
            }
        }

        public async void LogoutUserAsync(int userId)
        {
            var taskFactory = new TaskFactory();
            await taskFactory.StartNew(() =>
            {
                if (_clients.All(c => c.Value.User.Id != userId)) return;

                //var callback = OperationContext.Current.GetCallbackChannel<IClientCallback>();
                var clientInfo = _clients.First(c => c.Value.User.Id == userId);

                ClientConnection clientConnection;
                if (_clients.TryRemove(clientInfo.Value.User.Login, out clientConnection))
                {
                    foreach (
                        var conn in _clients.Where(c => c.Value != clientConnection))//  && c.Value.Callback != callback
                    {
                        conn.Value.Callback.UserLoggedOut(clientConnection.User);
                    }
                }

                clientInfo.Value.Callback.DoLogout("Администратор выгнал вас из чата.");
            });
        }

        public async Task<LoadMessagesOperationResult> LoadMessagesAsync(int? top = null)
        {
            var messages = await _chatMessagesRepository.GetMessagesAsync(top);

            return new LoadMessagesOperationResult
            {
                Messages = messages
            };
        }

        public async Task<SendMessageOperationResult> SendMessageAsync(ChatMessage message, bool save = true)
        {
            if (save)
            {
                await _chatMessagesRepository.AddMessageAsync(message);
            }

            foreach (
                var clientConnection in
                    _clients.Where(clientConnection => clientConnection.Value.User.Id != message.UserId))
            {
                clientConnection.Value.Callback.SetMessage(message);
            }

            return new SendMessageOperationResult();
        }

        public IEnumerable<User> GetOnlineUsers()
        {
            var users = _clients.Select(c => c.Value.User).ToList();

            return users;
        }
    }
}