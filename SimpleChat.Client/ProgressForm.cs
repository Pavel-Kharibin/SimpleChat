using System.Windows.Forms;

namespace SimpleChat.Client
{
    public partial class ProgressForm : Form
    {
        public ProgressForm()
        {
            InitializeComponent();
        }

        public ProgressForm(string message)
            : this()
        {
            lblAction.Text = message;
            //LostFocus += (sender, args) => { Focus(); };
        }
    }
}
