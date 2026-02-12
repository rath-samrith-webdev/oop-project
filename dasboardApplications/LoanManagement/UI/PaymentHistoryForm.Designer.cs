namespace dasboardApplications.Features.LoanManagement
{
    partial class PaymentHistoryForm
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
            lblCustomer = new Label();
            cmbCustomer = new ComboBox();
            lblLoan = new Label();
            cmbLoan = new ComboBox();
            btnLoad = new Button();
            lblInfo = new Label();
            dgvPayments = new DataGridView();
            dgvSchedule = new DataGridView();
            tabControlHistory = new TabControl();
            tabTransactions = new TabPage();
            tabSchedule = new TabPage();

            ((System.ComponentModel.ISupportInitialize)dgvPayments).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvSchedule).BeginInit();
            tabControlHistory.SuspendLayout();
            tabTransactions.SuspendLayout();
            tabSchedule.SuspendLayout();
            SuspendLayout();

            // Layout Panels
            FlowLayoutPanel mainLayout = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Padding = new Padding(32)
            };

            FlowLayoutPanel filterPanel = new FlowLayoutPanel
            {
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                Margin = new Padding(0, 0, 0, 24)
            };

            // titleLabel
            titleLabel.AutoSize = true;
            titleLabel.Font = new Font("Segoe UI Variable Display", 28F, FontStyle.Bold);
            titleLabel.Name = "titleLabel";
            titleLabel.Text = "Payment History";
            titleLabel.Margin = new Padding(0, 0, 0, 32);

            // lblCustomer
            lblCustomer.AutoSize = true;
            lblCustomer.Text = "Select Customer";
            lblCustomer.Margin = new Padding(0, 0, 0, 8);

            // cmbCustomer
            cmbCustomer.Name = "cmbCustomer";
            cmbCustomer.Size = new Size(280, 40);
            cmbCustomer.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCustomer.SelectedIndexChanged += cmbCustomer_SelectedIndexChanged;
            cmbCustomer.Margin = new Padding(0, 0, 24, 0);

            // lblLoan
            lblLoan.AutoSize = true;
            lblLoan.Text = "Select Loan";
            lblLoan.Margin = new Padding(0, 0, 0, 8);

            // cmbLoan
            cmbLoan.Name = "cmbLoan";
            cmbLoan.Size = new Size(350, 40);
            cmbLoan.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbLoan.SelectedIndexChanged += cmbLoan_SelectedIndexChanged;
            cmbLoan.Margin = new Padding(0, 0, 24, 0);

            btnLoad.Size = new Size(160, 42);
            btnLoad.Margin = new Padding(0, 28, 0, 0); // Fine-tuned alignment
            btnLoad.Text = "Load History";
            btnLoad.Click += btnLoad_Click;

            // lblInfo
            lblInfo.AutoSize = true;
            lblInfo.Location = new Point(0, 136);
            lblInfo.Name = "lblInfo";
            lblInfo.Text = "Select a customer and loan to view payment history.";
            lblInfo.Margin = new Padding(0, 0, 0, 16);

            // tabControlHistory
            tabControlHistory.Size = new Size(1036, 610);
            tabControlHistory.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControlHistory.ItemSize = new Size(240, 50);
            tabControlHistory.SizeMode = TabSizeMode.Fixed;
            tabControlHistory.Controls.Add(tabTransactions);
            tabControlHistory.Controls.Add(tabSchedule);
            tabControlHistory.Margin = new Padding(0);
            tabControlHistory.DrawItem += tabControlHistory_DrawItem;
            tabControlHistory.Paint += tabControlHistory_Paint;

            // tabTransactions
            tabTransactions.Text = "Transaction History";
            tabTransactions.Padding = new Padding(10);
            tabTransactions.BackColor = Color.FromArgb(15, 23, 42);

            // tabSchedule
            tabSchedule.Text = "Installment Schedule";
            tabSchedule.Padding = new Padding(10);
            tabSchedule.BackColor = Color.FromArgb(15, 23, 42);

            // dgvPayments
            dgvPayments.Dock = DockStyle.Fill;
            dgvPayments.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPayments.AllowUserToAddRows = false;
            dgvPayments.AllowUserToDeleteRows = false;
            dgvPayments.ReadOnly = true;
            dgvPayments.RowHeadersVisible = false;
            dgvPayments.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            tabTransactions.Controls.Add(dgvPayments);

            // dgvSchedule
            dgvSchedule.Dock = DockStyle.Fill;
            dgvSchedule.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvSchedule.Name = "dgvSchedule";
            dgvSchedule.RowHeadersVisible = false;
            dgvSchedule.Size = new Size(1008, 500);
            dgvSchedule.TabIndex = 0;
            dgvSchedule.CellPainting += dgvSchedule_CellPainting;
            dgvSchedule.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            tabSchedule.Controls.Add(dgvSchedule);

            // Assembly via Panels
            FlowLayoutPanel customerGrp = new FlowLayoutPanel { AutoSize = true, FlowDirection = FlowDirection.TopDown };
            customerGrp.Controls.Add(lblCustomer);
            customerGrp.Controls.Add(cmbCustomer);

            FlowLayoutPanel loanGrp = new FlowLayoutPanel { AutoSize = true, FlowDirection = FlowDirection.TopDown };
            loanGrp.Controls.Add(lblLoan);
            loanGrp.Controls.Add(cmbLoan);

            filterPanel.Controls.Add(customerGrp);
            filterPanel.Controls.Add(loanGrp);
            filterPanel.Controls.Add(btnLoad);

            mainLayout.Controls.Add(titleLabel);
            mainLayout.Controls.Add(filterPanel);
            mainLayout.Controls.Add(lblInfo);
            mainLayout.Controls.Add(tabControlHistory);

            Controls.Add(mainLayout);

            // PaymentHistoryForm
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1100, 780);
            Name = "PaymentHistoryForm";
            Text = "Payment History";

            ((System.ComponentModel.ISupportInitialize)dgvPayments).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvSchedule).EndInit();
            tabTransactions.ResumeLayout(false);
            tabSchedule.ResumeLayout(false);
            tabControlHistory.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        private Label titleLabel;
        private Label lblCustomer;
        private ComboBox cmbCustomer;
        private Label lblLoan;
        private ComboBox cmbLoan;
        private Button btnLoad;
        private Label lblInfo;
        private DataGridView dgvPayments;
        private DataGridView dgvSchedule;
        private TabControl tabControlHistory;
        private TabPage tabTransactions;
        private TabPage tabSchedule;
    }
}
