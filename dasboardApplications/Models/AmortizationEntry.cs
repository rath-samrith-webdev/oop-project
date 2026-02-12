using System;

namespace dasboardApplications.Models
{
    /// <summary>
    /// Represents a single installment in the loan amortization schedule.
    /// Each property describes the state of the loan at a specific payment interval.
    /// </summary>
    public class AmortizationEntry
    {
        /// <summary>
        /// The sequence number of the payment (1, 2, 3...).
        /// </summary>
        public int PaymentNumber { get; set; }

        /// <summary>
        /// Outstanding principal amount before this payment is made.
        /// </summary>
        public double OpeningBalance { get; set; }

        /// <summary>
        /// Total payment amount (Principal + Interest).
        /// </summary>
        public double EMI { get; set; }

        /// <summary>
        /// The portion of the EMI that goes towards reducing the principal.
        /// </summary>
        public double PrincipalComponent { get; set; }

        /// <summary>
        /// The portion of the EMI that covers the interest for the period.
        /// </summary>
        public double InterestComponent { get; set; }

        /// <summary>
        /// Outstanding principal amount after this payment is made.
        /// openingBalance - PrincipalComponent.
        /// </summary>
        public double ClosingBalance { get; set; }

        /// <summary>
        /// Status of this installment (e.g., Paid, Unpaid).
        /// </summary>
        public string Status { get; set; } = "Unpaid";
    }
}
