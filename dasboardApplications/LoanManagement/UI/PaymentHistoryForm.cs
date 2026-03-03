using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using dasboardApplications.Core;
using dasboardApplications.Interfaces;
using dasboardApplications.Models;
using dasboardApplications.Services;

namespace dasboardApplications.Features.LoanManagement
{
    public partial class PaymentHistoryForm : BaseFeatureForm, IFeature
    {
        public string FeatureName => "Payment History";
        public Form GetForm() => this;

        private readonly CustomerService _customerService;
        private readonly LoanService _loanService;
        private readonly PaymentService _paymentService;
        private readonly LoanCalculatorService _loanCalculatorService;

        private List<Customer> _customers;
        private List<LoanModel> _customerLoans;

        public PaymentHistoryForm()
        {
            InitializeComponent();
            _customerService = dasboardApplications.Core.ServiceContainer.GetService<CustomerService>();
            _loanService = dasboardApplications.Core.ServiceContainer.GetService<LoanService>();
            _paymentService = dasboardApplications.Core.ServiceContainer.GetService<PaymentService>();
            _loanCalculatorService = new LoanCalculatorService();
            ApplyTheme();
            LoadCustomers();
            SetupScheduleGrid();
        }

        private void ApplyTheme()
        {
            this.BackColor = UITheme.PrimaryBackground;
            this.ForeColor = UITheme.TextPrimary;

            tabControlHistory.BackColor = UITheme.PrimaryBackground;
            tabControlHistory.ForeColor = UITheme.TextPrimary;
            tabTransactions.BackColor = UITheme.PrimaryBackground;
            tabSchedule.BackColor = UITheme.PrimaryBackground;
            UITheme.StyleTabControl(tabControlHistory);

            ApplyToAll(this.Controls);
            tabControlHistory.Refresh();

            if (titleLabel != null) UITheme.StyleLabel(titleLabel, UITheme.LabelLevel.Header);
            UITheme.StyleButton(btnLoad, isPrimary: true);
            UITheme.StyleDataGrid(dgvPayments);
            UITheme.StyleDataGrid(dgvSchedule);

            lblInfo.ForeColor = UITheme.TextSecondary;
            lblInfo.Font = UITheme.BodyFont;
        }

        private void SetupScheduleGrid()
        {
            dgvSchedule.CellContentClick += dgvSchedule_CellContentClick;
        }

        private void ApplyToAll(Control.ControlCollection controls)
        {
            foreach (Control ctrl in controls)
            {
                if (ctrl is Label lbl && lbl != titleLabel && lbl != lblInfo)
                    UITheme.StyleLabel(lbl, UITheme.LabelLevel.Body);
                else if (ctrl is ComboBox cmb)
                {
                    cmb.BackColor = UITheme.SecondaryBackground;
                    cmb.ForeColor = UITheme.TextPrimary;
                    cmb.Font = UITheme.BodyFont;
                    // Remove FlatStyle.Flat as it often causes rendering issues in dark mode with WinForms
                }
                else if (ctrl.HasChildren)
                    ApplyToAll(ctrl.Controls);
            }
        }

        private void LoadCustomers()
        {
            _customers = _customerService.GetAllCustomers();
            cmbCustomer.DataSource = null;
            cmbCustomer.DataSource = _customers;
            cmbCustomer.DisplayMember = "FullName";
            cmbCustomer.ValueMember = "Id";

            cmbLoan.DataSource = null;
            cmbLoan.Items.Clear();
            dgvPayments.DataSource = null;
        }

        private void cmbCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCustomer.SelectedValue == null) return;

            int customerId = (int)cmbCustomer.SelectedValue;
            _customerLoans = _loanService.GetLoansByCustomer(customerId);

            cmbLoan.DataSource = null;
            cmbLoan.DataSource = _customerLoans;
            cmbLoan.DisplayMember = "LoanSummary";
            cmbLoan.ValueMember = "Id";

            // Add summary display text
            foreach (var loan in _customerLoans)
            {
                // The list will display via ToString() override or DisplayMember
            }

            dgvPayments.DataSource = null;
            lblInfo.Text = $"Found {_customerLoans.Count} loan(s) for this customer.";
        }

        private void cmbLoan_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Auto-load when loan selection changes
            LoadPayments();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            LoadPayments();
        }

        private void LoadPayments()
        {
            if (cmbLoan.SelectedValue == null || !(cmbLoan.SelectedValue is int loanId)) return;

            var payments = _paymentService.GetPaymentsByLoan(loanId);
            dgvPayments.DataSource = null;
            dgvPayments.DataSource = payments;

            // Hide internal/metadata columns for cleaner view
            string[] columnsToHide = { "LoanId", "Id", "Status", "CreatedAt", "UpdatedAt", "PaymentType" };
            foreach (var colName in columnsToHide)
            {
                if (dgvPayments.Columns.Contains(colName))
                    dgvPayments.Columns[colName].Visible = false;
            }

            // Format remaining columns
            if (dgvPayments.Columns.Contains("AmountPaid"))
            {
                dgvPayments.Columns["AmountPaid"].DefaultCellStyle.Format = "N2";
                dgvPayments.Columns["AmountPaid"].HeaderText = "Amount Paid";
            }

            if (dgvPayments.Columns.Contains("PaymentDate"))
                dgvPayments.Columns["PaymentDate"].HeaderText = "Date";

            LoadInstallmentSchedule(loanId, payments);

            if (payments.Count == 0)
                lblInfo.Text = "No payments found for this loan.";
            else
            {
                double totalPaid = payments.Sum(p => p.AmountPaid);
                lblInfo.Text = $"{payments.Count} payment(s) found. Total Paid: {totalPaid:N2}";
            }
        }

        private void tabControlHistory_DrawItem(object sender, DrawItemEventArgs e)
        {
            UITheme.DrawTab(tabControlHistory, e);
        }

        private void tabControlHistory_Paint(object sender, PaintEventArgs e)
        {
            UITheme.PaintTabControlBackground(tabControlHistory, e);
        }

        private void LoadInstallmentSchedule(int loanId, List<Payment> payments)
        {
            var loan = _customerLoans.FirstOrDefault(l => l.Id == loanId);
            if (loan == null) return;

            var schedule = _loanCalculatorService.GenerateAmortizationSchedule(loan);
            double totalPaid = payments.Sum(p => p.AmountPaid);
            double cumulativeExpected = 0;

            foreach (var entry in schedule)
            {
                cumulativeExpected += entry.EMI;
                if (totalPaid >= cumulativeExpected - 0.01) // Small delta for floating point
                    entry.Status = "Paid";
                else if (totalPaid > (cumulativeExpected - entry.EMI) + 0.01)
                    entry.Status = "Partially Paid";
                else
                    entry.Status = "Unpaid";
            }

            dgvSchedule.DataSource = null;
            dgvSchedule.Columns.Clear();
            dgvSchedule.DataSource = schedule;

            // Add action button column
            DataGridViewButtonColumn payColumn = new DataGridViewButtonColumn
            {
                Name = "PayAction",
                HeaderText = "Action",
                Text = "Mark as Paid",
                UseColumnTextForButtonValue = true,
                FlatStyle = FlatStyle.Flat,
                FillWeight = 60,
                MinimumWidth = 140
            };
            if (dgvSchedule.Columns.Contains("PayAction")) dgvSchedule.Columns.Remove("PayAction");
            dgvSchedule.Columns.Add(payColumn);

            // Column Header Formatting
            if (dgvSchedule.Columns.Contains("PaymentNumber"))
            {
                dgvSchedule.Columns["PaymentNumber"].HeaderText = "No.";
                dgvSchedule.Columns["PaymentNumber"].FillWeight = 20;
            }
            if (dgvSchedule.Columns.Contains("EMI")) dgvSchedule.Columns["EMI"].FillWeight = 35;
            if (dgvSchedule.Columns.Contains("PrincipalComponent")) dgvSchedule.Columns["PrincipalComponent"].FillWeight = 35;
            if (dgvSchedule.Columns.Contains("InterestComponent")) dgvSchedule.Columns["InterestComponent"].FillWeight = 35;
            if (dgvSchedule.Columns.Contains("OpeningBalance")) dgvSchedule.Columns["OpeningBalance"].Visible = false;
            if (dgvSchedule.Columns.Contains("ClosingBalance")) dgvSchedule.Columns["ClosingBalance"].Visible = false;

            if (dgvSchedule.Columns.Contains("Status"))
            {
                dgvSchedule.Columns["Status"].FillWeight = 40;
            }

            foreach (DataGridViewColumn col in dgvSchedule.Columns)
            {
                if (col.ValueType == typeof(double))
                    col.DefaultCellStyle.Format = "N2";

                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            }

            StyleScheduleRows();
        }

        private void StyleScheduleRows()
        {
            foreach (DataGridViewRow row in dgvSchedule.Rows)
            {
                var entry = row.DataBoundItem as AmortizationEntry;
                if (entry == null) continue;

                if (entry.Status == "Paid")
                {
                    row.DefaultCellStyle.ForeColor = UITheme.SuccessColor;
                }
                else if (entry.Status == "Partially Paid")
                {
                    row.DefaultCellStyle.ForeColor = UITheme.WarningColor;
                }
            }
        }

        private void dgvSchedule_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            // Only care about the button column
            if (dgvSchedule.Columns[e.ColumnIndex]?.Name == "PayAction")
            {
                var entry = dgvSchedule.Rows[e.RowIndex].DataBoundItem as AmortizationEntry;
                if (entry != null && entry.Status == "Paid")
                {
                    // Paint background only to hide the button
                    e.PaintBackground(e.CellBounds, true);
                    e.Handled = true;
                }
            }
        }

        private void dgvSchedule_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != dgvSchedule.Columns["PayAction"]?.Index) return;

            var entry = dgvSchedule.Rows[e.RowIndex].DataBoundItem as AmortizationEntry;
            if (entry == null || entry.Status == "Paid") return;

            if (cmbLoan.SelectedValue == null || !(cmbLoan.SelectedValue is int loanId)) return;

            // Record payment for this specific EMI amount
            var payment = new Payment
            {
                LoanId = loanId,
                PaymentDate = DateTime.Now,
                AmountPaid = entry.EMI,
                PaymentType = "Manual",
                Status = "Completed"
            };

            _paymentService.RecordPayment(payment);
            MessageBox.Show($"Installment #{entry.PaymentNumber} marked as paid successfully!", "Payment Recorded", MessageBoxButtons.OK, MessageBoxIcon.Information);

            LoadPayments(); // Refresh everything
        }
    }
}
