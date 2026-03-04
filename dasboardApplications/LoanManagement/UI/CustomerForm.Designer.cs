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
            this.components = new System.ComponentModel.Container();
            this.titleLabel = new System.Windows.Forms.Label();
            this.customerDataGridView = new System.Windows.Forms.DataGridView();
            this.toolbarPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.searchPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.searchLabel = new System.Windows.Forms.Label();
            this.searchTextBox = new System.Windows.Forms.TextBox();
            this.addNewButton = new System.Windows.Forms.Button();
            this.editButton = new System.Windows.Forms.Button();
            this.deleteButton = new System.Windows.Forms.Button();
            this.refreshButton = new System.Windows.Forms.Button();
            this.mainContainer = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.customerDataGridView)).BeginInit();
            this.toolbarPanel.SuspendLayout();
            this.searchPanel.SuspendLayout();
            this.mainContainer.SuspendLayout();
            this.SuspendLayout();

            // titleLabel
            this.titleLabel.AutoSize = true;
            this.titleLabel.Font = new System.Drawing.Font("Segoe UI Variable Display", 20F, System.Drawing.FontStyle.Bold);
            this.titleLabel.Location = new System.Drawing.Point(24, 20);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(320, 36);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "Customer Management";

            // toolbarPanel
            this.toolbarPanel.Controls.Add(this.searchPanel);
            this.toolbarPanel.Controls.Add(this.addNewButton);
            this.toolbarPanel.Controls.Add(this.editButton);
            this.toolbarPanel.Controls.Add(this.deleteButton);
            this.toolbarPanel.Controls.Add(this.refreshButton);
            this.toolbarPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.toolbarPanel.Location = new System.Drawing.Point(0, 70);
            this.toolbarPanel.Name = "toolbarPanel";
            this.toolbarPanel.Padding = new System.Windows.Forms.Padding(20, 10, 20, 10);
            this.toolbarPanel.Size = new System.Drawing.Size(1100, 60);
            this.toolbarPanel.TabIndex = 1;

            // searchPanel
            this.searchPanel.AutoSize = true;
            this.searchPanel.Controls.Add(this.searchLabel);
            this.searchPanel.Controls.Add(this.searchTextBox);
            this.searchPanel.Location = new System.Drawing.Point(23, 13);
            this.searchPanel.Name = "searchPanel";
            this.searchPanel.Size = new System.Drawing.Size(420, 34);
            this.searchPanel.TabIndex = 0;

            // searchLabel
            this.searchLabel.AutoSize = true;
            this.searchLabel.Location = new System.Drawing.Point(3, 8);
            this.searchLabel.Margin = new System.Windows.Forms.Padding(3, 8, 3, 0);
            this.searchLabel.Name = "searchLabel";
            this.searchLabel.Size = new System.Drawing.Size(45, 15);
            this.searchLabel.TabIndex = 0;
            this.searchLabel.Text = "Search:";

            // searchTextBox
            this.searchTextBox.Location = new System.Drawing.Point(54, 3);
            this.searchTextBox.Name = "searchTextBox";
            this.searchTextBox.Size = new System.Drawing.Size(350, 23);
            this.searchTextBox.TabIndex = 1;
            this.searchTextBox.TextChanged += new System.EventHandler(this.searchTextBox_TextChanged);

            // addNewButton
            this.addNewButton.Location = new System.Drawing.Point(449, 13);
            this.addNewButton.Name = "addNewButton";
            this.addNewButton.Size = new System.Drawing.Size(130, 34);
            this.addNewButton.TabIndex = 1;
            this.addNewButton.Text = "+ Add Customer";
            this.addNewButton.Click += new System.EventHandler(this.addNewButton_Click);

            // editButton
            this.editButton.Location = new System.Drawing.Point(585, 13);
            this.editButton.Name = "editButton";
            this.editButton.Size = new System.Drawing.Size(90, 34);
            this.editButton.TabIndex = 2;
            this.editButton.Text = "Edit";
            this.editButton.Click += new System.EventHandler(this.editButton_Click);

            // deleteButton
            this.deleteButton.Location = new System.Drawing.Point(681, 13);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(100, 34);
            this.deleteButton.TabIndex = 3;
            this.deleteButton.Text = "Delete";
            this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);

            // refreshButton
            this.refreshButton.Location = new System.Drawing.Point(787, 13);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(40, 34);
            this.refreshButton.TabIndex = 4;
            this.refreshButton.Text = "🔄";
            this.refreshButton.Click += new System.EventHandler(this.refreshButton_Click);

            // customerDataGridView
            this.customerDataGridView.AllowUserToAddRows = false;
            this.customerDataGridView.AllowUserToDeleteRows = false;
            this.customerDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.customerDataGridView.BackgroundColor = System.Drawing.Color.White;
            this.customerDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.customerDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.customerDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customerDataGridView.Location = new System.Drawing.Point(24, 24);
            this.customerDataGridView.Name = "customerDataGridView";
            this.customerDataGridView.ReadOnly = true;
            this.customerDataGridView.RowHeadersVisible = false;
            this.customerDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.customerDataGridView.Size = new System.Drawing.Size(1052, 532);
            this.customerDataGridView.TabIndex = 0;
            this.customerDataGridView.SelectionChanged += new System.EventHandler(this.customerDataGridView_SelectionChanged);
            this.customerDataGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.customerDataGridView_CellDoubleClick);

            // mainContainer
            this.mainContainer.Controls.Add(this.customerDataGridView);
            this.mainContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainContainer.Location = new System.Drawing.Point(0, 130);
            this.mainContainer.Name = "mainContainer";
            this.mainContainer.Padding = new System.Windows.Forms.Padding(24);
            this.mainContainer.Size = new System.Drawing.Size(1100, 580);
            this.mainContainer.TabIndex = 2;

            // CustomerForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1100, 710);
            this.Controls.Add(this.mainContainer);
            this.Controls.Add(this.toolbarPanel);
            this.Controls.Add(this.titleLabel);
            this.Name = "CustomerForm";
            this.Padding = new System.Windows.Forms.Padding(0, 20, 0, 0);
            ((System.ComponentModel.ISupportInitialize)(this.customerDataGridView)).EndInit();
            this.toolbarPanel.ResumeLayout(false);
            this.toolbarPanel.PerformLayout();
            this.searchPanel.ResumeLayout(false);
            this.searchPanel.PerformLayout();
            this.mainContainer.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.DataGridView customerDataGridView;
        private System.Windows.Forms.FlowLayoutPanel toolbarPanel;
        private System.Windows.Forms.FlowLayoutPanel searchPanel;
        private System.Windows.Forms.Label searchLabel;
        private System.Windows.Forms.TextBox searchTextBox;
        private System.Windows.Forms.Button addNewButton;
        private System.Windows.Forms.Button editButton;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.Button refreshButton;
        private System.Windows.Forms.Panel mainContainer;
    }
}
