using System;
using System.Drawing;
using System.Windows.Forms;
using dasboardApplications.Core;
using dasboardApplications.Models;

namespace dasboardApplications.Features.LoanManagement
{
    public partial class CustomerEditorDialog : Form
    {
        public Customer Customer { get; private set; }
        private bool _isEditMode;

        public CustomerEditorDialog(Customer customer = null)
        {
            InitializeComponent();
            _isEditMode = customer != null;
            Customer = customer ?? new Customer();

            if (_isEditMode)
            {
                this.Text = "Edit Customer";
                saveButton.Text = "Update Customer";
                PopulateFields();
            }
            else
            {
                this.Text = "Add New Customer";
                saveButton.Text = "Create Customer";
            }

            ApplyTheme();
        }

        private void PopulateFields()
        {
            nameTextBox.Text = Customer.FullName;
            emailTextBox.Text = Customer.Email;
            phoneTextBox.Text = Customer.PhoneNumber;
            addressTextBox.Text = Customer.Address;
            kycTextBox.Text = Customer.KycDocuments;
        }

        private void ApplyTheme()
        {
            this.BackColor = UITheme.SecondaryBackground;
            this.ForeColor = UITheme.TextPrimary;

            UITheme.StyleLabel(headerLabel, UITheme.LabelLevel.Header);
            UITheme.StyleLabel(nameLabel, UITheme.LabelLevel.Body);
            UITheme.StyleLabel(emailLabel, UITheme.LabelLevel.Body);
            UITheme.StyleLabel(phoneLabel, UITheme.LabelLevel.Body);
            UITheme.StyleLabel(addressLabel, UITheme.LabelLevel.Body);
            UITheme.StyleLabel(kycLabel, UITheme.LabelLevel.Body);

            UITheme.StyleTextBox(nameTextBox);
            UITheme.StyleTextBox(emailTextBox);
            UITheme.StyleTextBox(phoneTextBox);
            UITheme.StyleTextBox(addressTextBox);
            UITheme.StyleTextBox(kycTextBox);

            UITheme.StyleButton(saveButton, isPrimary: true);
            UITheme.StyleButton(cancelButton, isPrimary: false);
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(nameTextBox.Text) || string.IsNullOrWhiteSpace(emailTextBox.Text))
            {
                MessageBox.Show("Please enter Name and Email.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Customer.FullName = nameTextBox.Text.Trim();
            Customer.Email = emailTextBox.Text.Trim();
            Customer.PhoneNumber = phoneTextBox.Text.Trim();
            Customer.Address = addressTextBox.Text.Trim();
            Customer.KycDocuments = kycTextBox.Text.Trim();
            Customer.UpdatedAt = DateTime.Now;

            if (!_isEditMode)
            {
                Customer.CreatedAt = DateTime.Now;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
