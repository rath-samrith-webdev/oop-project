namespace dasboardApplications.Features.LoanManagement
{
    partial class LoanListForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            titleLabel = new Label();
            dgvLoans = new DataGridView();
            btnEdit = new Button();
            btnDelete = new Button();
            btnRefresh = new Button();
            pnlEdit = new Panel();
            lblEditTitle = new Label();
            lblAmount = new Label();
            txtAmount = new TextBox();
            lblRate = new Label();
            txtRate = new TextBox();
            lblTenure = new Label();
            txtTenure = new TextBox();
            lblStatus = new Label();
            cmbStatus = new ComboBox();
            btnSaveEdit = new Button();
            btnCancelEdit = new Button();

            ((System.ComponentModel.ISupportInitialize)dgvLoans).BeginInit();
            pnlEdit.SuspendLayout();
            SuspendLayout();

            // titleLabel
            titleLabel.AutoSize = true;
            titleLabel.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            titleLabel.Location = new Point(0, 0);
            titleLabel.Name = "titleLabel";
            titleLabel.Text = "Loan List";

            // dgvLoans
            dgvLoans.AllowUserToAddRows = false;
            dgvLoans.AllowUserToDeleteRows = false;
            dgvLoans.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvLoans.Location = new Point(0, 64);
            dgvLoans.Name = "dgvLoans";
            dgvLoans.ReadOnly = true;
            dgvLoans.RowHeadersVisible = false;
            dgvLoans.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLoans.Size = new Size(860, 400);
            dgvLoans.TabIndex = 1;
            dgvLoans.SelectionChanged += dgvLoans_SelectionChanged;

            // btnEdit
            btnEdit.Location = new Point(0, 488);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(120, 36);
            btnEdit.Text = "Edit Loan";
            btnEdit.TabIndex = 2;
            btnEdit.Enabled = false;
            btnEdit.Click += btnEdit_Click;

            // btnDelete
            btnDelete.Location = new Point(132, 488);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(120, 36);
            btnDelete.Text = "Delete Loan";
            btnDelete.TabIndex = 3;
            btnDelete.Enabled = false;
            btnDelete.Click += btnDelete_Click;

            // btnRefresh
            btnRefresh.Location = new Point(264, 488);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(100, 36);
            btnRefresh.Text = "Refresh";
            btnRefresh.TabIndex = 4;
            btnRefresh.Click += btnRefresh_Click;

            // pnlEdit - inline edit panel (hidden by default)
            pnlEdit.Location = new Point(0, 64);
            pnlEdit.Name = "pnlEdit";
            pnlEdit.Size = new Size(860, 400);
            pnlEdit.Visible = false;
            pnlEdit.TabIndex = 5;

            // lblEditTitle
            lblEditTitle.AutoSize = true;
            lblEditTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblEditTitle.Location = new Point(0, 0);
            lblEditTitle.Name = "lblEditTitle";
            lblEditTitle.Text = "Edit Loan";
            pnlEdit.Controls.Add(lblEditTitle);

            // lblAmount
            lblAmount.AutoSize = true;
            lblAmount.Location = new Point(0, 48);
            lblAmount.Text = "Loan Amount";
            pnlEdit.Controls.Add(lblAmount);

            // txtAmount
            txtAmount.Location = new Point(0, 72);
            txtAmount.Name = "txtAmount";
            txtAmount.Size = new Size(200, 23);
            pnlEdit.Controls.Add(txtAmount);

            // lblRate
            lblRate.AutoSize = true;
            lblRate.Location = new Point(224, 48);
            lblRate.Text = "Interest Rate (%)";
            pnlEdit.Controls.Add(lblRate);

            // txtRate
            txtRate.Location = new Point(224, 72);
            txtRate.Name = "txtRate";
            txtRate.Size = new Size(150, 23);
            pnlEdit.Controls.Add(txtRate);

            // lblTenure
            lblTenure.AutoSize = true;
            lblTenure.Location = new Point(0, 112);
            lblTenure.Text = "Tenure (Months)";
            pnlEdit.Controls.Add(lblTenure);

            // txtTenure
            txtTenure.Location = new Point(0, 136);
            txtTenure.Name = "txtTenure";
            txtTenure.Size = new Size(150, 23);
            pnlEdit.Controls.Add(txtTenure);

            // lblStatus
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(174, 112);
            lblStatus.Text = "Status";
            pnlEdit.Controls.Add(lblStatus);

            // cmbStatus
            cmbStatus.Location = new Point(174, 136);
            cmbStatus.Name = "cmbStatus";
            cmbStatus.Size = new Size(150, 23);
            cmbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbStatus.Items.AddRange(new object[] { "Active", "Paid", "Defaulted" });
            pnlEdit.Controls.Add(cmbStatus);

            // btnSaveEdit
            btnSaveEdit.Location = new Point(0, 192);
            btnSaveEdit.Name = "btnSaveEdit";
            btnSaveEdit.Size = new Size(140, 36);
            btnSaveEdit.Text = "Save Changes";
            btnSaveEdit.Click += btnSaveEdit_Click;
            pnlEdit.Controls.Add(btnSaveEdit);

            // btnCancelEdit
            btnCancelEdit.Location = new Point(152, 192);
            btnCancelEdit.Name = "btnCancelEdit";
            btnCancelEdit.Size = new Size(100, 36);
            btnCancelEdit.Text = "Cancel";
            btnCancelEdit.Click += btnCancelEdit_Click;
            pnlEdit.Controls.Add(btnCancelEdit);

            // LoanListForm
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(910, 530);
            Controls.Add(pnlEdit);
            Controls.Add(btnRefresh);
            Controls.Add(btnDelete);
            Controls.Add(btnEdit);
            Controls.Add(dgvLoans);
            Controls.Add(titleLabel);
            Name = "LoanListForm";
            Text = "Loan List";

            ((System.ComponentModel.ISupportInitialize)dgvLoans).EndInit();
            pnlEdit.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        private Label titleLabel;
        private DataGridView dgvLoans;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnRefresh;
        private Panel pnlEdit;
        private Label lblEditTitle;
        private Label lblAmount;
        private TextBox txtAmount;
        private Label lblRate;
        private TextBox txtRate;
        private Label lblTenure;
        private TextBox txtTenure;
        private Label lblStatus;
        private ComboBox cmbStatus;
        private Button btnSaveEdit;
        private Button btnCancelEdit;
    }
}
