using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Windows.Forms;
using mshtml;
using SimpleChat.Core;
using SimpleChat.Core.Contracts;
using SimpleChat.Core.Enums;
using SimpleChat.Core.Models;

namespace SimpleChat.Client
{
    public partial class MainForm : Form
    {
        private const string APP_NAME = "SimpleChat";
        private const int MAX_MESSAGES_COUNT = 10;
        public const string REQUIRED_FIELD = "Данное поле обязательно к заполнению!";

        private readonly List<User> _onlineUsers = new List<User>();
        private readonly IServerService _server;
        private User _user;
        private readonly WebBrowserWrapper _browserWrapper;
        private bool _doLogoutOnClose = true;

        public MainForm()
        {
            InitializeComponent();

            _browserWrapper = new WebBrowserWrapper(webBrowser);

            var clientCallback = new ClientCallback();
            clientCallback.OnLogoutCommand += (sender, args) =>
            {
                ResetForm();
                ShowMessageBox(args.Message, MessageBoxIcon.Warning);
            };
            clientCallback.OnMessage += (sender, args) => { ShowMessage(args.Message); };
            clientCallback.OnUserStatusChanged += (sender, args) =>
            {
                var message = new ChatMessage
                {
                    Message = args.IsOnline
                        ? $"Пользователь {args.User.Name} присоеденился к чату."
                        : $"Пользователь {args.User.Name} покинул чат."
                };
                ShowMessage(message);

                UpdateUsersList(args.User, args.IsOnline);
            };
            clientCallback.OnServerStopped += (sender, args) =>
            {
                _doLogoutOnClose = false;
                ShowMessageBox("Получена комманда остановки сервера.");
                Close();
            };

            var channelFactory = new DuplexChannelFactory<IServerService>(clientCallback, "SimpleChatServerEndPoint");
            _server = channelFactory.CreateChannel();
        }

        private void ResetForm()
        {
            _user = null;

            Text = APP_NAME;

            _browserWrapper.SetUserId(0);

            _browserWrapper.Clear();

            panel.Enabled = false;
            mainMenu_Chat_Login.Enabled = true;
            mainMenu_Chat_Register.Enabled = true;
            lstUsers.Items.Clear();

            _onlineUsers.Clear();
        }

        private async void Init(User user)
        {
            _user = user;

            Text = $"{APP_NAME} - {_user.Name}";

            _browserWrapper.SetUserId(_user.Id);

            panel.Enabled = true;
            mainMenu_Chat_Login.Enabled = false;
            mainMenu_Chat_Register.Enabled = false;

            var messagesResult = await _server.LoadMessagesAsync(_user.IsAdmin ? (int?) null : MAX_MESSAGES_COUNT);
            if (messagesResult.OperationResult == OperationResult.Success)
            {
                var messages = messagesResult.Messages.ToList();
                messages.ForEach(m => ShowMessage(m, false));
                _browserWrapper.ScrollToBottom();
            }
            else
            {
                ShowMessageBox($"Не удалось загрузить список сообщений:\n{messagesResult.Message}", MessageBoxIcon.Error);
            }

            var result = await _server.GetOnlineUsersAsync();
            result.ToList().ForEach(u => UpdateUsersList(u, true));
        }

        private void ShowMessage(ChatMessage message, bool scroll = true, bool animate = true)
        {
            _browserWrapper.ShowMessage(message, scroll, animate);

            if (!_user.IsAdmin) _browserWrapper.RemoveOldMessages(MAX_MESSAGES_COUNT);
        }

        private void ShowMessage(string message)
        {
            var chatMessage = new ChatMessage {Message = message};

            ShowMessage(chatMessage, true, false);
        }

        private void UpdateUsersList(User user, bool isLogged)
        {
            if (isLogged)
            {
                if (_onlineUsers.All(u => u.Id != user.Id))
                    _onlineUsers.Add(user);
            }
            else
            {
                var userToRemove = _onlineUsers.FirstOrDefault(u => u.Id == user.Id);
                if (userToRemove != null)
                    _onlineUsers.Remove(userToRemove);
            }

            lstUsers.Items.Clear();
            _onlineUsers.ForEach(u => lstUsers.Items.Add(u.Name));
        }

        private void ShowMessageBox(string message, MessageBoxIcon icon = MessageBoxIcon.Information)
        {
            MessageBox.Show(this, message, APP_NAME, MessageBoxButtons.OK, icon);
        }

        private void mainMenu_Chat_Login_Click(object sender, EventArgs e)
        {
            var loginForm = new LoginForm();

            loginForm.OnTryLogon += async (o, args) =>
            {
                loginForm.Enabled = false;

                var progress = new ProgressForm("Выполняется вход в чат...");
                progress.ShowCenterParent(loginForm);

                var result = await _server.LoginAsync(args.Login, args.Password);

                progress.Close();

                if (result.OperationResult == OperationResult.Success)
                {
                    loginForm.Close();
                    Init(result.User);
                }
                else
                {
                    loginForm.Enabled = true;

                    ShowMessageBox(result.Message, MessageBoxIcon.Exclamation);
                }
            };

            loginForm.ShowCenterParent(this);
        }

        private void mainMenu_Chat_Register_Click(object sender, EventArgs e)
        {
            var registerForm = new RegisterForm();

            registerForm.OnTryRegister += async (o, args) =>
            {
                registerForm.Enabled = false;

                var progressForm = new ProgressForm("Выполняется регистрация");
                progressForm.ShowCenterParent(registerForm);

                var result = await _server.RegisterAsync(args.UserName, args.Login, args.Password);

                progressForm.Close();

                if (result.OperationResult == OperationResult.Success)
                {
                    registerForm.Close();
                    Init(result.User);
                }
                else
                {
                    registerForm.Enabled = true;

                    ShowMessageBox(result.Message, MessageBoxIcon.Exclamation);
                }
            };

            registerForm.ShowCenterParent(this);
        }

        private void mainMenu_Chat_Exit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void contextMenu_Logout_Click(object sender, EventArgs e)
        {
            var selectedIdx = lstUsers.SelectedIndex;
            if (selectedIdx == -1) return;

            var user = _onlineUsers[selectedIdx];

            _server.LogoutUserAsync(user.Id);

            UpdateUsersList(user, false);
            ShowMessage($"Вы выгнали пользователя {user.Name} из чата.");
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMessage.Text)) return;
            
            var chatMessage = new ChatMessage
            {
                Id = Guid.NewGuid(),
                Message = txtMessage.Text,
                Sent = DateTime.Now,
                UserId = _user.Id
            };

            txtMessage.Text = "";

            ShowMessage(chatMessage, true, false);

            var result = await _server.SendMessageAsync(chatMessage);

            if (result.OperationResult != OperationResult.Success)
            {
                ShowMessageBox($"Не удалось отправить сообщение.\n{result.Message}", MessageBoxIcon.Warning);
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_doLogoutOnClose) return;

            try
            {
                if (_user != null)
                    _server.LogoutAsync();
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void lstUsers_MouseUp(object sender, MouseEventArgs e)
        {
            if (!_user.IsAdmin || e.Button != MouseButtons.Right) return;

            var selectedIdx = lstUsers.IndexFromPoint(e.Location);
            if (selectedIdx == -1) return;

            var user = _onlineUsers[selectedIdx];

            if (user.Id == _user.Id) return;
            contextMenu.Show(lstUsers, e.Location);
            lstUsers.SelectedIndex = selectedIdx;
        }
    }

    internal class WebBrowserWrapper
    {
        private const string CHAT_DOCUMENT = "<!DOCTYPE html><html><head>" +
                                             "<meta http-equiv='X-UA-Compatible' content='IE=edge,chrome=1'>" +
                                             "</head><body></body></html>";

        private readonly WebBrowser _webBrowser;

        public WebBrowserWrapper(WebBrowser webBrowser)
        {
            _webBrowser = webBrowser;

            _webBrowser.DocumentCompleted += (sender, args) =>
            {
                var currentPath = Path.GetDirectoryName(Application.ExecutablePath);

                var doc = (_webBrowser.Document?.DomDocument) as IHTMLDocument2;
                var styleSheet = doc?.createStyleSheet("", 0);
                styleSheet.cssText = File.ReadAllText(Path.Combine(currentPath, "chat/chat.css"));

                var head = _webBrowser.Document?.GetElementsByTagName("head")[0];

                var jqscript = _webBrowser.Document?.CreateElement("script");
                jqscript.SetAttribute("text", File.ReadAllText(Path.Combine(currentPath, "chat/jquery.min.js")));
                head.AppendChild(jqscript);

                var script = _webBrowser.Document?.CreateElement("script");
                script.SetAttribute("text", File.ReadAllText(Path.Combine(currentPath, "chat/chat.js")));
                head.AppendChild(script);
            };

            _webBrowser.DocumentText = CHAT_DOCUMENT;
        }

        public void Clear()
        {
            if (_webBrowser.Document?.Body != null)
                _webBrowser.Document.Body.InnerHtml = "";
        }

        public void ScrollToBottom()
        {
            _webBrowser.Document?.InvokeScript("ScrollToBottom");
        }

        public void ShowMessage(ChatMessage message, bool scroll, bool animate)
        {
            _webBrowser.Document?.InvokeScript("ShowMessage",
                new object[]
                {
                    message.Id.Equals(Guid.Empty) ? "" : message.Id.ToString(),
                    message.UserId,
                    message.User?.Name,
                    message.Sent.ToString("dd.MM.yyyy hh:mm:ss"),
                    message.Message,
                    scroll,
                    animate
                });
        }

        public void RemoveOldMessages(int maxMessages)
        {
            _webBrowser.Document?.InvokeScript("RemoveOldMessages", new object[] { maxMessages });
        }

        public void SetUserId(int id)
        {
            _webBrowser.Document?.InvokeScript("SetUserId", new object[] {id});
        }
    }
}