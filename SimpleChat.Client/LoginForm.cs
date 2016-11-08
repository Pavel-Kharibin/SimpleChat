using System;

namespace SimpleChat.Client
{
    public partial class LoginForm : FormBase
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        public event EventHandler<OnTryLogonEventArgs> OnTryLogon;

        public string Login => txtLogin.Text;
        public string Password => txtPassword.Text;

        private void btnLogin_Click(object sender, EventArgs e)
        {
            OnTryLogon?.Invoke(this, new OnTryLogonEventArgs
            {
                Login = txtLogin.Text,
                Password = txtPassword.Text
            });
        }
    }

    public class OnTryLogonEventArgs
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
