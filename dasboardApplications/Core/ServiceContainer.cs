using System;
using System.Collections.Generic;
using dasboardApplications.Interfaces;
using dasboardApplications.Services;

namespace dasboardApplications.Core
{
    /// <summary>
    /// Simple Service Locator / DI Container for the application.
    /// Manages the lifecycle of core services.
    /// </summary>
    public static class ServiceContainer
    {
        private static readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();

        static ServiceContainer()
        {
            // Register default services
            Register<IDatabaseService>(new DatabaseService());
        }

        public static void Register<T>(T service) where T : class
        {
            _services[typeof(T)] = service;
        }

        public static T Get<T>() where T : class
        {
            if (_services.TryGetValue(typeof(T), out var service))
            {
                return (T)service;
            }
            throw new Exception($"Service {typeof(T).Name} not registered.");
        }
    }
}
