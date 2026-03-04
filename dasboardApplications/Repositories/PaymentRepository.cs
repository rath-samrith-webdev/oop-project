using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.Data.Sqlite;
using dasboardApplications.Interfaces;
using dasboardApplications.Models;
using dasboardApplications.Core;
using dasboardApplications.Services;

namespace dasboardApplications.Repositories
{
    public class PaymentRepository : IRepository<Payment>
    {
        private readonly string _connectionString;

        public PaymentRepository(DatabaseService dbService)
        {
            _connectionString = dbService.GetConnectionString();
        }

        public Payment GetById(int id)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Payments WHERE Id = $id";
                command.Parameters.AddWithValue("$id", id);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read()) return MapReaderToPayment(reader);
                }
            }
            return null;
        }

        public IEnumerable<Payment> GetAll()
        {
            var payments = new List<Payment>();
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Payments ORDER BY PaymentDate DESC";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read()) payments.Add(MapReaderToPayment(reader));
                }
            }
            return payments;
        }

        public IEnumerable<Payment> Find(Expression<Func<Payment, bool>> predicate)
        {
            // Simplified
            return GetAll();
        }

        public int Add(Payment entity)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    INSERT INTO Payments (LoanId, PaymentDate, AmountPaid, PaymentType, Status, CreatedAt, UpdatedAt)
                    VALUES ($loanId, $date, $amount, $type, $status, $now, $now);
                    SELECT last_insert_rowid();";

                command.Parameters.AddWithValue("$loanId", entity.LoanId);
                command.Parameters.AddWithValue("$date", entity.PaymentDate.ToString("yyyy-MM-dd HH:mm:ss"));
                command.Parameters.AddWithValue("$amount", entity.AmountPaid);
                command.Parameters.AddWithValue("$type", entity.PaymentType.ToString());
                command.Parameters.AddWithValue("$status", entity.Status);
                command.Parameters.AddWithValue("$now", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        public void Update(Payment entity)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    UPDATE Payments SET
                        AmountPaid = $amount, PaymentType = $type, Status = $status, UpdatedAt = $now
                    WHERE Id = $id";

                command.Parameters.AddWithValue("$id", entity.Id);
                command.Parameters.AddWithValue("$amount", entity.AmountPaid);
                command.Parameters.AddWithValue("$type", entity.PaymentType.ToString());
                command.Parameters.AddWithValue("$status", entity.Status);
                command.Parameters.AddWithValue("$now", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                command.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "DELETE FROM Payments WHERE Id = $id";
                command.Parameters.AddWithValue("$id", id);
                command.ExecuteNonQuery();
            }
        }

        private Payment MapReaderToPayment(SqliteDataReader reader)
        {
            return new Payment
            {
                Id = reader.GetInt32(0),
                LoanId = reader.GetInt32(1),
                PaymentDate = DateTime.Parse(reader.GetString(2)),
                AmountPaid = reader.GetDouble(3),
                PaymentType = reader.GetString(4),
                Status = reader.GetString(5),
                CreatedAt = DateTime.Parse(reader.GetString(6)),
                UpdatedAt = DateTime.Parse(reader.GetString(7))
            };
        }
    }
}
