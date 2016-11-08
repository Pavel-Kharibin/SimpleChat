namespace SimpleChat.Client
{
    public partial class ProgressForm : FormBase
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
