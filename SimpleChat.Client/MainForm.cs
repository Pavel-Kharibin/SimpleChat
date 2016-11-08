using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Windows.Forms;
using SimpleChat.Core;
using SimpleChat.Core.Contracts;
using SimpleChat.Core.Enums;
using SimpleChat.Core.Models;

namespace SimpleChat.Client
{
    public partial class MainForm : Form
    {
        private const string APP_NAME = "SimpleChat";
        private readonly List<User> _onlineUsers = new List<User>();
        private readonly IServerService _server;
        private User _user;

        public MainForm()
        {
            InitializeComponent();

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

            var channelFactory = new DuplexChannelFactory<IServerService>(clientCallback, "SimpleChatServerEndPoint");
            _server = channelFactory.CreateChannel();
        }

        private void ResetForm()
        {
            txtMessages.Clear();

            panel.Enabled = false;
            mainMenu_Chat_Login.Enabled = true;
            mainMenu_Chat_Register.Enabled = true;
            lstUsers.Items.Clear();

            _user = null;
            _onlineUsers.Clear();
        }

        private async void Init(User user)
        {
            _user = user;

            panel.Enabled = true;
            mainMenu_Chat_Login.Enabled = false;
            mainMenu_Chat_Register.Enabled = false;

            var messagesResult = await _server.LoadMessagesAsync(_user.IsAdmin ? (int?) null : 10);
            foreach (var message in messagesResult.Messages)
            {
                ShowMessage(message);
            }

            var onlineUsers = _server.GetOnlineUsers().ToList();
            onlineUsers.ForEach(u => UpdateUsersList(u, true));
        }

        private void ShowMessage(ChatMessage message)
        {
            if (!message.Id.Equals(Guid.Empty))
            {
                txtMessages.Text +=
                    $"{(message.UserId == _user.Id ? "Вы" : message.User?.Name)} ({message.Sent}): {message.Message}\r\n";
            }
            else
            {
                txtMessages.Text += $"({message.Sent}): {message.Message}\r\n";
            }
        }

        private void ShowMessage(string message)
        {
            var chatMessage = new ChatMessage {Message = message};

            ShowMessage(chatMessage);
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

                var result = await _server.LoginAsync(loginForm.Login, loginForm.Password);

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

            ShowMessage(chatMessage);

            var result = await _server.SendMessageAsync(chatMessage);

            if (result.OperationResult == OperationResult.Success)
            {
                txtMessage.Text = "";
            }
            else
            {
                ShowMessageBox(result.Message, MessageBoxIcon.Warning);
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (_user != null)
                    _server.Logout();
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
}