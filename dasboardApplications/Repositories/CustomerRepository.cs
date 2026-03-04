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
    public class CustomerRepository : IRepository<Customer>
    {
        private readonly string _connectionString;

        public CustomerRepository(DatabaseService dbService)
        {
            _connectionString = dbService.GetConnectionString();
        }

        public Customer GetById(int id)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Id, FullName, Email, PhoneNumber, Address, KycDocuments, CreatedAt, UpdatedAt FROM Customers WHERE Id = $id";
                command.Parameters.AddWithValue("$id", id);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read()) return MapReaderToCustomer(reader);
                }
            }
            return null;
        }

        public IEnumerable<Customer> GetAll()
        {
            var customers = new List<Customer>();
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Id, FullName, Email, PhoneNumber, Address, KycDocuments, CreatedAt, UpdatedAt FROM Customers";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read()) customers.Add(MapReaderToCustomer(reader));
                }
            }
            return customers;
        }

        public IEnumerable<Customer> Find(Expression<Func<Customer, bool>> predicate)
        {
            // Simple implementation for demonstration; usually would use a LINQ provider or custom SQL
            return GetAll(); // Should be refined for real search
        }

        public int Add(Customer entity)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    INSERT INTO Customers (FullName, Email, PhoneNumber, Address, KycDocuments, CreatedAt, UpdatedAt)
                    VALUES ($name, $email, $phone, $address, $kyc, $now, $now);
                    SELECT last_insert_rowid();";

                command.Parameters.AddWithValue("$name", entity.FullName);
                command.Parameters.AddWithValue("$email", entity.Email);
                command.Parameters.AddWithValue("$phone", entity.PhoneNumber);
                command.Parameters.AddWithValue("$address", entity.Address ?? "");
                command.Parameters.AddWithValue("$kyc", entity.KycDocuments ?? "");
                command.Parameters.AddWithValue("$now", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        public void Update(Customer entity)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    UPDATE Customers SET
                        FullName = $name, Email = $email, PhoneNumber = $phone,
                        Address = $address, KycDocuments = $kyc, UpdatedAt = $now
                    WHERE Id = $id";

                command.Parameters.AddWithValue("$id", entity.Id);
                command.Parameters.AddWithValue("$name", entity.FullName);
                command.Parameters.AddWithValue("$email", entity.Email);
                command.Parameters.AddWithValue("$phone", entity.PhoneNumber);
                command.Parameters.AddWithValue("$address", entity.Address ?? "");
                command.Parameters.AddWithValue("$kyc", entity.KycDocuments ?? "");
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
                command.CommandText = "DELETE FROM Customers WHERE Id = $id";
                command.Parameters.AddWithValue("$id", id);
                command.ExecuteNonQuery();
            }
        }

        private Customer MapReaderToCustomer(SqliteDataReader reader)
        {
            return new Customer
            {
                Id = reader.GetInt32(0),
                FullName = reader.GetString(1),
                Email = reader.GetString(2),
                PhoneNumber = reader.GetString(3),
                Address = reader.IsDBNull(4) ? "" : reader.GetString(4),
                KycDocuments = reader.IsDBNull(5) ? "" : reader.GetString(5),
                CreatedAt = DateTime.Parse(reader.GetString(6)),
                UpdatedAt = DateTime.Parse(reader.GetString(7))
            };
        }
    }
}
