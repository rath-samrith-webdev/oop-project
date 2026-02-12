using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using dasboardApplications.Models;

namespace dasboardApplications.Services
{
    public class PaymentService
    {
        private readonly string _connectionString;
        private readonly LoanService _loanService;
        private readonly AuditService _auditService;

        public PaymentService(DatabaseService databaseService, LoanService loanService, AuditService auditService)
        {
            _connectionString = databaseService.GetConnectionString();
            _loanService = loanService;
            _auditService = auditService;
        }

        public int RecordPayment(Payment payment)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    INSERT INTO Payments (LoanId, PaymentDate, AmountPaid, PaymentType, Status, CreatedAt, UpdatedAt)
                    VALUES ($loanId, $date, $amount, $type, $status, $now, $now);
                    SELECT last_insert_rowid();";

                command.Parameters.AddWithValue("$loanId", payment.LoanId);
                command.Parameters.AddWithValue("$date", payment.PaymentDate.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("$amount", payment.AmountPaid);
                command.Parameters.AddWithValue("$type", payment.PaymentType);
                command.Parameters.AddWithValue("$status", payment.Status);
                command.Parameters.AddWithValue("$now", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                int id = Convert.ToInt32(command.ExecuteScalar());

                // Update Loan Balance
                UpdateLoanBalance(payment.LoanId, payment.AmountPaid);

                _auditService.LogAction("Create", "Payment", id, $"Recorded payment of {payment.AmountPaid} for Loan ID {payment.LoanId}");
                return id;
            }
        }

        private void UpdateLoanBalance(int loanId, double amountPaid)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT OutstandingBalance FROM Loans WHERE Id = $id";
                command.Parameters.AddWithValue("$id", loanId);

                double currentBalance = Convert.ToDouble(command.ExecuteScalar());
                double newBalance = Math.Max(0, currentBalance - amountPaid);

                _loanService.UpdateBalance(loanId, newBalance);
            }
        }

        public List<Payment> GetPaymentsByLoan(int loanId)
        {
            var payments = new List<Payment>();
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Payments WHERE LoanId = $loanId";
                command.Parameters.AddWithValue("$loanId", loanId);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        payments.Add(new Payment
                        {
                            Id = reader.GetInt32(0),
                            LoanId = reader.GetInt32(1),
                            PaymentDate = DateTime.Parse(reader.GetString(2)),
                            AmountPaid = reader.GetDouble(3),
                            PaymentType = reader.GetString(4),
                            Status = reader.GetString(5),
                            CreatedAt = DateTime.Parse(reader.GetString(6)),
                            UpdatedAt = DateTime.Parse(reader.GetString(7))
                        });
                    }
                }
            }
            return payments;
        }
    }
}
