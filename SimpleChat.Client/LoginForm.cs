using System;
using System.ComponentModel;

namespace SimpleChat.Client
{
    public partial class LoginForm : FormBase
    {
        public LoginForm()
        {
            InitializeComponent();

            errorProvider.SetIconPadding(txtLogin, 1);
            errorProvider.SetIconPadding(txtPassword, 1);
        }

        public event EventHandler<OnTryLogonEventArgs> OnTryLogon;

        private void btnLogin_Click(object sender, EventArgs e)
        {
            ValidateChildren();

            if (!string.IsNullOrWhiteSpace(errorProvider.GetError(txtLogin)) ||
                !string.IsNullOrWhiteSpace(errorProvider.GetError(txtPassword))) return;

            OnTryLogon?.Invoke(this, new OnTryLogonEventArgs
            {
                Login = txtLogin.Text,
                Password = txtPassword.Text
            });
        }

        private void txtLogin_Validating(object sender, CancelEventArgs e)
        {
            errorProvider.SetError(txtLogin,
                string.IsNullOrWhiteSpace(txtLogin.Text) ? MainForm.REQUIRED_FIELD : string.Empty);
        }

        private void txtPassword_Validating(object sender, CancelEventArgs e)
        {
            errorProvider.SetError(txtPassword,
                string.IsNullOrWhiteSpace(txtPassword.Text) ? MainForm.REQUIRED_FIELD : string.Empty);
        }
    }
}

public class OnTryLogonEventArgs
{
    public string Login { get; set; }
    public string Password { get; set; }
}