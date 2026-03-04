using System;
using Microsoft.Data.Sqlite;
using dasboardApplications.Models;
using dasboardApplications.Interfaces;

namespace dasboardApplications.Services
{
    public class AuditService
    {
        private readonly IRepository<AuditLog> _auditRepository;

        public AuditService(IRepository<AuditLog> auditRepository)
        {
            _auditRepository = auditRepository;
        }

        public void LogAction(string action, string entityName, int entityId, string changes)
        {
            int userId = AuthService.CurrentUser?.Id ?? 0;

            var log = new AuditLog
            {
                UserId = userId,
                Action = action,
                EntityName = entityName,
                EntityId = entityId,
                Changes = changes,
                Timestamp = DateTime.Now
            };

            _auditRepository.Add(log);
        }
    }
}
