using Xunit;
using dasboardApplications.Services;
using dasboardApplications.Models;

namespace dasboardApplications.Tests
{
    public class LoanCalculatorTests
    {
        private readonly LoanCalculatorService _calculator;

        public LoanCalculatorTests()
        {
            _calculator = new LoanCalculatorService();
        }

        [Fact]
        public void CalculateEMI_FlatRate_ReturnsCorrectAmount()
        {
            var loan = new LoanModel
            {
                LoanAmount = 10000,
                AnnualInterestRate = 12,
                TenureInMonths = 12,
                Type = LoanType.FlatRate,
                Frequency = PaymentFrequency.Monthly
            };

            // Interest = 10000 * 0.12 * 1 = 1200
            // Total = 11200
            // EMI = 11200 / 12 = 933.33
            double emi = _calculator.CalculateEMI(loan);
            Assert.Equal(933.33, emi);
        }

        [Fact]
        public void CalculateEMI_ReducingBalance_ReturnsCorrectAmount()
        {
            var loan = new LoanModel
            {
                LoanAmount = 10000,
                AnnualInterestRate = 12,
                TenureInMonths = 12,
                Type = LoanType.ReducingBalance,
                Frequency = PaymentFrequency.Monthly
            };

            // EMI = [10000 * 0.01 * (1.01)^12] / [(1.01)^12 - 1] = 888.49
            double emi = _calculator.CalculateEMI(loan);
            Assert.Equal(888.49, emi);
        }
    }
}
