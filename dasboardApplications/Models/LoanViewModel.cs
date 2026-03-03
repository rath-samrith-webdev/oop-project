namespace dasboardApplications.Models
{
    /// <summary>
    /// A read-only view model for displaying loans in a grid, joining Customer name.
    /// </summary>
    public class LoanViewModel
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = "";
        public double LoanAmount { get; set; }
        public double AnnualInterestRate { get; set; }
        public int TenureInMonths { get; set; }
        public string Type { get; set; } = "";
        public string Frequency { get; set; } = "";
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = "";
        public double OutstandingBalance { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
