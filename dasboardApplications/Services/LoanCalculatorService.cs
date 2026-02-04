using dasboardApplications.Models;

namespace dasboardApplications.Services
{
    /// <summary>
    /// Service responsible for performing financial calculations related to loans.
    /// Handles EMI calculation and full amortization schedule generation.
    /// </summary>
    public class LoanCalculatorService
    {
        private const int MONTHS_IN_YEAR = 12;

        /// <summary>
        /// Calculates the Equated Monthly Installment (EMI) based on the loan type.
        /// </summary>
        /// <param name="loan">The loan parameters.</param>
        /// <returns>The calculated EMI amount.</returns>
        public double CalculateEMI(LoanModel loan)
        {
            if (loan.Type == LoanType.FlatRate)
            {
                // Formula: (Principal + (Principal * Rate * Time)) / Total Installments
                double totalInterest = (loan.LoanAmount * (loan.AnnualInterestRate / 100) * (loan.TenureInMonths / 12.0));
                double totalAmount = loan.LoanAmount + totalInterest;
                int totalPayments = loan.TenureInMonths / (int)loan.Frequency;
                return Math.Round(totalAmount / totalPayments, 2);
            }
            else
            {
                // Reducing Balance Formula: [P x R x (1+R)^N]/[(1+R)^N-1]
                double monthlyRate = (loan.AnnualInterestRate / 100) / (MONTHS_IN_YEAR / (int)loan.Frequency);
                int totalPayments = loan.TenureInMonths / (int)loan.Frequency;

                if (monthlyRate == 0) return Math.Round(loan.LoanAmount / totalPayments, 2);

                double emi = (loan.LoanAmount * monthlyRate * Math.Pow(1 + monthlyRate, totalPayments)) /
                             (Math.Pow(1 + monthlyRate, totalPayments) - 1);

                return Math.Round(emi, 2);
            }
        }

        /// <summary>
        /// Generates a full amortization schedule for the given loan.
        /// </summary>
        /// <param name="loan">The loan parameters.</param>
        /// <returns>A list of amortization entries.</returns>
        public List<AmortizationEntry> GenerateAmortizationSchedule(LoanModel loan)
        {
            var schedule = new List<AmortizationEntry>();
            double emi = CalculateEMI(loan);
            double remainingBalance = loan.LoanAmount;
            int totalPayments = loan.TenureInMonths / (int)loan.Frequency;
            double periodicRate = (loan.AnnualInterestRate / 100) / (MONTHS_IN_YEAR / (int)loan.Frequency);

            for (int i = 1; i <= totalPayments; i++)
            {
                double interest;
                double principal;

                if (loan.Type == LoanType.FlatRate)
                {
                    // For Flat Rate, interest is constant every month based on initial principal
                    interest = (loan.LoanAmount * (loan.AnnualInterestRate / 100) * ((int)loan.Frequency / 12.0));
                    principal = emi - interest;
                }
                else
                {
                    // For Reducing Balance, interest is calculated on the remaining balance
                    interest = remainingBalance * periodicRate;
                    principal = emi - interest;
                }

                // Handling the last payment rounding
                if (i == totalPayments)
                {
                    principal = remainingBalance;
                    emi = principal + interest;
                }

                var entry = new AmortizationEntry
                {
                    PaymentNumber = i,
                    OpeningBalance = Math.Round(remainingBalance, 2),
                    EMI = Math.Round(emi, 2),
                    InterestComponent = Math.Round(interest, 2),
                    PrincipalComponent = Math.Round(principal, 2),
                    ClosingBalance = Math.Round(remainingBalance - principal, 2)
                };

                schedule.Add(entry);
                remainingBalance -= principal;

                if (remainingBalance < 0) remainingBalance = 0;
            }

            return schedule;
        }

        /// <summary>
        /// Calculates the total interest payable over the loan tenure.
        /// </summary>
        public double CalculateTotalInterest(List<AmortizationEntry> schedule)
        {
            double total = 0;
            foreach (var entry in schedule) total += entry.InterestComponent;
            return Math.Round(total, 2);
        }

        /// <summary>
        /// Calculates the total amount payable (Principal + Interest).
        /// </summary>
        public double CalculateTotalPayment(List<AmortizationEntry> schedule)
        {
            double total = 0;
            foreach (var entry in schedule) total += entry.EMI;
            return Math.Round(total, 2);
        }
    }
}
