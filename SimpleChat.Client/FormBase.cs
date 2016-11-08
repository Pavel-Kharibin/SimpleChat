using System.Drawing;
using System.Windows.Forms;

namespace SimpleChat.Client
{
    public class FormBase : Form
    {
        public void ShowCenterParent(Form parent)
        {
            StartPosition = FormStartPosition.Manual;
            Location = new Point(parent.Location.X + (parent.Width - Width)/2,
                parent.Location.Y + (parent.Height - Height)/2);

            Show(parent);
        }
    }
}