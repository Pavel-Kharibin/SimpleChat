using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleChat.Client
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        public string Login => txtLogin.Text;
        public string Password => txtPassword.Text;

        private void btnLogin_Click(object sender, EventArgs e)
        {
            
        }
    }
}
