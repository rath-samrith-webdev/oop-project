using System;
using System.Collections.Generic;
using dasboardApplications.Services;
using dasboardApplications.Interfaces;

namespace dasboardApplications.Core
{
    public static class ServiceContainer
    {
        private static readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();

        static ServiceContainer()
        {
            var dbService = new DatabaseService();
            var encryptionService = new EncryptionService();
            var auditService = new AuditService(dbService);
            var authService = new AuthService(dbService);
            var customerService = new CustomerService(dbService, encryptionService, auditService);
            var loanService = new LoanService(dbService, auditService);
            var paymentService = new PaymentService(dbService, loanService, auditService);
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
        }

        public static T GetService<T>()
        {
            return (T)_services[typeof(T)];
        }
    }
}
