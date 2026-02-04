using System.Windows.Forms;
using dasboardApplications.Core;

namespace dasboardApplications
{
    public partial class AuthenticationForm : Form
    {
        public AuthenticationForm()
        {
            InitializeComponent();
            ApplyTheme();
        }

        private void ApplyTheme()
        {
            this.BackColor = UITheme.ContentBackground;
            titleLabel.ForeColor = UITheme.PrimaryBackground;
            titleLabel.Font = UITheme.HeaderFont;

            signInButton.BackColor = UITheme.AccentColor;
            signInButton.ForeColor = Color.White;
            signInButton.FlatStyle = FlatStyle.Flat;
            signInButton.FlatAppearance.BorderSize = 0;
            signInButton.Font = UITheme.ButtonFont;
        }

        private void signInButton_Click(object sender, EventArgs e)
        {
            Dashboard dsh = new Dashboard();
            dsh.Show();
            this.Hide();
        }
    }
}
