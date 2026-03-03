using System;

namespace dasboardApplications.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public int LoanId { get; set; }
        public DateTime PaymentDate { get; set; }
        public double AmountPaid { get; set; }
        public string PaymentType { get; set; } // Cash, BankTransfer, etc.
        public string Status { get; set; } // Pending, Completed, Failed
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
