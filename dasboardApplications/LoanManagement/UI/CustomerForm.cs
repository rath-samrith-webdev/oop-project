using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using dasboardApplications.Core;
using dasboardApplications.Models;
using dasboardApplications.Services;
using dasboardApplications.Interfaces;

namespace dasboardApplications.Features.LoanManagement
{
    public partial class CustomerForm : BaseFeatureForm, IFeature
    {
        public string FeatureName => "Customer Management";
        public Form GetForm() => this;

        private readonly CustomerService _customerService;
        private List<Customer> _customers;
        private Customer _selectedCustomer;

        public CustomerForm()
        {
            InitializeComponent();
            _customerService = dasboardApplications.Core.ServiceContainer.GetService<CustomerService>();
            ApplyTheme();
            LoadCustomers();
        }

        private void ApplyTheme()
        {
            this.BackColor = UITheme.PrimaryBackground;
            this.ForeColor = UITheme.TextPrimary;

            UITheme.StyleButton(addNewButton, isPrimary: true);
            UITheme.StyleButton(editButton, isPrimary: false);
            UITheme.StyleButton(deleteButton, isPrimary: false, isDanger: true);
            UITheme.StyleButton(refreshButton, isPrimary: false);

            editButton.Enabled = false;
            deleteButton.Enabled = false;

            UITheme.StyleLabel(titleLabel, UITheme.LabelLevel.Header);
            UITheme.StyleDataGrid(customerDataGridView);
        }

        private void LoadCustomers()
        {
            _customers = _customerService.GetAllCustomers();
            customerDataGridView.DataSource = null;
            customerDataGridView.DataSource = _customers;
            if (customerDataGridView.Columns["KycDocuments"] != null)
                customerDataGridView.Columns["KycDocuments"].Visible = false;

            if (customerDataGridView.Columns["Id"] != null)
            {
                customerDataGridView.Columns["Id"].FillWeight = 30;
                customerDataGridView.Columns["Id"].HeaderText = "ID";
            }
        }

        private void addNewButton_Click(object sender, EventArgs e)
        {
            using (var dialog = new CustomerEditorDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    _customerService.CreateCustomer(dialog.Customer);
                    MessageBox.Show("Customer created successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadCustomers();
                }
            }
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            if (_selectedCustomer == null) return;

            using (var dialog = new CustomerEditorDialog(_selectedCustomer))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    _customerService.UpdateCustomer(dialog.Customer);
                    MessageBox.Show("Customer updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadCustomers();
                }
            }
        }

        private void customerDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (customerDataGridView.SelectedRows.Count > 0)
            {
                _selectedCustomer = (Customer)customerDataGridView.SelectedRows[0].DataBoundItem;
                editButton.Enabled = true;
                deleteButton.Enabled = true;
            }
            else
            {
                _selectedCustomer = null;
                editButton.Enabled = false;
                deleteButton.Enabled = false;
            }
        }

        private void customerDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                editButton_Click(sender, e);
            }
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            LoadCustomers();
        }

        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {
            var filter = searchTextBox.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(filter))
            {
                customerDataGridView.DataSource = _customers;
            }
            else
            {
                customerDataGridView.DataSource = _customers.Where(c =>
                    c.FullName.ToLower().Contains(filter) ||
                    c.Email.ToLower().Contains(filter) ||
                    c.PhoneNumber.ToLower().Contains(filter)).ToList();
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (_selectedCustomer == null) return;

            var result = MessageBox.Show(
                $"Are you sure you want to delete \"{_selectedCustomer.FullName}\"?\n\nThis will also delete all their loans and payment records.",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                _customerService.DeleteCustomer(_selectedCustomer.Id);
                MessageBox.Show("Customer deleted successfully.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadCustomers();
            }
        }
    }
}
