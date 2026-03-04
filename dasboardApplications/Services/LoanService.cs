using System.Linq;
using Microsoft.Data.Sqlite;
using dasboardApplications.Models;
using dasboardApplications.Interfaces;

namespace dasboardApplications.Services
{
    public class LoanService
    {
        private readonly IRepository<LoanModel> _loanRepository;
        private readonly IRepository<Customer> _customerRepository;
        private readonly AuditService _auditService;

        public LoanService(IRepository<LoanModel> loanRepository, IRepository<Customer> customerRepository, AuditService auditService)
        {
            _loanRepository = loanRepository;
            _customerRepository = customerRepository;
            _auditService = auditService;
        }

        public int CreateLoan(LoanModel loan)
        {
            int id = _loanRepository.Add(loan);
            _auditService.LogAction("Create", "Loan", id, $"Created loan of {loan.LoanAmount} for Customer ID {loan.CustomerId}");
            return id;
        }

        public List<LoanModel> GetLoansByCustomer(int customerId)
        {
            return _loanRepository.GetAll().Where(l => l.CustomerId == customerId).ToList();
        }

        public void UpdateBalance(int loanId, double newBalance)
        {
            var loan = _loanRepository.GetById(loanId);
            if (loan != null)
            {
                loan.OutstandingBalance = newBalance;
                if (newBalance <= 0) loan.Status = "Paid";
                _loanRepository.Update(loan);
            }
        }

        public List<LoanViewModel> GetAllLoans()
        {
            var loans = _loanRepository.GetAll().ToList();
            var customers = _customerRepository.GetAll().ToDictionary(c => c.Id, c => c.FullName);

            return loans.Select(l => new LoanViewModel
            {
                Id = l.Id,
                CustomerName = customers.ContainsKey(l.CustomerId) ? customers[l.CustomerId] : "Unknown",
                LoanAmount = l.LoanAmount,
                AnnualInterestRate = l.AnnualInterestRate,
                TenureInMonths = l.TenureInMonths,
                Type = l.Type.ToString(),
                Frequency = l.Frequency.ToString(),
                StartDate = l.StartDate,
                EndDate = l.EndDate,
                Status = l.Status,
                OutstandingBalance = l.OutstandingBalance,
                CreatedAt = l.CreatedAt,
                CustomerId = l.CustomerId
            }).ToList();
        }

        public void UpdateLoan(LoanModel loan)
        {
            _loanRepository.Update(loan);
            _auditService.LogAction("Update", "Loan", loan.Id, $"Updated loan ID {loan.Id}");
        }

        public void DeleteLoan(int id)
        {
            _loanRepository.Delete(id);
            _auditService.LogAction("Delete", "Loan", id, $"Deleted loan ID {id}");
        }
    }
}
