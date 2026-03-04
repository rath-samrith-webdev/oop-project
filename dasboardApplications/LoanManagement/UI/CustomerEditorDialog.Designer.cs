namespace dasboardApplications.Features.LoanManagement
{
    partial class CustomerEditorDialog
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.headerLabel = new System.Windows.Forms.Label();
            this.nameLabel = new System.Windows.Forms.Label();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.emailLabel = new System.Windows.Forms.Label();
            this.emailTextBox = new System.Windows.Forms.TextBox();
            this.phoneLabel = new System.Windows.Forms.Label();
            this.phoneTextBox = new System.Windows.Forms.TextBox();
            this.addressLabel = new System.Windows.Forms.Label();
            this.addressTextBox = new System.Windows.Forms.TextBox();
            this.kycLabel = new System.Windows.Forms.Label();
            this.kycTextBox = new System.Windows.Forms.TextBox();
            this.saveButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.mainLayout = new System.Windows.Forms.TableLayoutPanel();
            this.buttonPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.mainLayout.SuspendLayout();
            this.buttonPanel.SuspendLayout();
            this.SuspendLayout();

            // headerLabel
            this.headerLabel.AutoSize = true;
            this.headerLabel.Font = new System.Drawing.Font("Segoe UI Variable Display", 16F, System.Drawing.FontStyle.Bold);
            this.headerLabel.Location = new System.Drawing.Point(20, 20);
            this.headerLabel.Margin = new System.Windows.Forms.Padding(0, 0, 0, 20);
            this.headerLabel.Name = "headerLabel";
            this.headerLabel.Size = new System.Drawing.Size(188, 30);
            this.headerLabel.TabIndex = 0;
            this.headerLabel.Text = "Customer Details";

            // mainLayout
            this.mainLayout.ColumnCount = 1;
            this.mainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainLayout.Controls.Add(this.headerLabel, 0, 0);
            this.mainLayout.Controls.Add(this.nameLabel, 0, 1);
            this.mainLayout.Controls.Add(this.nameTextBox, 0, 2);
            this.mainLayout.Controls.Add(this.emailLabel, 0, 3);
            this.mainLayout.Controls.Add(this.emailTextBox, 0, 4);
            this.mainLayout.Controls.Add(this.phoneLabel, 0, 5);
            this.mainLayout.Controls.Add(this.phoneTextBox, 0, 6);
            this.mainLayout.Controls.Add(this.addressLabel, 0, 7);
            this.mainLayout.Controls.Add(this.addressTextBox, 0, 8);
            this.mainLayout.Controls.Add(this.kycLabel, 0, 9);
            this.mainLayout.Controls.Add(this.kycTextBox, 0, 10);
            this.mainLayout.Controls.Add(this.buttonPanel, 0, 11);
            this.mainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainLayout.Location = new System.Drawing.Point(0, 0);
            this.mainLayout.Name = "mainLayout";
            this.mainLayout.Padding = new System.Windows.Forms.Padding(24);
            this.mainLayout.RowCount = 12;
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.mainLayout.Size = new System.Drawing.Size(450, 650);
            this.mainLayout.TabIndex = 0;

            // nameLabel
            this.nameLabel.AutoSize = true;
            this.nameLabel.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.nameLabel.Text = "Full Name";
            // nameTextBox
            this.nameTextBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.nameTextBox.Margin = new System.Windows.Forms.Padding(0, 0, 0, 16);
            this.nameTextBox.TabIndex = 1;

            // emailLabel
            this.emailLabel.AutoSize = true;
            this.emailLabel.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.emailLabel.Text = "Email";
            // emailTextBox
            this.emailTextBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.emailTextBox.Margin = new System.Windows.Forms.Padding(0, 0, 0, 16);
            this.emailTextBox.TabIndex = 2;

            // phoneLabel
            this.phoneLabel.AutoSize = true;
            this.phoneLabel.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.phoneLabel.Text = "Phone Number";
            // phoneTextBox
            this.phoneTextBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.phoneTextBox.Margin = new System.Windows.Forms.Padding(0, 0, 0, 16);
            this.phoneTextBox.TabIndex = 3;

            // addressLabel
            this.addressLabel.AutoSize = true;
            this.addressLabel.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.addressLabel.Text = "Address";
            // addressTextBox
            this.addressTextBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.addressTextBox.Height = 60;
            this.addressTextBox.Multiline = true;
            this.addressTextBox.Margin = new System.Windows.Forms.Padding(0, 0, 0, 16);
            this.addressTextBox.TabIndex = 4;

            // kycLabel
            this.kycLabel.AutoSize = true;
            this.kycLabel.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.kycLabel.Text = "KYC Documents";
            // kycTextBox
            this.kycTextBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.kycTextBox.Height = 60;
            this.kycTextBox.Multiline = true;
            this.kycTextBox.Margin = new System.Windows.Forms.Padding(0, 0, 0, 24);
            this.kycTextBox.TabIndex = 5;

            // buttonPanel
            this.buttonPanel.Controls.Add(this.saveButton);
            this.buttonPanel.Controls.Add(this.cancelButton);
            this.buttonPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.buttonPanel.Location = new System.Drawing.Point(24, 550);
            this.buttonPanel.Name = "buttonPanel";
            this.buttonPanel.Size = new System.Drawing.Size(402, 50);
            this.buttonPanel.TabIndex = 6;

            // saveButton
            this.saveButton.Size = new System.Drawing.Size(140, 40);
            this.saveButton.Name = "saveButton";
            this.saveButton.TabIndex = 0;
            this.saveButton.Text = "Save";
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);

            // cancelButton
            this.cancelButton.Size = new System.Drawing.Size(100, 40);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);

            // CustomerEditorDialog
            this.AcceptButton = this.saveButton;
            this.CancelButton = this.cancelButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(450, 650);
            this.Controls.Add(this.mainLayout);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CustomerEditorDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.mainLayout.ResumeLayout(false);
            this.mainLayout.PerformLayout();
            this.buttonPanel.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Label headerLabel;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.Label emailLabel;
        private System.Windows.Forms.TextBox emailTextBox;
        private System.Windows.Forms.Label phoneLabel;
        private System.Windows.Forms.TextBox phoneTextBox;
        private System.Windows.Forms.Label addressLabel;
        private System.Windows.Forms.TextBox addressTextBox;
        private System.Windows.Forms.Label kycLabel;
        private System.Windows.Forms.TextBox kycTextBox;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.TableLayoutPanel mainLayout;
        private System.Windows.Forms.FlowLayoutPanel buttonPanel;
    }
}
