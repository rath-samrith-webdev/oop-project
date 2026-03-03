using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using dasboardApplications.Models;

namespace dasboardApplications.Services
{
    public class LoanService
    {
        private readonly string _connectionString;
        private readonly AuditService _auditService;

        public LoanService(DatabaseService databaseService, AuditService auditService)
        {
            _connectionString = databaseService.GetConnectionString();
            _auditService = auditService;
        }

        public int CreateLoan(LoanModel loan)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    INSERT INTO Loans (
                        CustomerId, LoanAmount, AnnualInterestRate, TenureInMonths,
                        Type, Frequency, StartDate, EndDate, Status, OutstandingBalance,
                        CreatedAt, UpdatedAt
                    )
                    VALUES (
                        $customerId, $amount, $rate, $tenure,
                        $type, $frequency, $start, $end, $status, $balance,
                        $now, $now
                    );
                    SELECT last_insert_rowid();";

                command.Parameters.AddWithValue("$customerId", loan.CustomerId);
                command.Parameters.AddWithValue("$amount", loan.LoanAmount);
                command.Parameters.AddWithValue("$rate", loan.AnnualInterestRate);
                command.Parameters.AddWithValue("$tenure", loan.TenureInMonths);
                command.Parameters.AddWithValue("$type", loan.Type.ToString());
                command.Parameters.AddWithValue("$frequency", loan.Frequency.ToString());
                command.Parameters.AddWithValue("$start", loan.StartDate.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("$end", loan.EndDate.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("$status", loan.Status);
                command.Parameters.AddWithValue("$balance", loan.LoanAmount); // Initial balance is the principal
                command.Parameters.AddWithValue("$now", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                int id = Convert.ToInt32(command.ExecuteScalar());
                _auditService.LogAction("Create", "Loan", id, $"Created loan of {loan.LoanAmount} for Customer ID {loan.CustomerId}");
                return id;
            }
        }

        public List<LoanModel> GetLoansByCustomer(int customerId)
        {
            var loans = new List<LoanModel>();
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Loans WHERE CustomerId = $customerId";
                command.Parameters.AddWithValue("$customerId", customerId);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        loans.Add(new LoanModel
                        {
                            Id = reader.GetInt32(0),
                            CustomerId = reader.GetInt32(1),
                            LoanAmount = reader.GetDouble(2),
                            AnnualInterestRate = reader.GetDouble(3),
                            TenureInMonths = reader.GetInt32(4),
                            Type = Enum.Parse<LoanType>(reader.GetString(5)),
                            Frequency = Enum.Parse<PaymentFrequency>(reader.GetString(6)),
                            StartDate = DateTime.Parse(reader.GetString(7)),
                            EndDate = DateTime.Parse(reader.GetString(8)),
                            Status = reader.GetString(9),
                            OutstandingBalance = reader.GetDouble(10),
                            CreatedAt = DateTime.Parse(reader.GetString(11)),
                            UpdatedAt = DateTime.Parse(reader.GetString(12))
                        });
                    }
                }
            }
            return loans;
        }

        public void UpdateBalance(int loanId, double newBalance)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "UPDATE Loans SET OutstandingBalance = $balance, UpdatedAt = $now WHERE Id = $id";
                command.Parameters.AddWithValue("$id", loanId);
                command.Parameters.AddWithValue("$balance", newBalance);
                command.Parameters.AddWithValue("$now", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                command.ExecuteNonQuery();

                if (newBalance <= 0)
                {
                    command.CommandText = "UPDATE Loans SET Status = 'Paid' WHERE Id = $id";
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<LoanViewModel> GetAllLoans()
        {
            var loans = new List<LoanViewModel>();
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    SELECT l.Id, c.FullName, l.LoanAmount, l.AnnualInterestRate,
                           l.TenureInMonths, l.Type, l.Frequency, l.StartDate,
                           l.EndDate, l.Status, l.OutstandingBalance, l.CreatedAt, l.CustomerId
                    FROM Loans l
                    INNER JOIN Customers c ON l.CustomerId = c.Id
                    ORDER BY l.Id DESC";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        loans.Add(new LoanViewModel
                        {
                            Id = reader.GetInt32(0),
                            CustomerName = reader.GetString(1),
                            LoanAmount = reader.GetDouble(2),
                            AnnualInterestRate = reader.GetDouble(3),
                            TenureInMonths = reader.GetInt32(4),
                            Type = reader.GetString(5),
                            Frequency = reader.GetString(6),
                            StartDate = DateTime.Parse(reader.GetString(7)),
                            EndDate = DateTime.Parse(reader.GetString(8)),
                            Status = reader.GetString(9),
                            OutstandingBalance = reader.GetDouble(10),
                            CreatedAt = DateTime.Parse(reader.GetString(11)),
                            CustomerId = reader.GetInt32(12)
                        });
                    }
                }
            }
            return loans;
        }

        public void UpdateLoan(LoanModel loan)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    UPDATE Loans SET
                        LoanAmount = $amount, AnnualInterestRate = $rate,
                        TenureInMonths = $tenure, Type = $type, Frequency = $frequency,
                        Status = $status, UpdatedAt = $now
                    WHERE Id = $id";
                command.Parameters.AddWithValue("$id", loan.Id);
                command.Parameters.AddWithValue("$amount", loan.LoanAmount);
                command.Parameters.AddWithValue("$rate", loan.AnnualInterestRate);
                command.Parameters.AddWithValue("$tenure", loan.TenureInMonths);
                command.Parameters.AddWithValue("$type", loan.Type.ToString());
                command.Parameters.AddWithValue("$frequency", loan.Frequency.ToString());
                command.Parameters.AddWithValue("$status", loan.Status);
                command.Parameters.AddWithValue("$now", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                command.ExecuteNonQuery();
                _auditService.LogAction("Update", "Loan", loan.Id, $"Updated loan ID {loan.Id}");
            }
        }

        public void DeleteLoan(int id)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "DELETE FROM Payments WHERE LoanId = $id";
                command.Parameters.AddWithValue("$id", id);
                command.ExecuteNonQuery();

                command.CommandText = "DELETE FROM Loans WHERE Id = $id";
                command.ExecuteNonQuery();

                _auditService.LogAction("Delete", "Loan", id, $"Deleted loan ID {id} with its payments.");
            }
        }
    }
}
