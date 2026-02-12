using System;

namespace dasboardApplications.Models
{
    public enum LoanType
    {
        ReducingBalance,
        FlatRate
    }

    public enum PaymentFrequency
    {
        Monthly = 1,
        Quarterly = 3
    }

    /// <summary>
    /// Represents the input parameters and state for a loan.
    /// </summary>
    public class LoanModel
    {
        public int Id { get; set; }

        /// <summary>
        /// ID of the customer who took the loan.
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// Total principal amount borrowed.
        /// </summary>
        public double LoanAmount { get; set; }

        /// <summary>
        /// Annual interest rate in percentage (e.g., 10.5 for 10.5%).
        /// </summary>
        public double AnnualInterestRate { get; set; }

        /// <summary>
        /// Total duration of the loan in months.
        /// </summary>
        public int TenureInMonths { get; set; }

        /// <summary>
        /// The method used for interest calculation (Reducing vs Flat).
        /// </summary>
        public LoanType Type { get; set; }

        /// <summary>
        /// How often payments are made.
        /// </summary>
        public PaymentFrequency Frequency { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = "Active"; // e.g., Active, Paid, Defaulted
        public double OutstandingBalance { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public string LoanSummary => $"Loan #{Id}: {LoanAmount:N2} ({Type})";
    }
}
