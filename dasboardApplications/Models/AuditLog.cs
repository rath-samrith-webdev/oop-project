using System;

namespace dasboardApplications.Models
{
    public class AuditLog
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Action { get; set; } // Create, Update, Delete
        public string EntityName { get; set; } // Customer, Loan, Payment
        public int EntityId { get; set; }
        public string Changes { get; set; } // JSON or simple string of changes
        public DateTime Timestamp { get; set; }
    }
}
