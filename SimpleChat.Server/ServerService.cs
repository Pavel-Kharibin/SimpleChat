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
            var clientCallback = OperationContext.Current.GetCallbackChannel<IClientCallback>();

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

            return Login(user, clientCallback);
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
            var callback = OperationContext.Current.GetCallbackChannel<IClientCallback>();

            var taskFactory = new TaskFactory();
            await taskFactory.StartNew(() =>
            {
                if (_clients.All(c => c.Value.User.Id != userId)) return;

                
                var clientInfo = _clients.First(c => c.Value.User.Id == userId);

                ClientConnection clientConnection;
                if (_clients.TryRemove(clientInfo.Value.User.Login, out clientConnection))
                {
                    foreach (
                        var conn in _clients.Where(c => c.Value != clientConnection && c.Value.Callback != callback))//  
                    {
                        conn.Value.Callback.UserLoggedOut(clientConnection.User);
                    }
                }

                clientInfo.Value.Callback.DoLogout("Администратор выгнал вас из чата.");
            });
        }

        public async Task<LoginOperationResult> RegisterAsync(string userName, string login, string password)
        {
            var clientCallback = OperationContext.Current.GetCallbackChannel<IClientCallback>();

            var loginExists = await _usersRepository.CheckLoginExists(login);
            if (loginExists)
            {
                return new LoginOperationResult
                {
                    OperationResult = OperationResult.Failure,
                    Message = $"Логин {login} уже зарегистрирован."
                };
            }

            var user = await _usersRepository.RegisterAsync(userName, login, password);

            return Login(user, clientCallback);
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

            foreach (var clientInfo in _clients.Where(c => c.Value.User.Id != message.UserId))
            {
                clientInfo.Value.Callback.SetMessage(message);
            }

            return new SendMessageOperationResult();
        }

        public IEnumerable<User> GetOnlineUsers()
        {
            var users = _clients.Select(c => c.Value.User).ToList();

            return users;
        }

        private LoginOperationResult Login(User user, IClientCallback clientCallback)
        {
            var newClientConnection = new ClientConnection
            {
                Callback = clientCallback,
                User = user
            };

            if (_clients.TryAdd(user.Login, newClientConnection))
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
    }
}