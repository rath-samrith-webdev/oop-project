using System;

namespace dasboardApplications.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        /// <summary>
        /// Encrypted base64 string of KYC documents.
        /// </summary>
        public string KycDocuments { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
