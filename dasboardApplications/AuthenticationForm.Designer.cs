namespace dasboardApplications
{
    partial class AuthenticationForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            titleLabel = new Label();
            userNameLabel = new Label();
            usernameTextBox = new TextBox();
            passwordTextBox = new TextBox();
            passwordLabel = new Label();
            signInButton = new Button();
            SuspendLayout();
            //
            // titleLabel
            //
            titleLabel.AutoSize = true;
            titleLabel.Font = new Font("Agency FB", 36F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            titleLabel.Location = new Point(220, 51);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(131, 59);
            titleLabel.TabIndex = 0;
            titleLabel.Text = "Sign in";
            //
            // userNameLabel
            //
            userNameLabel.AutoSize = true;
            userNameLabel.Location = new Point(90, 125);
            userNameLabel.Name = "userNameLabel";
            userNameLabel.Size = new Size(63, 15);
            userNameLabel.TabIndex = 1;
            userNameLabel.Text = "User name";
            //
            // usernameTextBox
            //
            usernameTextBox.Location = new Point(90, 143);
            usernameTextBox.Name = "usernameTextBox";
            usernameTextBox.Size = new Size(430, 23);
            usernameTextBox.TabIndex = 2;
            //
            // passwordTextBox
            //
            passwordTextBox.Location = new Point(90, 197);
            passwordTextBox.Name = "passwordTextBox";
            passwordTextBox.Size = new Size(430, 23);
            passwordTextBox.TabIndex = 3;
            passwordTextBox.UseSystemPasswordChar = true;
            //
            // passwordLabel
            //
            passwordLabel.AutoSize = true;
            passwordLabel.Location = new Point(88, 178);
            passwordLabel.Name = "passwordLabel";
            passwordLabel.Size = new Size(88, 15);
            passwordLabel.TabIndex = 4;
            passwordLabel.Text = "Password Label";
            //
            // signInButton
            //
            signInButton.Location = new Point(380, 253);
            signInButton.Name = "signInButton";
            signInButton.Size = new Size(140, 27);
            signInButton.TabIndex = 5;
            signInButton.Text = "Sign in";
            signInButton.UseVisualStyleBackColor = true;
            signInButton.Click += signInButton_Click;
            //
            // AuthenticationForm
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(583, 376);
            Controls.Add(signInButton);
            Controls.Add(passwordLabel);
            Controls.Add(passwordTextBox);
            Controls.Add(usernameTextBox);
            Controls.Add(userNameLabel);
            Controls.Add(titleLabel);
            Name = "AuthenticationForm";
            Text = "Authentication";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label titleLabel;
        private Label userNameLabel;
        private TextBox usernameTextBox;
        private TextBox passwordTextBox;
        private Label passwordLabel;
        private Button signInButton;
    }
}
