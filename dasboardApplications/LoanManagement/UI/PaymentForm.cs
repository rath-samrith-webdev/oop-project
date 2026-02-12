using System;
using System.Drawing;
using System.Windows.Forms;
using dasboardApplications.Core;
using dasboardApplications.Models;
using dasboardApplications.Services;

namespace dasboardApplications.Features.LoanManagement
{
    public partial class PaymentForm : Form
    {
        private readonly LoanModel _loan;
        private readonly PaymentService _paymentService;

        public PaymentForm(LoanModel loan)
        {
            InitializeComponent();
            _loan = loan;
            _paymentService = dasboardApplications.Core.ServiceContainer.GetService<PaymentService>();
            ApplyTheme();
            LoadLoanDetails();
        }

        private void ApplyTheme()
        {
            this.BackColor = UITheme.ContentBackground;

            if (titleLabel != null) UITheme.StyleLabel(titleLabel, UITheme.LabelLevel.Header);

            // Labels
            var labels = this.Controls.Cast<Control>().Where(c => c is Label).ToList();
            foreach (Label lbl in labels)
            {
                if (lbl != titleLabel) UITheme.StyleLabel(lbl, UITheme.LabelLevel.Body);
            }

            // Inputs
            var inputs = this.Controls.Cast<Control>().Where(c => c is TextBox || c is ComboBox).ToList();
            foreach (var ctrl in inputs)
            {
                if (ctrl is TextBox txt) UITheme.StyleTextBox(txt);
                else
                {
                    ctrl.BackColor = UITheme.SecondaryBackground;
                    ctrl.ForeColor = UITheme.TextPrimary;
                    ctrl.Font = UITheme.BodyFont;
                }
            }

            // Buttons
            UITheme.StyleButton(btnSave, isPrimary: true);
            UITheme.StyleButton(btnCancel, isPrimary: false);
        }

        private void LoadLoanDetails()
        {
            lblLoanInfo.Text = $"Loan ID: {_loan.Id} | Balance: {_loan.OutstandingBalance:C}";
            txtAmount.Text = "0";
            cmbPaymentType.SelectedIndex = 0;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!double.TryParse(txtAmount.Text, out double amount) || amount <= 0)
            {
                MessageBox.Show("Please enter a valid amount.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (amount > _loan.OutstandingBalance)
            {
                MessageBox.Show("Payment amount cannot exceed outstanding balance.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var payment = new Payment
            {
                LoanId = _loan.Id,
                AmountPaid = amount,
                PaymentDate = DateTime.Now,
                PaymentType = cmbPaymentType.SelectedItem.ToString(),
                Status = "Completed",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _paymentService.RecordPayment(payment);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
