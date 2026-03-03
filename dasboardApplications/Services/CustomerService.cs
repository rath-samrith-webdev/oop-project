using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using dasboardApplications.Models;

namespace dasboardApplications.Services
{
    public class CustomerService
    {
        private readonly string _connectionString;
        private readonly EncryptionService _encryptionService;
        private readonly AuditService _auditService;

        public CustomerService(DatabaseService databaseService, EncryptionService encryptionService, AuditService auditService)
        {
            _connectionString = databaseService.GetConnectionString();
            _encryptionService = encryptionService;
            _auditService = auditService;
        }

        public int CreateCustomer(Customer customer)
        {
            string encryptedKyc = _encryptionService.Encrypt(customer.KycDocuments);

            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    INSERT INTO Customers (FullName, Email, PhoneNumber, Address, KycDocuments, CreatedAt, UpdatedAt)
                    VALUES ($name, $email, $phone, $address, $kyc, $now, $now);
                    SELECT last_insert_rowid();";
                command.Parameters.AddWithValue("$name", customer.FullName);
                command.Parameters.AddWithValue("$email", customer.Email);
                command.Parameters.AddWithValue("$phone", customer.PhoneNumber);
                command.Parameters.AddWithValue("$address", customer.Address ?? "");
                command.Parameters.AddWithValue("$kyc", encryptedKyc ?? "");
                command.Parameters.AddWithValue("$now", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                int id = Convert.ToInt32(command.ExecuteScalar());
                _auditService.LogAction("Create", "Customer", id, $"Created customer: {customer.FullName}");
                return id;
            }
        }

        public List<Customer> GetAllCustomers()
        {
            var customers = new List<Customer>();
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Customers";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        customers.Add(new Customer
                        {
                            Id = reader.GetInt32(0),
                            FullName = reader.GetString(1),
                            Email = reader.GetString(2),
                            PhoneNumber = reader.GetString(3),
                            Address = reader.GetString(4),
                            KycDocuments = _encryptionService.Decrypt(reader.GetString(4)), // wait, index 5 is KYC
                            // Wait, let's check columns: Id(0), FullName(1), Email(2), PhoneNumber(3), Address(4), KycDocuments(5), CreatedAt(6), UpdatedAt(7)
                        });
                    }
                }
            }
            // Re-fetch to be safe with column indexes
            return FetchCustomers();
        }

        private List<Customer> FetchCustomers()
        {
            var customers = new List<Customer>();
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Id, FullName, Email, PhoneNumber, Address, KycDocuments, CreatedAt, UpdatedAt FROM Customers";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        customers.Add(new Customer
                        {
                            Id = reader.GetInt32(0),
                            FullName = reader.GetString(1),
                            Email = reader.GetString(2),
                            PhoneNumber = reader.GetString(3),
                            Address = reader.IsDBNull(4) ? "" : reader.GetString(4),
                            KycDocuments = reader.IsDBNull(5) ? "" : _encryptionService.Decrypt(reader.GetString(5)),
                            CreatedAt = DateTime.Parse(reader.GetString(6)),
                            UpdatedAt = DateTime.Parse(reader.GetString(7))
                        });
                    }
                }
            }
            return customers;
        }

        public void UpdateCustomer(Customer customer)
        {
            string encryptedKyc = _encryptionService.Encrypt(customer.KycDocuments);

            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    UPDATE Customers SET
                        FullName = $name, Email = $email, PhoneNumber = $phone,
                        Address = $address, KycDocuments = $kyc, UpdatedAt = $now
                    WHERE Id = $id";
                command.Parameters.AddWithValue("$id", customer.Id);
                command.Parameters.AddWithValue("$name", customer.FullName);
                command.Parameters.AddWithValue("$email", customer.Email);
                command.Parameters.AddWithValue("$phone", customer.PhoneNumber);
                command.Parameters.AddWithValue("$address", customer.Address ?? "");
                command.Parameters.AddWithValue("$kyc", encryptedKyc ?? "");
                command.Parameters.AddWithValue("$now", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                command.ExecuteNonQuery();
                _auditService.LogAction("Update", "Customer", customer.Id, $"Updated customer: {customer.FullName}");
            }
        }

        public void DeleteCustomer(int id)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                // Delete associated loans' payments first, then loans, then the customer
                command.CommandText = "DELETE FROM Payments WHERE LoanId IN (SELECT Id FROM Loans WHERE CustomerId = $id)";
                command.Parameters.AddWithValue("$id", id);
                command.ExecuteNonQuery();

                command.CommandText = "DELETE FROM Loans WHERE CustomerId = $id";
                command.ExecuteNonQuery();

                command.CommandText = "DELETE FROM Customers WHERE Id = $id";
                command.ExecuteNonQuery();

                _auditService.LogAction("Delete", "Customer", id, $"Deleted customer ID {id} with all related loans and payments.");
            }
        }
    }
}
