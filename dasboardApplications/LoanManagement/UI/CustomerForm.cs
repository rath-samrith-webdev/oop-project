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

            ApplyToAll(this.Controls);

            UITheme.StyleButton(saveButton, isPrimary: true);
            UITheme.StyleButton(addNewButton, isPrimary: false);
            UITheme.StyleButton(deleteButton, isPrimary: false, isDanger: true);
            UITheme.StyleButton(refreshButton, isPrimary: false);
            deleteButton.Enabled = false;

            UITheme.StyleLabel(titleLabel, UITheme.LabelLevel.Header);
            UITheme.StyleDataGrid(customerDataGridView);
        }

        private void ApplyToAll(Control.ControlCollection controls)
        {
            foreach (Control ctrl in controls)
            {
                if (ctrl is Label lbl && lbl != titleLabel) UITheme.StyleLabel(lbl, UITheme.LabelLevel.Body);
                else if (ctrl is TextBox txt) UITheme.StyleTextBox(txt);
                else if (ctrl.HasChildren) ApplyToAll(ctrl.Controls);
            }
        }

        private void LoadCustomers()
        {
            _customers = _customerService.GetAllCustomers();
            customerDataGridView.DataSource = null;
            customerDataGridView.DataSource = _customers;
            if (customerDataGridView.Columns["KycDocuments"] != null)
                customerDataGridView.Columns["KycDocuments"].Visible = false;

            // Format ID column to be smaller using FillWeight
            if (customerDataGridView.Columns["Id"] != null)
            {
                customerDataGridView.Columns["Id"].FillWeight = 30; // Much smaller than default 100
                customerDataGridView.Columns["Id"].HeaderText = "ID";
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(nameTextBox.Text) || string.IsNullOrWhiteSpace(emailTextBox.Text))
            {
                MessageBox.Show("Please enter Name and Email.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var customer = _selectedCustomer ?? new Customer();
            customer.FullName = nameTextBox.Text;
            customer.Email = emailTextBox.Text;
            customer.PhoneNumber = phoneTextBox.Text;
            customer.Address = addressTextBox.Text;
            customer.KycDocuments = kycTextBox.Text;
            customer.UpdatedAt = DateTime.Now;

            if (_selectedCustomer == null)
            {
                customer.CreatedAt = DateTime.Now;
                _customerService.CreateCustomer(customer);
                MessageBox.Show("Customer created successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                _customerService.UpdateCustomer(customer);
                MessageBox.Show("Customer updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            ClearForm();
            LoadCustomers();
        }

        private void customerDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (customerDataGridView.SelectedRows.Count > 0)
            {
                _selectedCustomer = (Customer)customerDataGridView.SelectedRows[0].DataBoundItem;
                nameTextBox.Text = _selectedCustomer.FullName;
                emailTextBox.Text = _selectedCustomer.Email;
                phoneTextBox.Text = _selectedCustomer.PhoneNumber;
                addressTextBox.Text = _selectedCustomer.Address;
                kycTextBox.Text = _selectedCustomer.KycDocuments;
                saveButton.Text = "Update Customer";
                deleteButton.Enabled = true;
            }
        }

        private void addNewButton_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            LoadCustomers();
            MessageBox.Show("Customer list refreshed.", "Refresh", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                ClearForm();
                LoadCustomers();
            }
        }

        private void ClearForm()
        {
            _selectedCustomer = null;
            nameTextBox.Clear();
            emailTextBox.Clear();
            phoneTextBox.Clear();
            addressTextBox.Clear();
            kycTextBox.Clear();
            saveButton.Text = "Save Customer";
            deleteButton.Enabled = false;
        }
    }
}
