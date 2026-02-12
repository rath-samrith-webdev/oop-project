namespace dasboardApplications.Features.LoanManagement
{
    partial class CustomerForm
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
            titleLabel = new Label();
            nameLabel = new Label();
            nameTextBox = new TextBox();
            emailLabel = new Label();
            emailTextBox = new TextBox();
            phoneLabel = new Label();
            phoneTextBox = new TextBox();
            addressLabel = new Label();
            addressTextBox = new TextBox();
            kycLabel = new Label();
            kycTextBox = new TextBox();
            saveButton = new Button();
            addNewButton = new Button();
            deleteButton = new Button();
            refreshButton = new Button();
            searchLabel = new Label();
            searchTextBox = new TextBox();
            customerDataGridView = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)customerDataGridView).BeginInit();
            SuspendLayout();
            //
            // titleLabel
            //
            this.titleLabel.AutoSize = true;
            this.titleLabel.MaximumSize = new System.Drawing.Size(350, 0);
            this.titleLabel.Font = new System.Drawing.Font("Segoe UI Variable Display", 18F, System.Drawing.FontStyle.Bold);
            this.titleLabel.Location = new System.Drawing.Point(0, 0);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(365, 45);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "Customer Management";
            //
            // nameLabel
            //
            this.nameLabel.AutoSize = true;
            this.nameLabel.Location = new System.Drawing.Point(0, 64);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(61, 15);
            this.nameLabel.TabIndex = 1;
            this.nameLabel.Text = "Full Name";
            //
            // nameTextBox
            //
            this.nameTextBox.Location = new System.Drawing.Point(0, 88);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(350, 23);
            this.nameTextBox.TabIndex = 2;
            //
            // emailLabel
            //
            this.emailLabel.AutoSize = true;
            this.emailLabel.Location = new System.Drawing.Point(0, 136);
            this.emailLabel.Name = "emailLabel";
            this.emailLabel.Size = new Size(36, 15);
            this.emailLabel.TabIndex = 3;
            this.emailLabel.Text = "Email";
            //
            // emailTextBox
            //
            this.emailTextBox.Location = new System.Drawing.Point(0, 160);
            this.emailTextBox.Name = "emailTextBox";
            this.emailTextBox.Size = new System.Drawing.Size(350, 23);
            this.emailTextBox.TabIndex = 4;
            //
            // phoneLabel
            //
            this.phoneLabel.AutoSize = true;
            this.phoneLabel.Location = new System.Drawing.Point(0, 208);
            this.phoneLabel.Name = "phoneLabel";
            this.phoneLabel.Size = new Size(88, 15);
            this.phoneLabel.TabIndex = 5;
            this.phoneLabel.Text = "Phone Number";
            //
            // phoneTextBox
            //
            this.phoneTextBox.Location = new System.Drawing.Point(0, 232);
            this.phoneTextBox.Name = "phoneTextBox";
            this.phoneTextBox.Size = new System.Drawing.Size(350, 23);
            this.phoneTextBox.TabIndex = 6;
            //
            // addressLabel
            //
            this.addressLabel.AutoSize = true;
            this.addressLabel.Location = new System.Drawing.Point(0, 280);
            this.addressLabel.Name = "addressLabel";
            this.addressLabel.Size = new Size(49, 15);
            this.addressLabel.TabIndex = 7;
            this.addressLabel.Text = "Address";
            //
            // addressTextBox
            //
            this.addressTextBox.Location = new System.Drawing.Point(0, 304);
            this.addressTextBox.Multiline = true;
            this.addressTextBox.Name = "addressTextBox";
            this.addressTextBox.Size = new System.Drawing.Size(350, 64);
            this.addressTextBox.TabIndex = 8;
            //
            // kycLabel
            //
            this.kycLabel.AutoSize = true;
            this.kycLabel.Location = new System.Drawing.Point(0, 392);
            this.kycLabel.Name = "kycLabel";
            this.kycLabel.Size = new Size(95, 15);
            this.kycLabel.TabIndex = 9;
            this.kycLabel.Text = "KYC Documents";
            //
            // kycTextBox
            //
            this.kycTextBox.Location = new System.Drawing.Point(0, 416);
            this.kycTextBox.Multiline = true;
            this.kycTextBox.Name = "kycTextBox";
            this.kycTextBox.Size = new System.Drawing.Size(350, 64);
            this.kycTextBox.TabIndex = 10;
            //
            // saveButton
            //
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.saveButton.Location = new System.Drawing.Point(0, 504);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(130, 36);
            this.saveButton.TabIndex = 11;
            this.saveButton.Text = "Save Customer";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            //
            // addNewButton
            //
            this.addNewButton.Name = "addNewButton";
            this.addNewButton.Size = new Size(350, 40);
            this.addNewButton.TabIndex = 12;
            this.addNewButton.Text = "Add New Customer";
            this.addNewButton.UseVisualStyleBackColor = true;
            this.addNewButton.Margin = new Padding(0, 5, 0, 5);
            this.addNewButton.Click += new System.EventHandler(this.addNewButton_Click);
            //
            // deleteButton
            //
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new Size(350, 40);
            this.deleteButton.TabIndex = 14;
            this.deleteButton.Text = "Delete Selected";
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Margin = new Padding(0, 5, 0, 5);
            this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
            //
            // refreshButton
            //
            this.refreshButton.Size = new Size(350, 40);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Text = "Refresh Grid";
            this.refreshButton.TabIndex = 15;
            this.refreshButton.Margin = new Padding(0, 5, 0, 0);
            this.refreshButton.Click += new System.EventHandler(this.refreshButton_Click);
            //
            // searchLabel
            //
            this.searchLabel.Text = "Search Customers:";
            this.searchLabel.AutoSize = true;
            this.searchLabel.Margin = new Padding(0, 8, 10, 0);
            //
            // searchTextBox
            //
            this.searchTextBox.Size = new Size(350, 30);
            this.searchTextBox.Name = "searchTextBox";
            this.searchTextBox.TextChanged += new System.EventHandler(this.searchTextBox_TextChanged);
            //
            // customerDataGridView
            //
            this.customerDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customerDataGridView.Location = new System.Drawing.Point(0, 0);
            this.customerDataGridView.Name = "customerDataGridView";
            this.customerDataGridView.ReadOnly = true;
            this.customerDataGridView.RowHeadersVisible = false;
            this.customerDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.customerDataGridView.Size = new System.Drawing.Size(600, 476);
            this.customerDataGridView.TabIndex = 13;
            this.customerDataGridView.SelectionChanged += new System.EventHandler(this.customerDataGridView_SelectionChanged);
            //
            // CustomerForm
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            //
            // inputPanel
            //
            FlowLayoutPanel inputPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Left,
                Width = 420,
                Padding = new Padding(24),
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true
            };

            titleLabel.Margin = new Padding(0, 0, 0, 20);
            nameLabel.Margin = new Padding(0, 0, 0, 5);
            nameTextBox.Margin = new Padding(0, 0, 0, 15);
            emailLabel.Margin = new Padding(0, 0, 0, 5);
            emailTextBox.Margin = new Padding(0, 0, 0, 15);
            phoneLabel.Margin = new Padding(0, 0, 0, 5);
            phoneTextBox.Margin = new Padding(0, 0, 0, 15);
            addressLabel.Margin = new Padding(0, 0, 0, 5);
            addressTextBox.Margin = new Padding(0, 0, 0, 15);
            kycLabel.Margin = new Padding(0, 0, 0, 5);
            kycTextBox.Margin = new Padding(0, 0, 0, 30);
            saveButton.Size = new Size(350, 45);
            saveButton.Margin = new Padding(0, 0, 0, 10);

            inputPanel.Controls.Add(titleLabel);
            inputPanel.Controls.Add(nameLabel);
            inputPanel.Controls.Add(nameTextBox);
            inputPanel.Controls.Add(emailLabel);
            inputPanel.Controls.Add(emailTextBox);
            inputPanel.Controls.Add(phoneLabel);
            inputPanel.Controls.Add(phoneTextBox);
            inputPanel.Controls.Add(addressLabel);
            inputPanel.Controls.Add(addressTextBox);
            inputPanel.Controls.Add(kycLabel);
            inputPanel.Controls.Add(kycTextBox);
            inputPanel.Controls.Add(saveButton);
            inputPanel.Controls.Add(addNewButton);
            inputPanel.Controls.Add(deleteButton);
            inputPanel.Controls.Add(refreshButton);

            //
            // gridPanel
            //
            Panel gridPanel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(24) };

            FlowLayoutPanel searchPanel = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 60, FlowDirection = FlowDirection.LeftToRight, Padding = new Padding(0, 10, 0, 10) };
            searchPanel.Controls.Add(searchLabel);
            searchPanel.Controls.Add(searchTextBox);

            gridPanel.Controls.Add(customerDataGridView);
            gridPanel.Controls.Add(searchPanel);

            this.ClientSize = new System.Drawing.Size(1200, 750);
            this.Controls.Add(gridPanel);
            this.Controls.Add(inputPanel);
            Name = "CustomerForm";
            Text = "Customer Management";
            ((System.ComponentModel.ISupportInitialize)customerDataGridView).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private Label titleLabel;
        private Label nameLabel;
        private TextBox nameTextBox;
        private Label emailLabel;
        private TextBox emailTextBox;
        private Label phoneLabel;
        private TextBox phoneTextBox;
        private Label addressLabel;
        private TextBox addressTextBox;
        private Label kycLabel;
        private TextBox kycTextBox;
        private Button saveButton;
        private Button addNewButton;
        private Button deleteButton;
        private Button refreshButton;
        private Label searchLabel;
        private TextBox searchTextBox;
        private DataGridView customerDataGridView;
    }
}
