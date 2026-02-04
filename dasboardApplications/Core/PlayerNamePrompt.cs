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
            this.Size = new Size(300, 150);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            Label label = new Label() { Left = 20, Top = 20, Text = "Enter your name:", Width = 250 };
            nameTextBox = new TextBox() { Left = 20, Top = 50, Width = 240 };
            okButton = new Button() { Text = "OK", Left = 190, Width = 70, Top = 80, DialogResult = DialogResult.OK };

            this.Controls.Add(label);
            this.Controls.Add(nameTextBox);
            this.Controls.Add(okButton);
            this.AcceptButton = okButton;

            okButton.Click += (s, e) => {
                if (!string.IsNullOrWhiteSpace(nameTextBox.Text))
                    PlayerName = nameTextBox.Text;
                this.Close();
            };
        }
    }
}
