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
    public partial class LoanListForm : BaseFeatureForm, IFeature
    {
        public string FeatureName => "Loan List";
        public Form GetForm() => this;

        private readonly LoanService _loanService;
        private List<LoanViewModel> _loans;
        private LoanViewModel _selectedLoan;

        public LoanListForm()
        {
            InitializeComponent();
            _loanService = dasboardApplications.Core.ServiceContainer.GetService<LoanService>();
            ApplyTheme();
            LoadLoans();
        }

        private void ApplyTheme()
        {
            this.BackColor = UITheme.PrimaryBackground;
            this.ForeColor = UITheme.TextPrimary;

            if (titleLabel != null) UITheme.StyleLabel(titleLabel, UITheme.LabelLevel.Header);

            UITheme.StyleButton(btnEdit, isPrimary: true);
            UITheme.StyleButton(btnDelete, isPrimary: false, isDanger: true);
            UITheme.StyleButton(btnRefresh, isPrimary: false);

            UITheme.StyleDataGrid(dgvLoans);

            // Edit panel theme
            pnlEdit.BackColor = UITheme.SecondaryBackground;
            if (lblEditTitle != null) UITheme.StyleLabel(lblEditTitle, UITheme.LabelLevel.Header);
            foreach (Label lbl in pnlEdit.Controls.OfType<Label>())
                if (lbl != lblEditTitle) UITheme.StyleLabel(lbl, UITheme.LabelLevel.Body);
            foreach (TextBox txt in pnlEdit.Controls.OfType<TextBox>())
                UITheme.StyleTextBox(txt);

            cmbStatus.BackColor = UITheme.SecondaryBackground;
            cmbStatus.ForeColor = UITheme.TextPrimary;
            cmbStatus.Font = UITheme.BodyFont;

            UITheme.StyleButton(btnSaveEdit, isPrimary: true);
            UITheme.StyleButton(btnCancelEdit, isPrimary: false);
        }

        private void LoadLoans()
        {
            _loans = _loanService.GetAllLoans();
            dgvLoans.DataSource = null;
            dgvLoans.DataSource = _loans;

            // Hide internal Id columns from user view
            if (dgvLoans.Columns.Contains("CustomerId"))
                dgvLoans.Columns["CustomerId"].Visible = false;

            if (dgvLoans.Columns.Contains("OutstandingBalance"))
                dgvLoans.Columns["OutstandingBalance"].DefaultCellStyle.Format = "N2";
            if (dgvLoans.Columns.Contains("LoanAmount"))
                dgvLoans.Columns["LoanAmount"].DefaultCellStyle.Format = "N2";
        }

        private void dgvLoans_SelectionChanged(object sender, EventArgs e)
        {
            bool hasSelection = dgvLoans.SelectedRows.Count > 0 && dgvLoans.SelectedRows[0].DataBoundItem != null;
            btnEdit.Enabled = hasSelection;
            btnDelete.Enabled = hasSelection;
            if (hasSelection)
                _selectedLoan = (LoanViewModel)dgvLoans.SelectedRows[0].DataBoundItem;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (_selectedLoan == null) return;

            txtAmount.Text = _selectedLoan.LoanAmount.ToString();
            txtRate.Text = _selectedLoan.AnnualInterestRate.ToString();
            txtTenure.Text = _selectedLoan.TenureInMonths.ToString();
            cmbStatus.SelectedItem = _selectedLoan.Status;

            dgvLoans.Visible = false;
            pnlEdit.Visible = true;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
        }

        private void btnSaveEdit_Click(object sender, EventArgs e)
        {
            if (_selectedLoan == null) return;

            if (!double.TryParse(txtAmount.Text, out double amount) || amount <= 0 ||
                !double.TryParse(txtRate.Text, out double rate) || rate < 0 ||
                !int.TryParse(txtTenure.Text, out int tenure) || tenure <= 0)
            {
                MessageBox.Show("Please enter valid numeric values.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var updatedLoan = new LoanModel
            {
                Id = _selectedLoan.Id,
                LoanAmount = amount,
                AnnualInterestRate = rate,
                TenureInMonths = tenure,
                Type = Enum.Parse<LoanType>(_selectedLoan.Type),
                Frequency = Enum.Parse<PaymentFrequency>(_selectedLoan.Frequency),
                Status = cmbStatus.SelectedItem?.ToString() ?? _selectedLoan.Status
            };

            _loanService.UpdateLoan(updatedLoan);
            MessageBox.Show("Loan updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            HideEditPanel();
            LoadLoans();
        }

        private void btnCancelEdit_Click(object sender, EventArgs e)
        {
            HideEditPanel();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedLoan == null) return;

            var result = MessageBox.Show(
                $"Delete loan #{_selectedLoan.Id} for {_selectedLoan.CustomerName}?\n\nAmount: {_selectedLoan.LoanAmount:N2}\nThis will also delete all payment records for this loan.",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                _loanService.DeleteLoan(_selectedLoan.Id);
                MessageBox.Show("Loan deleted.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _selectedLoan = null;
                LoadLoans();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadLoans();
        }

        private void HideEditPanel()
        {
            pnlEdit.Visible = false;
            dgvLoans.Visible = true;
        }
    }
}
