using dasboardApplications.Interfaces;
using dasboardApplications.Services;
using dasboardApplications.Core;
using dasboardApplications.Models;

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

            LoadDefaults();
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
