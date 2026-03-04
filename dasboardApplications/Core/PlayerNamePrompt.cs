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
        private Panel headerPanel;
        public string PlayerName { get; private set; } = "Anonymous";

        public PlayerNamePrompt(string title)
        {
            this.Text = title;
            this.Size = new Size(420, 260); // Slightly larger for better spacing
            this.FormBorderStyle = FormBorderStyle.None; // Custom border/header
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = UITheme.PrimaryBackground;

            // Custom Header
            headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 50,
                BackColor = UITheme.SecondaryBackground
            };
            this.Controls.Add(headerPanel);

            Label titleLabel = new Label
            {
                Text = title.ToUpper(),
                AutoSize = true,
                Location = new Point(20, 15),
                ForeColor = UITheme.TextPrimary,
                Font = UITheme.ButtonFont
            };
            headerPanel.Controls.Add(titleLabel);

            // Close button in header
            Button closeBtn = new Button
            {
                Text = "✕",
                Size = new Size(30, 30),
                Location = new Point(380, 10),
                FlatStyle = FlatStyle.Flat,
                ForeColor = UITheme.TextSecondary,
                Cursor = Cursors.Hand
            };
            closeBtn.FlatAppearance.BorderSize = 0;
            closeBtn.Click += (s, e) => this.Close();
            headerPanel.Controls.Add(closeBtn);

            Panel contentArea = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(30),
                BackColor = Color.Transparent // Ensure background doesn't block painting
            };
            this.Controls.Add(contentArea);

            Label label = new Label
            {
                Text = "Enter your name:",
                AutoSize = true,
                ForeColor = UITheme.TextSecondary,
                Font = UITheme.BodyFont,
                Location = new Point(30, 25)
            };
            contentArea.Controls.Add(label);

            nameTextBox = new TextBox
            {
                Width = 360,
                Location = new Point(30, 55),
                Font = UITheme.BodyFont
            };
            UITheme.StyleTextBox(nameTextBox);
            contentArea.Controls.Add(nameTextBox);

            okButton = new Button
            {
                Text = "CONFIRM",
                Width = 120,
                Height = 40,
                Location = new Point(270, 125)
            };
            UITheme.StyleButton(okButton, isPrimary: true);
            contentArea.Controls.Add(okButton);

            this.AcceptButton = okButton;

            okButton.Click += (s, e) => {
                if (!string.IsNullOrWhiteSpace(nameTextBox.Text))
                    PlayerName = nameTextBox.Text;
                this.DialogResult = DialogResult.OK;
                this.Close();
            };

            // Rounded corners and border
            this.Paint += (s, e) => {
                using (Pen p = new Pen(UITheme.BorderColor, 2))
                {
                    e.Graphics.DrawRectangle(p, 0, 0, this.Width - 1, this.Height - 1);
                }
            };

            this.Load += (s, e) => UITheme.AnimateControlEntrance(this);
        }
    }
}
