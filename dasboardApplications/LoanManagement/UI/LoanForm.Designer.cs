namespace dasboardApplications.Features.LoanManagement
{
    partial class LoanForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.txtLoanAmount = new System.Windows.Forms.TextBox();
            this.txtInterestRate = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTenure = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbLoanType = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbFrequency = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnCalculate = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lblEMI = new System.Windows.Forms.Label();
            this.lblTotalInterest = new System.Windows.Forms.Label();
            this.lblTotalPayment = new System.Windows.Forms.Label();
            this.dgvSchedule = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.labelCustomer = new System.Windows.Forms.Label();
            this.cmbCustomer = new System.Windows.Forms.ComboBox();
            this.btnSaveLoan = new System.Windows.Forms.Button();
            this.btnRecordPayment = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSchedule)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            //
            // labelCustomer
            //
            this.labelCustomer.AutoSize = true;
            this.labelCustomer.Location = new System.Drawing.Point(0, 0);
            this.labelCustomer.Name = "labelCustomer";
            this.labelCustomer.Size = new Size(120, 20);
            this.labelCustomer.TabIndex = 20;
            this.labelCustomer.Text = "Select Customer";
            //
            // cmbCustomer
            //
            this.cmbCustomer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCustomer.Location = new System.Drawing.Point(0, 24);
            this.cmbCustomer.Name = "cmbCustomer";
            this.cmbCustomer.Size = new System.Drawing.Size(420, 32);
            this.cmbCustomer.TabIndex = 21;
            //
            // label1
            //
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 80);
            this.label1.Name = "label1";
            this.label1.Size = new Size(99, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Loan Amount";
            //
            // txtLoanAmount
            //
            this.txtLoanAmount.Location = new System.Drawing.Point(0, 104);
            this.txtLoanAmount.Name = "txtLoanAmount";
            this.txtLoanAmount.Size = new System.Drawing.Size(200, 32);
            this.txtLoanAmount.TabIndex = 1;
            //
            // txtInterestRate
            //
            this.txtInterestRate.Location = new System.Drawing.Point(224, 104);
            this.txtInterestRate.Name = "txtInterestRate";
            this.txtInterestRate.Size = new System.Drawing.Size(196, 32);
            this.txtInterestRate.TabIndex = 3;
            //
            // label2
            //
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(224, 80);
            this.label2.Name = "label2";
            this.label2.Size = new Size(142, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Interest Rate (%) p.a";
            //
            // txtTenure
            //
            this.txtTenure.Location = new System.Drawing.Point(444, 104);
            this.txtTenure.Name = "txtTenure";
            this.txtTenure.Size = new System.Drawing.Size(150, 32);
            this.txtTenure.TabIndex = 5;
            //
            // label3
            //
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(444, 80);
            this.label3.Name = "label3";
            this.label3.Size = new Size(111, 20);
            this.label3.TabIndex = 4;
            this.label3.Text = "Tenure (Months)";
            //
            // cmbLoanType
            //
            this.cmbLoanType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLoanType.Location = new System.Drawing.Point(0, 184);
            this.cmbLoanType.Name = "cmbLoanType";
            this.cmbLoanType.Size = new System.Drawing.Size(200, 32);
            this.cmbLoanType.TabIndex = 7;
            //
            // label4
            //
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(0, 160);
            this.label4.Name = "label4";
            this.label4.Size = new Size(76, 20);
            this.label4.TabIndex = 6;
            this.label4.Text = "Loan Type";
            //
            // cmbFrequency
            //
            this.cmbFrequency.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFrequency.Location = new System.Drawing.Point(224, 184);
            this.cmbFrequency.Name = "cmbFrequency";
            this.cmbFrequency.Size = new System.Drawing.Size(200, 32);
            this.cmbFrequency.TabIndex = 9;
            //
            // label5
            //
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(224, 160);
            this.label5.Name = "label5";
            this.label5.Size = new Size(137, 20);
            this.label5.TabIndex = 8;
            this.label5.Text = "Payment Frequency";
            //
            // btnCalculate
            //
            this.btnCalculate.Location = new System.Drawing.Point(0, 248);
            this.btnCalculate.Name = "btnCalculate";
            this.btnCalculate.Size = new System.Drawing.Size(140, 36);
            this.btnCalculate.TabIndex = 10;
            this.btnCalculate.Text = "Calculate Loan";
            this.btnCalculate.Click += new System.EventHandler(this.btnCalculate_Click);
            //
            // btnSaveLoan
            //
            this.btnSaveLoan.Location = new System.Drawing.Point(152, 248);
            this.btnSaveLoan.Name = "btnSaveLoan";
            this.btnSaveLoan.Size = new System.Drawing.Size(120, 36);
            this.btnSaveLoan.TabIndex = 21;
            this.btnSaveLoan.Text = "Save Loan";
            this.btnSaveLoan.Click += new System.EventHandler(this.btnSaveLoan_Click);
            //
            // btnClear
            //
            this.btnClear.Location = new System.Drawing.Point(284, 248);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(100, 36);
            this.btnClear.TabIndex = 11;
            this.btnClear.Text = "Clear";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            //
            // btnRecordPayment
            //
            this.btnRecordPayment.Location = new System.Drawing.Point(610, 248);
            this.btnRecordPayment.Name = "btnRecordPayment";
            this.btnRecordPayment.Size = new System.Drawing.Size(150, 36);
            this.btnRecordPayment.TabIndex = 22;
            this.btnRecordPayment.Text = "Record Payment";
            this.btnRecordPayment.Click += new System.EventHandler(this.btnRecordPayment_Click);
            //
            // lblEMI
            //
            this.lblEMI.AutoSize = true;
            this.lblEMI.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblEMI.Location = new System.Drawing.Point(15, 35);
            this.lblEMI.Name = "lblEMI";
            this.lblEMI.Size = new System.Drawing.Size(65, 28);
            this.lblEMI.TabIndex = 12;
            this.lblEMI.Text = "$ 0.00";
            //
            // lblTotalInterest
            //
            this.lblTotalInterest.AutoSize = true;
            this.lblTotalInterest.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTotalInterest.Location = new System.Drawing.Point(200, 35);
            this.lblTotalInterest.Name = "lblTotalInterest";
            this.lblTotalInterest.Size = new System.Drawing.Size(65, 28);
            this.lblTotalInterest.TabIndex = 13;
            this.lblTotalInterest.Text = "$ 0.00";
            //
            // lblTotalPayment
            //
            this.lblTotalPayment.AutoSize = true;
            this.lblTotalPayment.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTotalPayment.Location = new System.Drawing.Point(400, 35);
            this.lblTotalPayment.Name = "lblTotalPayment";
            this.lblTotalPayment.Size = new System.Drawing.Size(65, 28);
            this.lblTotalPayment.TabIndex = 14;
            this.lblTotalPayment.Text = "$ 0.00";
            //
            // dgvSchedule
            //
            this.dgvSchedule.AllowUserToAddRows = false;
            this.dgvSchedule.AllowUserToDeleteRows = false;
            this.dgvSchedule.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvSchedule.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSchedule.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSchedule.Location = new System.Drawing.Point(0, 0);
            this.dgvSchedule.Name = "dgvSchedule";
            this.dgvSchedule.ReadOnly = true;
            this.dgvSchedule.RowHeadersWidth = 51;
            this.dgvSchedule.RowTemplate.Height = 29;
            this.dgvSchedule.Size = new System.Drawing.Size(760, 280);
            this.dgvSchedule.TabIndex = 15;
            //
            // panel1
            //
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.lblEMI);
            this.panel1.Controls.Add(this.lblTotalInterest);
            this.panel1.Controls.Add(this.lblTotalPayment);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 520);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new Padding(24);
            this.panel1.Size = new System.Drawing.Size(650, 100);
            this.panel1.TabIndex = 16;
            //
            // panel2
            //
            this.panel2.Controls.Add(this.dgvSchedule);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new Padding(24);
            this.panel2.Size = new System.Drawing.Size(650, 520);
            this.panel2.TabIndex = 17;
            //
            // label6
            //
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 10);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(91, 20);
            this.label6.TabIndex = 15;
            this.label6.Text = "EMI Amount";
            //
            // label7
            //
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(200, 10);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(95, 20);
            this.label7.TabIndex = 16;
            this.label7.Text = "Total Interest";
            //
            // label8
            //
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(400, 10);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(102, 20);
            this.label8.TabIndex = 17;
            this.label8.Text = "Total Payment";
            //
            // LoanForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1200, 750);

            Panel leftPanel = new Panel { Dock = DockStyle.Left, Width = 460, Padding = new Padding(24) };
            Panel rightPanel = new Panel { Dock = DockStyle.Fill };

            rightPanel.Controls.Add(this.panel2); // Filling the top
            rightPanel.Controls.Add(this.panel1); // Docked to bottom

            leftPanel.Controls.Add(this.btnRecordPayment);
            leftPanel.Controls.Add(this.btnSaveLoan);
            leftPanel.Controls.Add(this.cmbCustomer);
            leftPanel.Controls.Add(this.labelCustomer);
            leftPanel.Controls.Add(this.btnClear);
            leftPanel.Controls.Add(this.btnCalculate);
            leftPanel.Controls.Add(this.cmbFrequency);
            leftPanel.Controls.Add(this.label5);
            leftPanel.Controls.Add(this.cmbLoanType);
            leftPanel.Controls.Add(this.label4);
            leftPanel.Controls.Add(this.txtTenure);
            leftPanel.Controls.Add(this.label3);
            leftPanel.Controls.Add(this.txtInterestRate);
            leftPanel.Controls.Add(this.label2);
            leftPanel.Controls.Add(this.txtLoanAmount);
            leftPanel.Controls.Add(this.label1);

            this.Controls.Add(rightPanel);
            this.Controls.Add(leftPanel);
            this.Name = "LoanForm";
            this.Text = "Loan Management System";
            ((System.ComponentModel.ISupportInitialize)(this.dgvSchedule)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtLoanAmount;
        private System.Windows.Forms.TextBox txtInterestRate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTenure;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbLoanType;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbFrequency;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnCalculate;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Label labelCustomer;
        private System.Windows.Forms.ComboBox cmbCustomer;
        private System.Windows.Forms.Button btnSaveLoan;
        private System.Windows.Forms.Button btnRecordPayment;
        private System.Windows.Forms.Label lblEMI;
        private System.Windows.Forms.Label lblTotalInterest;
        private System.Windows.Forms.Label lblTotalPayment;
        private System.Windows.Forms.DataGridView dgvSchedule;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
    }
}
