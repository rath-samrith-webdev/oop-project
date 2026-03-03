namespace dasboardApplications.Features.LoanManagement
{
    partial class PaymentForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            titleLabel = new Label();
            lblLoanInfo = new Label();
            lblAmount = new Label();
            txtAmount = new TextBox();
            lblType = new Label();
            cmbPaymentType = new ComboBox();
            btnSave = new Button();
            btnCancel = new Button();
            SuspendLayout();
            //
            // titleLabel
            //
            this.titleLabel.AutoSize = true;
            this.titleLabel.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.titleLabel.Location = new System.Drawing.Point(24, 24);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(205, 32);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "Record Payment";
            //
            // lblLoanInfo
            //
            this.lblLoanInfo.AutoSize = true;
            this.lblLoanInfo.Location = new System.Drawing.Point(24, 72);
            this.lblLoanInfo.Name = "lblLoanInfo";
            this.lblLoanInfo.Size = new System.Drawing.Size(61, 20);
            this.lblLoanInfo.TabIndex = 1;
            this.lblLoanInfo.Text = "Loan Info";
            //
            // lblAmount
            //
            this.lblAmount.AutoSize = true;
            this.lblAmount.Location = new System.Drawing.Point(24, 112);
            this.lblAmount.Name = "lblAmount";
            this.lblAmount.Size = new Size(81, 20);
            this.lblAmount.TabIndex = 2;
            this.lblAmount.Text = "Amount Paid";
            //
            // txtAmount
            //
            this.txtAmount.Location = new System.Drawing.Point(24, 136);
            this.txtAmount.Name = "txtAmount";
            this.txtAmount.Size = new System.Drawing.Size(272, 32);
            this.txtAmount.TabIndex = 3;
            //
            // lblType
            //
            this.lblType.AutoSize = true;
            this.lblType.Location = new System.Drawing.Point(24, 184);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(81, 20);
            this.lblType.TabIndex = 4;
            this.lblType.Text = "Payment Type";
            //
            // cmbPaymentType
            //
            this.cmbPaymentType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPaymentType.Items.AddRange(new object[] {
            "Cash",
            "Bank Transfer",
            "Credit Card",
            "Cheque"});
            this.cmbPaymentType.Location = new System.Drawing.Point(24, 208);
            this.cmbPaymentType.Name = "cmbPaymentType";
            this.cmbPaymentType.Size = new System.Drawing.Size(272, 32);
            this.cmbPaymentType.TabIndex = 5;
            //
            // btnSave
            //
            this.btnSave.Location = new System.Drawing.Point(24, 264);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(120, 36);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "Save Payment";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            //
            // btnCancel
            //
            this.btnCancel.Location = new System.Drawing.Point(176, 264);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(120, 36);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            //
            // PaymentForm
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(320, 280);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(cmbPaymentType);
            Controls.Add(lblType);
            Controls.Add(txtAmount);
            Controls.Add(lblAmount);
            Controls.Add(lblLoanInfo);
            Controls.Add(titleLabel);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "PaymentForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Record Payment";
            ResumeLayout(false);
            PerformLayout();
        }

        private Label titleLabel;
        private Label lblLoanInfo;
        private Label lblAmount;
        private TextBox txtAmount;
        private Label lblType;
        private ComboBox cmbPaymentType;
        private Button btnSave;
        private Button btnCancel;
    }
}
