using System;

namespace dasboardApplications.Services
{
    /// <summary>
    /// Service to validate user inputs for loan calculations.
    /// Ensures that inputs are within realistic and mathematically sound boundaries.
    /// </summary>
    public class ValidationService
    {
        /// <summary>
        /// Validates that the loan amount is greater than zero and within reasonable limits.
        /// </summary>
        public (bool isValid, string message) ValidateLoanAmount(string input)
        {
            if (!double.TryParse(input, out double amount))
                return (false, "Loan amount must be a numeric value.");

            if (amount <= 0)
                return (false, "Loan amount must be greater than zero.");

            if (amount > 1_000_000_000) // 1 Billion cap for sanity
                return (false, "Loan amount exceeds the maximum limit allowed.");

            return (true, string.Empty);
        }

        /// <summary>
        /// Validates that the interest rate is non-negative and realistic.
        /// </summary>
        public (bool isValid, string message) ValidateInterestRate(string input)
        {
            if (!double.TryParse(input, out double rate))
                return (false, "Interest rate must be a numeric value.");

            if (rate < 0)
                return (false, "Interest rate cannot be negative.");

            if (rate > 100)
                return (false, "Interest rate cannot exceed 100%.");

            return (true, string.Empty);
        }

        /// <summary>
        /// Validates that the tenure is at least 1 month and fits the payment frequency.
        /// </summary>
        public (bool isValid, string message) ValidateTenure(string input, int frequency)
        {
            if (!int.TryParse(input, out int months))
                return (false, "Tenure must be an integer (months).");

            if (months <= 0)
                return (false, "Tenure must be at least 1 month.");

            if (months % frequency != 0)
                return (false, $"Tenure must be divisible by the payment frequency ({frequency} months).");

            if (months > 600) // 50 years cap
                return (false, "Tenure cannot exceed 600 months (50 years).");

            return (true, string.Empty);
        }
    }
}
