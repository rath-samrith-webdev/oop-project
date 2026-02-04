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
    /// Represents the input parameters for a loan calculation.
    /// This model holds all the user-provided data required to calculate EMI and schedules.
    /// </summary>
    public class LoanModel
    {
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
    }
}
