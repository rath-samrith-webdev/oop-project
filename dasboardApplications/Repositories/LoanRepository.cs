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
    public class LoanRepository : IRepository<LoanModel>
    {
        private readonly string _connectionString;

        public LoanRepository(DatabaseService dbService)
        {
            _connectionString = dbService.GetConnectionString();
        }

        public LoanModel GetById(int id)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Loans WHERE Id = $id";
                command.Parameters.AddWithValue("$id", id);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read()) return MapReaderToLoan(reader);
                }
            }
            return null;
        }

        public IEnumerable<LoanModel> GetAll()
        {
            var loans = new List<LoanModel>();
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Loans ORDER BY Id DESC";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read()) loans.Add(MapReaderToLoan(reader));
                }
            }
            return loans;
        }

        public IEnumerable<LoanModel> Find(Expression<Func<LoanModel, bool>> predicate)
        {
            // Simplified for now
            return GetAll();
        }

        public int Add(LoanModel entity)
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

                command.Parameters.AddWithValue("$customerId", entity.CustomerId);
                command.Parameters.AddWithValue("$amount", entity.LoanAmount);
                command.Parameters.AddWithValue("$rate", entity.AnnualInterestRate);
                command.Parameters.AddWithValue("$tenure", entity.TenureInMonths);
                command.Parameters.AddWithValue("$type", entity.Type.ToString());
                command.Parameters.AddWithValue("$frequency", entity.Frequency.ToString());
                command.Parameters.AddWithValue("$start", entity.StartDate.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("$end", entity.EndDate.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("$status", entity.Status);
                command.Parameters.AddWithValue("$balance", entity.LoanAmount);
                command.Parameters.AddWithValue("$now", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        public void Update(LoanModel entity)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    UPDATE Loans SET
                        LoanAmount = $amount, AnnualInterestRate = $rate,
                        TenureInMonths = $tenure, Type = $type, Frequency = $frequency,
                        Status = $status, OutstandingBalance = $balance, UpdatedAt = $now
                    WHERE Id = $id";

                command.Parameters.AddWithValue("$id", entity.Id);
                command.Parameters.AddWithValue("$amount", entity.LoanAmount);
                command.Parameters.AddWithValue("$rate", entity.AnnualInterestRate);
                command.Parameters.AddWithValue("$tenure", entity.TenureInMonths);
                command.Parameters.AddWithValue("$type", entity.Type.ToString());
                command.Parameters.AddWithValue("$frequency", entity.Frequency.ToString());
                command.Parameters.AddWithValue("$status", entity.Status);
                command.Parameters.AddWithValue("$balance", entity.OutstandingBalance);
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
                command.CommandText = "DELETE FROM Loans WHERE Id = $id";
                command.Parameters.AddWithValue("$id", id);
                command.ExecuteNonQuery();
            }
        }

        private LoanModel MapReaderToLoan(SqliteDataReader reader)
        {
            return new LoanModel
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
            };
        }
    }
}
