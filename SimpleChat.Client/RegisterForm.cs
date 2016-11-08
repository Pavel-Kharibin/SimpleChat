using System;

namespace SimpleChat.Client
{
    public partial class RegisterForm : FormBase
    {
        public RegisterForm()
        {
            InitializeComponent();
        }

        public event EventHandler<OnTryRegisterEventArgs> OnTryRegister;

        private void btnRegister_Click(object sender, EventArgs e)
        {
            OnTryRegister?.Invoke(this, new OnTryRegisterEventArgs
            {
                UserName = txtUserName.Text,
                Login = txtLogin.Text,
                Password = txtPassword.Text
            });
        }
    }

    public class OnTryRegisterEventArgs
    {
        public string UserName { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
