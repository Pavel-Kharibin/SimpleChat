using System;

namespace SimpleChat.Client
{
    public partial class RegisterForm : FormBase
    {
        public RegisterForm()
        {
            InitializeComponent();

            errorProvider.SetIconPadding(txtUserName, 1);
            errorProvider.SetIconPadding(txtLogin, 1);
            errorProvider.SetIconPadding(txtPassword, 1);
        }

        public event EventHandler<OnTryRegisterEventArgs> OnTryRegister;

        private void btnRegister_Click(object sender, EventArgs e)
        {
            ValidateChildren();

            if (!string.IsNullOrWhiteSpace(errorProvider.GetError(txtUserName)) ||
                !string.IsNullOrWhiteSpace(errorProvider.GetError(txtLogin)) ||
                !string.IsNullOrWhiteSpace(errorProvider.GetError(txtPassword))) return;

            OnTryRegister?.Invoke(this, new OnTryRegisterEventArgs
            {
                UserName = txtUserName.Text,
                Login = txtLogin.Text,
                Password = txtPassword.Text
            });
        }

        private void txtUserName_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            errorProvider.SetError(txtUserName,
                string.IsNullOrWhiteSpace(txtUserName.Text) ? MainForm.REQUIRED_FIELD : string.Empty);
        }

        private void txtLogin_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            errorProvider.SetError(txtLogin,
                string.IsNullOrWhiteSpace(txtLogin.Text) ? MainForm.REQUIRED_FIELD : string.Empty);
        }

        private void txtPassword_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            errorProvider.SetError(txtPassword,
                string.IsNullOrWhiteSpace(txtPassword.Text) ? MainForm.REQUIRED_FIELD : string.Empty);
        }
    }

    public class OnTryRegisterEventArgs
    {
        public string UserName { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
