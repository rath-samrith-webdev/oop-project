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

        public LoanForm()
        {
            InitializeComponent();
            _calculatorService = new LoanCalculatorService();
            _validationService = new ValidationService();

            ApplyModernTheme();
            LoadDefaults();
        }

        private void ApplyModernTheme()
        {
            this.BackColor = UITheme.SecondaryBackground;

            // Labels and Title
            var labels = this.Controls.Cast<Control>().Where(c => c is Label).ToList();
            foreach (Label lbl in labels)
            {
                lbl.ForeColor = UITheme.TextPrimary;
                lbl.Font = UITheme.BodyFont;
            }

            // Inputs
            var inputs = this.Controls.Cast<Control>().Where(c => c is TextBox || c is ComboBox).ToList();
            foreach (var ctrl in inputs)
            {
                ctrl.BackColor = Color.FromArgb(35, 35, 40);
                ctrl.ForeColor = UITheme.TextPrimary;
                ctrl.Font = UITheme.BodyFont;
                if (ctrl is TextBox txt) txt.BorderStyle = BorderStyle.FixedSingle;
            }

            // Buttons
            btnCalculate.BackColor = UITheme.AccentColor;
            btnCalculate.FlatAppearance.BorderSize = 0;
            btnCalculate.Font = UITheme.ButtonFont;

            btnClear.BackColor = Color.Transparent;
            btnClear.ForeColor = UITheme.TextSecondary;
            btnClear.FlatAppearance.BorderColor = UITheme.TextMuted;
            btnClear.Font = UITheme.ButtonFont;

            // Panels
            panel1.BackColor = Color.FromArgb(30, 30, 35);
            panel2.BackColor = Color.Transparent;

            label6.ForeColor = label7.ForeColor = label8.ForeColor = UITheme.TextSecondary;

            // DataGrid
            dgvSchedule.BackgroundColor = Color.FromArgb(30, 30, 35);
            dgvSchedule.BorderStyle = BorderStyle.None;
            dgvSchedule.GridColor = Color.FromArgb(45, 45, 50);
            dgvSchedule.EnableHeadersVisualStyles = false;
            dgvSchedule.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(40, 40, 45);
            dgvSchedule.ColumnHeadersDefaultCellStyle.ForeColor = UITheme.TextSecondary;
            dgvSchedule.DefaultCellStyle.BackColor = Color.FromArgb(30, 30, 35);
            dgvSchedule.DefaultCellStyle.ForeColor = UITheme.TextPrimary;
            dgvSchedule.DefaultCellStyle.SelectionBackColor = Color.FromArgb(50, UITheme.AccentColor);
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
