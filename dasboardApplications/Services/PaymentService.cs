using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Sqlite;
using dasboardApplications.Models;
using dasboardApplications.Interfaces;

namespace dasboardApplications.Services
{
    public class PaymentService
    {
        private readonly IRepository<Payment> _paymentRepository;
        private readonly IRepository<LoanModel> _loanRepository;
        private readonly LoanService _loanService;
        private readonly AuditService _auditService;

        public PaymentService(IRepository<Payment> paymentRepository, IRepository<LoanModel> loanRepository, LoanService loanService, AuditService auditService)
        {
            _paymentRepository = paymentRepository;
            _loanRepository = loanRepository;
            _loanService = loanService;
            _auditService = auditService;
        }

        public int RecordPayment(Payment payment)
        {
            int id = _paymentRepository.Add(payment);

            // Update Loan Balance
            UpdateLoanBalance(payment.LoanId, payment.AmountPaid);

            _auditService.LogAction("Create", "Payment", id, $"Recorded payment of {payment.AmountPaid} for Loan ID {payment.LoanId}");
            return id;
        }

        private void UpdateLoanBalance(int loanId, double amountPaid)
        {
            var loan = _loanRepository.GetById(loanId);
            if (loan != null)
            {
                double newBalance = Math.Max(0, loan.OutstandingBalance - amountPaid);
                _loanService.UpdateBalance(loanId, newBalance);
            }
        }

        public List<Payment> GetPaymentsByLoan(int loanId)
        {
            return _paymentRepository.GetAll().Where(p => p.LoanId == loanId).ToList();
        }
    }
}
