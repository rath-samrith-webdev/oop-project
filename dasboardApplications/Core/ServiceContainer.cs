using System;
using System.Collections.Generic;
using dasboardApplications.Services;
using dasboardApplications.Interfaces;
using dasboardApplications.Models;

namespace dasboardApplications.Core
{
    public static class ServiceContainer
    {
        private static readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();

        static ServiceContainer()
        {
            var dbService = new DatabaseService();
            var encryptionService = new EncryptionService();

            // Repositories
            var customerRepo = new Repositories.CustomerRepository(dbService);
            var loanRepo = new Repositories.LoanRepository(dbService);
            var paymentRepo = new Repositories.PaymentRepository(dbService);
            var auditRepo = new Repositories.AuditRepository(dbService);
            var scoreRepo = new Repositories.ScoreRepository(dbService);
            var userRepo = new Repositories.UserRepository(dbService);

            // Infrastructure services
            var auditService = new AuditService(auditRepo);
            var authService = new AuthService(userRepo);
            var customerService = new CustomerService(customerRepo, encryptionService, auditService);
            var loanService = new LoanService(loanRepo, customerRepo, auditService);
            var paymentService = new PaymentService(paymentRepo, loanRepo, loanService, auditService);
            var loanCalculatorService = new LoanCalculatorService();
            var validationService = new ValidationService();

            _services[typeof(IDatabaseService)] = dbService;
            _services[typeof(DatabaseService)] = dbService;
            _services[typeof(EncryptionService)] = encryptionService;
            _services[typeof(AuditService)] = auditService;
            _services[typeof(AuthService)] = authService;
            _services[typeof(CustomerService)] = customerService;
            _services[typeof(LoanService)] = loanService;
            _services[typeof(PaymentService)] = paymentService;
            _services[typeof(LoanCalculatorService)] = loanCalculatorService;
            _services[typeof(ValidationService)] = validationService;

            _services[typeof(IRepository<AuditLog>)] = auditRepo;
            _services[typeof(IRepository<ScoreRecord>)] = scoreRepo;
            _services[typeof(IRepository<User>)] = userRepo;
            _services[typeof(IRepository<Customer>)] = customerRepo;
            _services[typeof(IRepository<LoanModel>)] = loanRepo;
            _services[typeof(IRepository<Payment>)] = paymentRepo;
        }

        public static T GetService<T>()
        {
            return (T)_services[typeof(T)];
        }
    }
}
