using dasboardApplications.Interfaces;
using dasboardApplications.Services;
using dasboardApplications.Core;
using dasboardApplications.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace dasboardApplications.Features.LoanManagement
{
    public partial class LoanForm : BaseFeatureForm, IFeature
    {
        public string FeatureName => "Loan Management";
        public Form GetForm() => this;

        private readonly LoanCalculatorService _calculatorService;
        private readonly ValidationService _validationService;
        private readonly CustomerService _customerService;
        private readonly LoanService _loanService;

        public LoanForm()
        {
            InitializeComponent();
            _calculatorService = dasboardApplications.Core.ServiceContainer.GetService<LoanCalculatorService>();
            _validationService = dasboardApplications.Core.ServiceContainer.GetService<ValidationService>();
            _customerService = dasboardApplications.Core.ServiceContainer.GetService<CustomerService>();
            _loanService = dasboardApplications.Core.ServiceContainer.GetService<LoanService>();

            ApplyModernTheme();
            LoadDefaults();
            LoadCustomers();
        }

        private void LoadCustomers()
        {
            var customers = _customerService.GetAllCustomers();
            cmbCustomer.DataSource = customers;
            cmbCustomer.DisplayMember = "FullName";
            cmbCustomer.ValueMember = "Id";
        }

        private void ApplyModernTheme()
        {
            this.BackColor = UITheme.PrimaryBackground;
            this.ForeColor = UITheme.TextPrimary;

            ApplyToAll(this.Controls);

            UITheme.StyleButton(btnCalculate, isPrimary: true);
            UITheme.StyleButton(btnSaveLoan, isPrimary: true);
            UITheme.StyleButton(btnClear, isPrimary: false);
            UITheme.StyleButton(btnRecordPayment, isPrimary: false);

            UITheme.StyleDataGrid(dgvSchedule);
        }

        private void ApplyToAll(Control.ControlCollection controls)
        {
            foreach (Control ctrl in controls)
            {
                if (ctrl is Label lbl) UITheme.StyleLabel(lbl, UITheme.LabelLevel.Body);
                else if (ctrl is TextBox txt) UITheme.StyleTextBox(txt);
                else if (ctrl is ComboBox cmb)
                {
                    cmb.BackColor = System.Drawing.Color.White;
                    cmb.ForeColor = UITheme.TextPrimary;
                    cmb.Font = UITheme.BodyFont;
                }
                else if (ctrl.HasChildren) ApplyToAll(ctrl.Controls);
            }
        }

        private void LoadDefaults()
        {
            cmbLoanType.DataSource = Enum.GetValues(typeof(LoanType));
            cmbFrequency.DataSource = Enum.GetValues(typeof(PaymentFrequency));

            txtLoanAmount.Text = "10000";
            txtInterestRate.Text = "10";
            txtTenure.Text = "12";
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateInputs()) return;

                var loan = new LoanModel
                {
                    LoanAmount = double.Parse(txtLoanAmount.Text),
                    AnnualInterestRate = double.Parse(txtInterestRate.Text),
                    TenureInMonths = int.Parse(txtTenure.Text),
                    Type = (LoanType)cmbLoanType.SelectedItem,
                    Frequency = (PaymentFrequency)cmbFrequency.SelectedItem
                };

                double emi = _calculatorService.CalculateEMI(loan);
                var schedule = _calculatorService.GenerateAmortizationSchedule(loan);
                double totalInterest = _calculatorService.CalculateTotalInterest(schedule);
                double totalPayment = _calculatorService.CalculateTotalPayment(schedule);

                DisplayResults(emi, totalInterest, totalPayment, schedule);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during calculation: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSaveLoan_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateInputs()) return;
                if (cmbCustomer.SelectedValue == null)
                {
                    MessageBox.Show("Please select a customer.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var loan = new LoanModel
                {
                    CustomerId = (int)cmbCustomer.SelectedValue,
                    LoanAmount = double.Parse(txtLoanAmount.Text),
                    AnnualInterestRate = double.Parse(txtInterestRate.Text),
                    TenureInMonths = int.Parse(txtTenure.Text),
                    Type = (LoanType)cmbLoanType.SelectedItem,
                    Frequency = (PaymentFrequency)cmbFrequency.SelectedItem,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddMonths(int.Parse(txtTenure.Text)),
                    Status = "Active",
                    OutstandingBalance = double.Parse(txtLoanAmount.Text),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                _loanService.CreateLoan(loan);
                MessageBox.Show("Loan saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Trigger calculation to show schedule
                btnCalculate_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while saving the loan: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRecordPayment_Click(object sender, EventArgs e)
        {
            if (cmbCustomer.SelectedValue == null)
            {
                MessageBox.Show("Please select a customer first.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var loans = _loanService.GetLoansByCustomer((int)cmbCustomer.SelectedValue);
            if (loans.Count == 0)
            {
                MessageBox.Show("No active loans found for this customer.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // For simplicity, we'll pick the first active loan or show a selection if multiple.
            // Here we'll just open the PaymentForm with the first loan.
            var loan = loans.FirstOrDefault(l => l.Status == "Active");
            if (loan == null)
            {
                MessageBox.Show("No active loans found for this customer.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var paymentForm = new PaymentForm(loan))
            {
                if (paymentForm.ShowDialog() == DialogResult.OK)
                {
                    // Update UI if needed
                    MessageBox.Show("Payment recorded.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private bool ValidateInputs()
        {
            var amountVal = _validationService.ValidateLoanAmount(txtLoanAmount.Text);
            if (!amountVal.isValid)
            {
                ShowValidationError(amountVal.message, txtLoanAmount);
                return false;
            }

            var rateVal = _validationService.ValidateInterestRate(txtInterestRate.Text);
            if (!rateVal.isValid)
            {
                ShowValidationError(rateVal.message, txtInterestRate);
                return false;
            }

            int frequency = (int)(PaymentFrequency)cmbFrequency.SelectedItem;
            var tenureVal = _validationService.ValidateTenure(txtTenure.Text, frequency);
            if (!tenureVal.isValid)
            {
                ShowValidationError(tenureVal.message, txtTenure);
                return false;
            }

            return true;
        }

        private void ShowValidationError(string message, Control control)
        {
            MessageBox.Show(message, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            control.Focus();
        }

        private void DisplayResults(double emi, double totalInterest, double totalPayment, List<AmortizationEntry> schedule)
        {
            lblEMI.Text = emi.ToString("C");
            lblTotalInterest.Text = totalInterest.ToString("C");
            lblTotalPayment.Text = totalPayment.ToString("C");

            dgvSchedule.DataSource = null;
            dgvSchedule.DataSource = schedule;

            // Format columns
            if (dgvSchedule.Columns.Count > 0)
            {
                dgvSchedule.Columns["OpeningBalance"].DefaultCellStyle.Format = "N2";
                dgvSchedule.Columns["EMI"].DefaultCellStyle.Format = "N2";
                dgvSchedule.Columns["PrincipalComponent"].DefaultCellStyle.Format = "N2";
                dgvSchedule.Columns["InterestComponent"].DefaultCellStyle.Format = "N2";
                dgvSchedule.Columns["ClosingBalance"].DefaultCellStyle.Format = "N2";
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtLoanAmount.Clear();
            txtInterestRate.Clear();
            txtTenure.Clear();
            cmbLoanType.SelectedIndex = 0;
            cmbFrequency.SelectedIndex = 0;

            lblEMI.Text = "$ 0.00";
            lblTotalInterest.Text = "$ 0.00";
            lblTotalPayment.Text = "$ 0.00";
            dgvSchedule.DataSource = null;

            txtLoanAmount.Focus();
        }
    }
}
