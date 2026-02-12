using System;
using System.Windows.Forms;
using System.Drawing;

namespace dasboardApplications.Core
{
    /// <summary>
    /// A simple dialog form to prompt the player for their name.
    /// Used across multiple games to track winners.
    /// </summary>
    public class PlayerNamePrompt : Form
    {
        private TextBox nameTextBox;
        private Button okButton;
        public string PlayerName { get; private set; } = "Anonymous";

        public PlayerNamePrompt(string title)
        {
            this.Text = title;
            this.Size = new Size(400, 220);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = UITheme.PrimaryBackground;

            Panel container = new Panel {
                Dock = DockStyle.Fill,
                Padding = new Padding(30)
            };
            this.Controls.Add(container);

            Label label = new Label {
                Text = "Enter your name:",
                AutoSize = true,
                ForeColor = UITheme.TextSecondary,
                Font = UITheme.BodyFont,
                Location = new Point(30, 30)
            };

            nameTextBox = new TextBox {
                Width = 330,
                Location = new Point(30, 65),
                Font = UITheme.BodyFont
            };
            UITheme.StyleTextBox(nameTextBox);

            okButton = new Button {
                Text = "CONFIRM",
                Width = 140,
                Height = 40,
                Location = new Point(220, 115),
                DialogResult = DialogResult.OK
            };
            UITheme.StyleButton(okButton, isPrimary: true);

            container.Controls.Add(label);
            container.Controls.Add(nameTextBox);
            container.Controls.Add(okButton);
            this.AcceptButton = okButton;

            okButton.Click += (s, e) => {
                if (!string.IsNullOrWhiteSpace(nameTextBox.Text))
                    PlayerName = nameTextBox.Text;
                this.Close();
            };
        }
    }
}
